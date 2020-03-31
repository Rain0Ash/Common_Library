// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using Common_Library.Utils;

namespace Common_Library.Types.Map
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class IndexMap<TKey, TValue> : Map<TKey, TValue>, IIndexMap<TKey, TValue>
    {
        private List<TKey> _orderList;

        public IReadOnlyList<TKey> OrderedKeys
        {
            get
            {
                return _orderList;
            }
        }

        public IndexMap()
        {
            _orderList = new List<TKey>();
        }

        public IndexMap(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            _orderList = new List<TKey>(dictionary.Keys);
        }

        public IndexMap(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? keyComparer,
            IEqualityComparer<TValue>? valueComparer)
            : base(dictionary, keyComparer, valueComparer)
        {
            _orderList = new List<TKey>(dictionary.Keys);
        }

        public IndexMap(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            : base(keyComparer, valueComparer)
        {
            _orderList = new List<TKey>();
        }

        public IndexMap(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : base(collection)
        {
            _orderList = new List<TKey>(collection.Select(pair => pair.Key));
        }

        public IndexMap(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? keyComparer,
            IEqualityComparer<TValue>? valueComparer)
            : base(collection, keyComparer, valueComparer)
        {
            _orderList = new List<TKey>(collection.Select(pair => pair.Key));
        }

        public IndexMap(Int32 capacity)
            : base(capacity)
        {
            _orderList = new List<TKey>(capacity);
        }

        public IndexMap(Int32 capacity, IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            : base(capacity, keyComparer, valueComparer)
        {
            _orderList = new List<TKey>(capacity);
        }

        protected IndexMap(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _orderList = new List<TKey>();
        }

        public TValue GetByIndex(Int32 index)
        {
            return this[GetReversedByIndex(index)];
        }

        public TKey GetReversedByIndex(Int32 index)
        {
            return _orderList[index];
        }

        public KeyValuePair<TKey, TValue> GetPairByIndex(Int32 index)
        {
            return this.GetPair(GetReversedByIndex(index));
        }

        public KeyValuePair<TValue, TKey> GetReversedPairByIndex(Int32 index)
        {
            return Reversed.GetPair(GetByIndex(index));
        }

        public Boolean TryGetPairByIndex(Int32 index, out KeyValuePair<TKey, TValue> pair)
        {
            return this.TryGetPair(GetReversedByIndex(index), out pair);
        }

        public Boolean TryGetReversedPairByIndex(Int32 index, out KeyValuePair<TValue, TKey> pair)
        {
            return Reversed.TryGetPair(GetByIndex(index), out pair);
        }

        public Int32 IndexOf(TKey key)
        {
            return _orderList.IndexOf(key);
        }

        public Int32 IndexOf(TValue key)
        {
            return IndexOf(Reversed[key]);
        }

        public void Insert(TKey key, TValue value)
        {
            Insert(0, key, value);
        }

        public void Insert(Int32 index, TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                return;
            }

            Add(key, value);
            _orderList.Insert(index, key);
        }

        public void Insert(TValue key, TKey value)
        {
            Insert(0, key, value);
        }

        public void Insert(Int32 index, TValue key, TKey value)
        {
            if (ContainsKey(value))
            {
                return;
            }

            Add(key, value);
            _orderList.Insert(index, value);
        }

        public Boolean TryInsert(TKey key, TValue value)
        {
            return TryInsert(0, key, value);
        }

        public Boolean TryInsert(Int32 index, TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (ContainsKey(key) || index < 0 || index >= _orderList.Count)
            {
                return false;
            }

            Insert(index, key, value);
            return true;
        }

        public Boolean TryInsert(TValue key, TKey value)
        {
            return TryInsert(0, key, value);
        }

        public Boolean TryInsert(Int32 index, TValue key, TKey value)
        {
            return TryInsert(index, value, key);
        }

        public void Swap(Int32 index1, Int32 index2)
        {
            _orderList.Swap(index1, index2);
        }

        public new void Remove(TKey key)
        {
            base.Remove(key);
            _orderList.Remove(key);
        }

        public new void Remove(TKey key, out TValue value)
        {
            base.Remove(key, out value);
            _orderList.Remove(key);
        }

        public new void Remove(TValue key)
        {
            Remove(key, out _);
        }

        public new void Remove(TValue key, out TKey value)
        {
            if (TryGetValue(key, out value))
            {
                _orderList.Remove(value);
            }

            base.Remove(key, out value);
        }

        public void Reverse()
        {
            _orderList.Reverse();
        }

        public void Reverse(Int32 index, Int32 count)
        {
            _orderList.Reverse(index, count);
        }

        public void Sort()
        {
            _orderList.Sort();
        }

        public void Sort(Comparison<TKey> comparison)
        {
            _orderList.Sort(comparison);
        }

        public void Sort(IComparer<TKey>? comparer)
        {
            _orderList.Sort(comparer);
        }

        public void Sort(Int32 index, Int32 count, IComparer<TKey>? comparer)
        {
            _orderList.Sort(index, count, comparer);
        }

        public new void Clear()
        {
            base.Clear();
            _orderList.Clear();
        }

        public new TValue this[TKey key]
        {
            get
            {
                return base[key];
            }
            set
            {
                if (!ContainsKey(key))
                {
                    _orderList.Add(key);
                }

                base[key] = value;
            }
        }

        public new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _orderList.Select(key => new KeyValuePair<TKey, TValue>(key, this[key])).GetEnumerator();
        }

        public new IEnumerator<KeyValuePair<TValue, TKey>> GetReversedEnumerator()
        {
            return _orderList.Select(key => new KeyValuePair<TValue, TKey>(this[key], key)).GetEnumerator();
        }

        public IEnumerator<TKey> GetKeyEnumerator()
        {
            return _orderList.GetEnumerator();
        }

        public IEnumerator<TValue> GetValueEnumerator()
        {
            return _orderList.Select(key => this[key]).GetEnumerator();
        }
    }
}