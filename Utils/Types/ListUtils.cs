// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Common_Library.Utils
{
    public static class ListUtils
    {
        public static void Swap<T>(ref IList<T> source, Int32 x1, Int32 x2)
        {
            T temp = source[x1];
            source[x1] = source[x2];
            source[x2] = temp;
        }

        public static void Swap<T>(ref T[] source, Int32 inx1, Int32 inx2)
        {
            T temp = source[inx1];
            source[inx1] = source[inx2];
            source[inx2] = temp;
        }
        
        public static void Swap<T>(ref T[,] source, Point point1, Point point2)
        {
            Swap(ref source, point1.X, point1.Y, point2.X, point2.Y);
        }
        
        public static void Swap<T>(ref T[,] source, Int32 x1, Int32 y1, Int32 x2, Int32 y2)
        {
            T temp = source[x1, y1];
            source[x1, y1] = source[x2, y2];
            source[x2, y2] = temp;
        }
    }
}