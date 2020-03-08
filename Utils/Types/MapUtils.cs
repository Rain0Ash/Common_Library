// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using Common_Library.Types.Map;

namespace Common_Library.Utils
{
    public static class MapUtils
    {
        public static Boolean TryGetValue<TKey, TValue>(this IReadOnlyMap<TKey, TValue> map, TValue key, out TKey result)
        {
            return map.TryGetValue(key, out result);
        }
        
        public static TKey TryGetValue<TKey, TValue>(this IReadOnlyMap<TKey, TValue> map, TValue key, TKey defaultValue = default)
        {
            return TryGetValue(map, key, out TKey result) ? result : defaultValue;
        }
    }
}