using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AAEmu.Commons.Utils;
using Common_Library.Utils.Math;
using MySql.Data.MySqlClient;

namespace AAEmu.Login.Utils
{
    public class IdManager
    {
        private BitSet _freeIds;
        private Int32 _freeIdCount;
        private Int32 _nextFreeId;

        private readonly String _name;
        private readonly UInt32 _firstId;
        private readonly UInt32 _lastId;
        private readonly UInt32[] _exclude;
        private readonly Int32 _freeIdSize;
        private readonly String[,] _objTables;
        private readonly Boolean _distinct;
        private readonly Object _lock = new Object();

        public IdManager(String name, UInt32 firstId, UInt32 lastId, String[,] objTables, UInt32[] exclude,
            Boolean distinct = false)
        {
            _name = name;
            _firstId = firstId;
            _lastId = lastId;
            _objTables = objTables;
            _exclude = exclude;
            _distinct = distinct;
            _freeIdSize = (Int32) (_lastId - _firstId);
        }

        public Boolean Initialize()
        {
            try
            {
                _freeIds = new BitSet(PrimeUtils.NextPrime(100000));
                _freeIds.Clear();
                _freeIdCount = _freeIdSize;

                foreach (UInt32 usedObjectId in ExtractUsedObjectIdTable())
                {
                    if (_exclude.Contains(usedObjectId))
                    {
                        continue;
                    }

                    Int32 objectId = (Int32) (usedObjectId - _firstId);
                    if (usedObjectId < _firstId)
                    {
#if NLOG
                        _log.Warn("{0}: Object ID {1} in DB is less than {2}", _name, usedObjectId, _firstId);
#endif
                        continue;
                    }

                    if (objectId >= _freeIds.Count)
                    {
                        IncreaseBitSetCapacity(objectId + 1);
                    }

                    _freeIds.Set(objectId);
                    Interlocked.Decrement(ref _freeIdCount);
                }

                _nextFreeId = _freeIds.NextClear(0);
#if NLOG
                _log.Info("{0} successfully initialized", _name);
#endif
            }
            catch (Exception e)
            {
#if NLOG
                _log.Error("{0} could not be initialized correctly", _name);
                _log.Error(e);
#endif
                return false;
            }

            return true;
        }

        private IEnumerable<UInt32> ExtractUsedObjectIdTable()
        {
            if (_objTables.Length < 2)
            {
                return new UInt32[0];
            }

            using (MySqlConnection connection = new MySqlConnection())
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    String query = "SELECT " + (_distinct ? "DISTINCT " : "") + _objTables[0, 1] + ", 0 AS i FROM " +
                                   _objTables[0, 0];
                    for (Int32 i = 1; i < _objTables.Length / 2; i++)
                    {
                        query += " UNION SELECT " + (_distinct ? "DISTINCT " : "") + _objTables[i, 1] + ", " + i +
                                 " FROM " + _objTables[i, 0];
                    }

                    command.CommandText = "SELECT COUNT(*), COUNT(DISTINCT " + _objTables[0, 1] + ") FROM ( " + query +
                                          " ) AS all_ids";
                    command.Prepare();
                    Int32 count;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            throw new Exception("IdManager: can't extract count ids");
                        }

                        if (reader.GetInt32(0) != reader.GetInt32(1) && !_distinct)
                        {
                            throw new Exception("IdManager: there are duplicates in object ids");
                        }

                        count = reader.GetInt32(0);
                    }

                    if (count == 0)
                    {
                        return new UInt32[0];
                    }

                    UInt32[] result = new UInt32[count];

#if NLOG
                    _log.Info("{0}: Extracting {1} used id's from data tables...", _name, count);
#endif

                    command.CommandText = query;
                    command.Prepare();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        Int32 idx = 0;
                        while (reader.Read())
                        {
                            result[idx] = reader.GetUInt32(0);
                            idx++;
                        }

#if NLOG
                        _log.Info("{0}: Successfully extracted {1} used id's from data tables.", _name, idx);
#endif
                    }

                    return result;
                }
            }
        }

        public virtual void ReleaseId(UInt32 usedObjectId)
        {
            Int32 objectId = (Int32) (usedObjectId - _firstId);
            if (objectId > -1)
            {
                _freeIds.Clear(objectId);
                if (_nextFreeId > objectId)
                {
                    _nextFreeId = objectId;
                }

                Interlocked.Increment(ref _freeIdCount);
            }
            else
            {
#if NLOG
                _log.Warn("{0}: release objectId {1} failed", _name, usedObjectId);
#endif
            }
        }

        public virtual void ReleaseId(IEnumerable<UInt32> usedObjectIds)
        {
            foreach (UInt32 id in usedObjectIds)
            {
                ReleaseId(id);
            }
        }

        public UInt32 GetNextId()
        {
            lock (_lock)
            {
                Int32 newId = _nextFreeId;
                _freeIds.Set(newId);
                Interlocked.Decrement(ref _freeIdCount);

                Int32 nextFree = _freeIds.NextClear(newId);

                while (nextFree < 0)
                {
                    nextFree = _freeIds.NextClear(0);
                    if (nextFree >= 0)
                    {
                        continue;
                    }

                    if (_freeIds.Count < _freeIdSize)
                    {
                        IncreaseBitSetCapacity();
                    }
                    else
                    {
                        throw new Exception("Ran out of valid Id's.");
                    }
                }

                _nextFreeId = nextFree;
                return (UInt32) newId + _firstId;
            }
        }

        public UInt32[] GetNextId(Int32 count)
        {
            UInt32[] res = new UInt32[count];
            for (Int32 i = 0; i < count; i++)
            {
                res[i] = GetNextId();
            }

            return res;
        }

        private void IncreaseBitSetCapacity()
        {
            Int32 size = PrimeUtils.NextPrime(_freeIds.Count + _freeIdSize / 10);
            if (size > _freeIdSize)
            {
                size = _freeIdSize;
            }

            BitSet newBitSet = new BitSet(size);
            newBitSet.Or(_freeIds);
            _freeIds = newBitSet;
        }

        private void IncreaseBitSetCapacity(Int32 count)
        {
            Int32 size = PrimeUtils.NextPrime(count);
            if (size > _freeIdSize)
            {
                size = _freeIdSize;
            }

            BitSet newBitSet = new BitSet(size);
            newBitSet.Or(_freeIds);
            _freeIds = newBitSet;
        }
    }
}