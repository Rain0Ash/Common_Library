// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Collections.Generic.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using Common_Library.Utils;

namespace System.Collections.Generic
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class IndexDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IIndexDictionary<TKey, TValue>
    {
        private List<TKey> _orderList;

        public IReadOnlyList<TKey> OrderedKeys
        {
            get
            {
                return _orderList;
            }
        }

        public IndexDictionary()
        {
            _orderList = new List<TKey>();
        }

        public IndexDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            _orderList = new List<TKey>(dictionary.Keys);
        }

        public IndexDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer)
            : base(dictionary, comparer)
        {
            _orderList = new List<TKey>(dictionary.Keys);
        }

        public IndexDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : base(collection)
        {
            _orderList = new List<TKey>(collection.Select(pair => pair.Key));
        }

        public IndexDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer)
            : base(collection, comparer)
        {
            _orderList = new List<TKey>(collection.Select(pair => pair.Key));
        }

        public IndexDictionary(IEqualityComparer<TKey>? comparer)
            : base(comparer)
        {
            _orderList = new List<TKey>();
        }

        public IndexDictionary(Int32 capacity)
            : base(capacity)
        {
            _orderList = new List<TKey>(capacity);
        }

        public IndexDictionary(Int32 capacity, IEqualityComparer<TKey>? comparer)
            : base(capacity, comparer)
        {
            _orderList = new List<TKey>(capacity);
        }

        protected IndexDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _orderList = new List<TKey>();
        }

        public TValue GetByIndex(Int32 index)
        {
            return this[_orderList[index]];
        }

        public KeyValuePair<TKey, TValue> GetPairByIndex(Int32 index)
        {
            return this.GetPair(_orderList[index]);
        }

        public Boolean TryGetPairByIndex(Int32 index, out KeyValuePair<TKey, TValue> pair)
        {
            return this.TryGetPair(_orderList[index], out pair);
        }

        public Int32 IndexOf(TKey key)
        {
            return _orderList.IndexOf(key);
        }

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            _orderList.Add(key);
        }

        public new Boolean TryAdd(TKey key, TValue value)
        {
            if (!base.TryAdd(key, value))
            {
                return false;
            }

            _orderList.Add(key);
            return true;
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

            base.Add(key, value);
            _orderList.Insert(index, key);
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

        public void Swap(Int32 index1, Int32 index2)
        {
            ListUtils.Swap(ref _orderList, index1, index2);
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