// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    public static class ArrayUtils
    {
        public static void Swap<T>(ref T[] source, Int32 inx1, Int32 inx2)
        {
            T temp = source[inx1];
            source[inx1] = source[inx2];
            source[inx2] = temp;
        }
        
        public static void Swap<T>(ref T[,] source, Int32 inx1, Int32 y1, Int32 x2, Int32 y2)
        {
            T temp = source[inx1, y1];
            source[inx1, y1] = source[x2, y2];
            source[x2, y2] = temp;
        }
        
        public static void Default(this Array array)
        {
            Array.Clear(array, 0, array.Length);
        }
    }
}