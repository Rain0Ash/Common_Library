// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;

namespace Common_Library.Utils
{
    public static class ListUtils
    {
        public static void Swap<T>(ref List<T> source, Int32 inx1, Int32 inx2)
        {
            T temp = source[inx1];
            source[inx1] = source[inx2];
            source[inx2] = temp;
        }
        
        public static void Swap<T>(ref IList<T> source, Int32 inx1, Int32 inx2)
        {
            T temp = source[inx1];
            source[inx1] = source[inx2];
            source[inx2] = temp;
        }
    }
}