// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    public static class ArrayUtils
    {
        public static void Default(Array array)
        {
            Array.Clear(array, 0, array.Length);
        }
    }
}