﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    public static partial class PrimeUtils
    {
        public const Int32 LargestPrime = Int32.MaxValue;

        static PrimeUtils()
        {
            Init();
            Array.Sort(PrimeCapacities);
        }

        public static Int32 NextPrime(Int32 desiredCapacity)
        {
            Int32 i = Array.BinarySearch(PrimeCapacities, desiredCapacity);
            if (i < 0)
            {
                i = -i - 1;
            }

            return PrimeCapacities[i];
        }
    }
}