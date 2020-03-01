// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;

namespace Common_Library.Utils
{
    public static class DictionaryUtils
    {
        public static Boolean TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue result, TValue defaultValue = default)
        {
            try
            {
                result = dictionary[key];
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                result = defaultValue;
                return false;
            }
        }
        
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> list, TKey key, TValue defaultValue = default)
        {
            return TryGetValue(list, key, out TValue result) ? result : defaultValue;
        }
        
        public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            Dictionary<TValue, TKey> dictionary = new Dictionary<TValue, TKey>();
            foreach (KeyValuePair<TKey, TValue> entry in source)
            {
                if(!dictionary.ContainsKey(entry.Value))
                {
                    dictionary.Add(entry.Value, entry.Key);
                }
            }
            
            return dictionary;
        }
        
        public static Dictionary<TKey, TValue> Clone<TKey, TValue> (this Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
            
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue) entry.Value.Clone());
            }
            
            return ret;
        }
    }
}