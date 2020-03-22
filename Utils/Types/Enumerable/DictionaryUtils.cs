// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Library.Utils
{
    public static class DictionaryUtils
    {
        public static Boolean TryGetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, out TValue result,
            TValue defaultValue = default)
        {
            try
            {
                result = dictionary[key];
                return true;
            }
            catch (KeyNotFoundException)
            {
                result = defaultValue;
                return false;
            }
        }

        public static TValue TryGetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            return TryGetValue(dictionary, key, out TValue result) ? result : defaultValue;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.TryGetValue(key, out TValue val))
            {
                return val;
            }

            dictionary.Add(key, value);
            return value;
        }

        public static KeyValuePair<TKey, TValue> GetPair<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            return new KeyValuePair<TKey, TValue>(key, dictionary[key]);
        }

        public static Boolean TryGetPair<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key,
            out KeyValuePair<TKey, TValue> pair)
        {
            if (dictionary.ContainsKey(key))
            {
                pair = GetPair(dictionary, key);
                return true;
            }

            pair = default;
            return false;
        }

        public static IList<KeyValuePair<TKey, TValue>> GetPairs<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            return dictionary.ToList();
        }

        public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            Dictionary<TValue, TKey> dictionary = new Dictionary<TValue, TKey>();
            foreach ((TKey key, TValue value) in source)
            {
                if (!dictionary.ContainsKey(value))
                {
                    dictionary.Add(value, key);
                }
            }

            return dictionary;
        }

        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count, original.Comparer);

            foreach ((TKey key, TValue value) in original)
            {
                ret.Add(key, (TValue) value.Clone());
            }

            return ret;
        }

        public static void CopyTo<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue>[] array, Int32 arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }
    }
}