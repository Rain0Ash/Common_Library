// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common_Library.Utils;

namespace Common_Library.Types.Map
{
    public class Map<TKey, TValue> : Dictionary<TKey, TValue>, IMap<TKey, TValue>
    {
        private Dictionary<TValue, TKey> Reverse { get; }

        public Map()
        {
            Reverse = new Dictionary<TValue, TKey>();
        }

        public Map(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            Reverse = new Dictionary<TValue, TKey>(dictionary.Reverse());
        }
        
        public Map(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : base(dictionary, keyComparer)
        {
            Reverse = new Dictionary<TValue, TKey>(dictionary.Reverse(), valueComparer);
        }
        
        public Map(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : base(keyComparer)
        {
            Reverse = new Dictionary<TValue, TKey>(valueComparer);
        }
        
        public Map(Int32 capacity)
            : base(capacity)
        {
            Reverse = new Dictionary<TValue, TKey>(capacity);
        }
        
        public Map(Int32 capacity, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : base(capacity, keyComparer)
        {
            Reverse = new Dictionary<TValue, TKey>(capacity, valueComparer);
        }
        

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            Reverse.Add(value, key);
        }
        
        public void Add(TValue key, TKey value)
        {
            Add(value, key);
        }
        
        public new Boolean TryAdd(TKey key, TValue value)
        {
            if (!base.ContainsKey(key) && !Reverse.ContainsKey(value))
            {
                return base.TryAdd(key, value) && Reverse.TryAdd(value, key);
            }

            return false;
        }
        
        public Boolean TryAdd(TValue key, TKey value)
        {
            return TryAdd(value, key);
        }
        
        public new void Remove(TKey key)
        {
            Remove(key, out _);
        }
        
        public new void Remove(TKey key, out TValue value)
        {
            if (TryGetValue(key, out value))
            {
                return;
            }

            base.Remove(key);
            Reverse.Remove(value);
        }
        
        public void Remove(TValue key)
        {
            Remove(key, out _);
        }
        
        public void Remove(TValue key, out TKey value)
        {
            if (!Reverse.TryGetValue(key, out value))
            {
                return;
            }

            Remove(value);
            Reverse.Remove(key);
        }

        public Boolean ContainsKey(TValue value)
        {
            return Reverse.ContainsKey(value);
        }

        public Boolean TryGetValue(TValue key, out TKey value)
        {
            return Reverse.TryGetValue(key, out value);
        }
        
        public new void Clear()
        {
            base.Clear();
            Reverse.Clear();
        }
        
        public new TValue this[TKey key]
        {
            get
            {
                return base[key];
            }
            set
            {
                base[key] = value;
                Reverse[value] = key;
            }
        }

        public TKey this[TValue key]
        {
            get
            {
                return Reverse[key];
            }
            set
            {
                Reverse[key] = value;
                base[value] = key;
            }
        }

        public IReadOnlyDictionary<TKey, TValue> Get()
        {
            return new ReadOnlyDictionary<TKey, TValue>(this);
        }
        
        public IReadOnlyDictionary<TValue, TKey> GetReverse()
        {
            return new ReadOnlyDictionary<TValue, TKey>(Reverse);
        }
    }
}