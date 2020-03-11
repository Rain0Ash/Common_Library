// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;

namespace Common_Library.Utils
{
    public static class GenericUtils<T>
    {
        public static Boolean TryGetValue(IList<T> list, Int32 index, out T result, T defaultValue = default)
        {
            try
            {
                result = list[index];
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                result = defaultValue;
                return false;
            }
        }

        public static T TryGetValue(IList<T> list, Int32 index, T defaultValue = default)
        {
            return TryGetValue(list, index, out T result) ? result : defaultValue;
        }
    }
}