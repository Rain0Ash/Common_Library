// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections;
using System.Collections.Generic;

namespace Common_Library.Utils
{
    public static class ListUtils
    {
        public static T TryGetValue<T>(this IReadOnlyList<T> collection, Int32 index, T defaultValue = default)
        {
            return collection.TryGetValue(index, out T value) ? value : defaultValue;
        }

        public static Boolean TryGetValue<T>(this IReadOnlyList<T> collection, Int32 index, out T value)
        {
            if (collection.InBounds(index))
            {
                value = collection[index];
                return true;
            }

            value = default;
            return false;
        }

        public static void Swap(IList source, Int32 inx1, Int32 inx2)
        {
            Object temp = source[inx1];
            source[inx1] = source[inx2];
            source[inx2] = temp;
        }
        
        public static Boolean TrySwap(IList source, Int32 inx1, Int32 inx2)
        {
            try
            {
                Swap(source, inx1, inx2);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Swap<T>(this IList<T> source, Int32 inx1, Int32 inx2)
        {
            T temp = source[inx1];
            source[inx1] = source[inx2];
            source[inx2] = temp;
        }
        
        public static Boolean TrySwap<T>(this IList<T> source, Int32 inx1, Int32 inx2)
        {
            try
            {
                Swap(source, inx1, inx2);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}