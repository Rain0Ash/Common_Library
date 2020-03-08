// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;

namespace Common_Library.Utils
{
    public static class CollectionUtils
    {
        public static Boolean InBounds<T>(this IReadOnlyCollection<T> collection, Int32 index)
        {
            return index > 0 && index < collection.Count;
        }
    }
}