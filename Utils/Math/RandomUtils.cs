// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using Common_Library.Random;

namespace Common_Library.Utils.Math
{
    public static class RandomUtils
    {
        private static readonly MersenneTwister Random = new MersenneTwister(DateTime.UtcNow.Millisecond);
        private static readonly Object Lock = new Object();

        public static Int32 Next()
        {
            lock (Lock)
            {
                return Random.Next();
            }
        }

        public static Int32 Next(Int32 maxValue)
        {
            lock (Lock)
            {
                return Random.Next(maxValue);
            }
        }

        public static Int32 Next(Int32 minValue, Int32 maxValue)
        {
            lock (Lock)
            {
                return Random.Next(minValue, maxValue);
            }
        }

        public static Double NextDouble()
        {
            lock (Lock)
            {
                return Random.NextDouble(true);
            }
        }

        public static Single NextSingle()
        {
            lock (Lock)
            {
                return Random.NextSingle(true);
            }
        }

        public static Single Next(Single maxValue)
        {
            lock (Lock)
            {
                return Random.NextSingle(true) * maxValue;
            }
        }

        public static Single Next(Single minValue, Single maxValue)
        {
            lock (Lock)
            {
                return Random.NextSingle(true) * (maxValue - minValue) + minValue;
            }
        }
    }
}