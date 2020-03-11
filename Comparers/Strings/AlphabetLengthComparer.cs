// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Comparers
{
    public class AlphabetLengthComparer : System.Collections.IComparer
    {
        public Int32 Compare(Object? x, Object? y)
        {
            if (x == null || y == null)
            {
                return -2;
            }

            if (x.ToString().Length == y.ToString().Length)
            {
                return String.CompareOrdinal(x.ToString(), y.ToString());
            }

            if (x.ToString().Length > y.ToString().Length)
            {
                return 1;
            }

            return -1;
        }
    }
}