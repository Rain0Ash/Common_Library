// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;

namespace Common_Library.Utils
{
    public static class LogicUtils
    {
        
    }
    
    public static class Gates
    {
        public static Boolean And(params Boolean[] values)
        {
            return values.Length switch
            {
                0 => false,
                1 => values[0],
                _ => values.All(boolean => boolean)
            };
        }

        public static Boolean Or(params Boolean[] values)
        {
            return values.Length switch
            {
                0 => false,
                1 => values[0],
                _ => values.Any(boolean => boolean)
            };
        }
        
        public static Boolean Xor(params Boolean[] values)
        {
            return values.Length switch
            {
                0 => false,
                1 => values[0],
                _ => (values.Count(boolean => boolean) & 1) == 1
            };
        }
        
        public static Boolean Nand(params Boolean[] values)
        {
            return !And(values);
        }
        
        public static Boolean Nor(params Boolean[] values)
        {
            return !Or(values);
        }

        public static Boolean Xnor(params Boolean[] values)
        {
            return !Xor(values);
        }
    }
}