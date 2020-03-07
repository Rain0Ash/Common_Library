// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Common_Library.Utils;

namespace Common_Library.Types.Map
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class Map<TKey, TValue> : Dictionary<TKey, TValue>, IMap<TKey, TValue>
    {
        protected Dictionary<TValue, TKey> Reversed { get; }

        public Map()
        {
            Reversed = new Dictionary<TValue, TKey>();
        }

        public Map(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            Reversed = new Dictionary<TValue, TKey>(dictionary.Reverse());
        }
        
        public Map(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            : base(dictionary, keyComparer)
        {
            Reversed = new Dictionary<TValue, TKey>(dictionary.Reverse(), valueComparer);
        }
        
        public Map(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            : base(keyComparer)
        {
            Reversed = new Dictionary<TValue, TKey>(valueComparer);
        }
        
        public Map(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : base(collection)
        {
            Reversed = new Dictionary<TValue, TKey>(collection.Select(pair => new KeyValuePair<TValue, TKey>(pair.Value, pair.Key)));
        }

        public Map(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            : base(collection, keyComparer)
        {
            Reversed = new Dictionary<TValue, TKey>(collection.Select(pair => new KeyValuePair<TValue, TKey>(pair.Value, pair.Key)), valueComparer);
        }
        
        public Map(Int32 capacity)
            : base(capacity)
        {
            Reversed = new Dictionary<TValue, TKey>(capacity);
        }
        
        public Map(Int32 capacity, IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            : base(capacity, keyComparer)
        {
            Reversed = new Dictionary<TValue, TKey>(capacity, valueComparer);
        }
        
        protected Map(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Reversed = new Dictionary<TValue, TKey>();
        }
        
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            Reversed.Add(value, key);
        }
        
        public void Add(TValue key, TKey value)
        {
            Add(value, key);
        }
        
        public new Boolean TryAdd(TKey key, TValue value)
        {
            if (!base.ContainsKey(key) && !Reversed.ContainsKey(value))
            {
                return base.TryAdd(key, value) && Reversed.TryAdd(value, key);
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
            Reversed.Remove(value);
        }
        
        public void Remove(TValue key)
        {
            Remove(key, out _);
        }
        
        public void Remove(TValue key, out TKey value)
        {
            if (!Reversed.TryGetValue(key, out value))
            {
                return;
            }

            Remove(value);
            Reversed.Remove(key);
        }

        public Boolean ContainsKey(TValue value)
        {
            return Reversed.ContainsKey(value);
        }

        public Boolean TryGetValue(TValue key, out TKey value)
        {
            return Reversed.TryGetValue(key, out value);
        }
        
        public new void Clear()
        {
            base.Clear();
            Reversed.Clear();
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
                Reversed[value] = key;
            }
        }

        public TKey this[TValue key]
        {
            get
            {
                return Reversed[key];
            }
            set
            {
                Reversed[key] = value;
                base[value] = key;
            }
        }

        public IReadOnlyDictionary<TKey, TValue> Get()
        {
            return new ReadOnlyDictionary<TKey, TValue>(this);
        }
        
        public IReadOnlyDictionary<TValue, TKey> GetReverse()
        {
            return new ReadOnlyDictionary<TValue, TKey>(Reversed);
        }
    }
}