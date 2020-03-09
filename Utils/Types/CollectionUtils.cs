// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using Common_Library.Combinatorics;

namespace Common_Library.Utils
{
    public static class CollectionUtils
    {
        public static Boolean InBounds<T>(this IReadOnlyCollection<T> collection, Int32 index)
        {
            return index > 0 && index < collection.Count;
        }
        
        public static IList<IList<T>> GetCombinations<T>(this ICollection<T> collection, Int32 minCount = 1)
        {
            return GetCombinations(collection, minCount, collection.Count);
        }
        
        public static IList<IList<T>> GetCombinations<T>(this ICollection<T> collection, Int32 minCount, Int32 maxCount)
        {
            if (minCount < 1 || minCount > collection.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(minCount));
            }
            
            if (maxCount < 1 || maxCount < minCount || maxCount > collection.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCount));
            }
            
            IEnumerable<IList<T>> comboList = new List<List<T>>();
            
            for (Int32 i = minCount; i <= maxCount; i++)
            {
                comboList = comboList.Concat(new Combinations<T>(collection, i));
            }

            return comboList.ToList();
        }
    }
}