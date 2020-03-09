// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Library.Utils
{
    public static class MathEnumerableUtils
    {
        public static IEnumerable<Decimal> ToDecimal(this Array array)
        {
            return array.OfType<IConvertible>().ToDecimal();
        }
        
        public static IEnumerable<Decimal> ToDecimal(this IEnumerable<IConvertible> enumerable)
        {
            return enumerable.Select(Convert.ToDecimal);
        }
    }
}