// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    public static class ComparerUtils
    {
        public static Int32? ToCompare(IComparable first, IComparable second, Boolean isThrow)
        {
            try
            {
                return ToCompare(first, second);
            }
            catch (Exception)
            {
                if (isThrow)
                {
                    throw;
                }

                return null;
            }
        }

        public static Int32 ToCompare(IComparable first, IComparable second)
        {
            Type firstType = first.GetType();
            Type secondType = second.GetType();

            if (firstType == secondType)
            {
                return first.CompareTo(second);
            }

            Decimal fc = first switch
            {
                DateTime dt => Convert.ToDecimal(dt.UnixTime()),
                Char chr => Convert.ToDecimal(Convert.ToInt16(chr)),
                _ => Convert.ToDecimal(first)
            };

            Decimal sc = second switch
            {
                DateTime dt => Convert.ToDecimal(dt.UnixTime()),
                Char chr => Convert.ToDecimal(Convert.ToInt16(chr)),
                _ => Convert.ToDecimal(second)
            };

            return fc.CompareTo(sc);

            /*
            Type castType;
            if (!CastDictionary.TryGetValue((firstType, secondType), out castType) && !CastDictionary.TryGetValue((secondType, firstType), out castType))
            {
                throw new InvalidCastException($"Can't find best type for {firstType} and {secondType} types");
            }
            */
        }
    }
}