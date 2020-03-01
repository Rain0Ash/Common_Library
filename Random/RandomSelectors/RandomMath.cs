using System;
using System.Collections.Generic;

namespace Common_Library.Random {

    public static class RandomMath {

        /// <summary>
        /// Breaking point between using Linear vs. Binary search for arrays (StaticSelector). 
        /// Was calculated empirically.
        /// </summary>
        public static readonly Int32 ArrayBreakpoint = 51;
        
        /// <summary>
        /// Breaking point between using Linear vs. Binary search for lists (DynamicSelector). 
        /// Was calculated empirically. 
        /// </summary>
        public static readonly Int32 ListBreakpoint = 26;
        
        /// <summary>
        /// Builds cummulative distribution out of non-normalized weights inplace.
        /// </summary>
        /// <param name="cdl">List of Non-normalized weights</param>
        public static void BuildCumulativeDistribution(List<Single> cdl) {

            Int32 length = cdl.Count;

            // Use double for more precise calculation
            Double sum = 0;

            // Sum of weights
            for (Int32 i = 0; i < length; i++)
            {
                sum += cdl[i];
            }

            // k is normalization constant
            // calculate inverse of sum and convert to float
            // use multiplying, since it is faster than dividing      
            Double k = 1f / sum;

            sum = 0;

            // Make Cummulative Distribution Array
            for (Int32 i = 0; i < length; i++) {

                sum += cdl[i] * k; //k, the normalization constant is applied here
                cdl[i] = (Single) sum; 
            }

            cdl[length - 1] = 1f; //last item of CDA is always 1, I do this because numerical inaccurarcies add up and last item probably wont be 1

        }

        /// <summary>
        /// Builds cummulative distribution out of non-normalized weights inplace.
        /// </summary>
        /// <param name="cda">Array of Non-normalized weights</param>
        public static void BuildCumulativeDistribution(Single[] cda) {

            Int32 length = cda.Length;

            // Use double for more precise calculation
            Double sum = 0;
            
            // Sum of weights
            for (Int32 i = 0; i < length; i++)
            {
                sum += cda[i];
            }

            // k is normalization constant
            // calculate inverse of sum and convert to float
            // use multiplying, since it is faster than dividing   
            Double k = 1f / sum;

            sum = 0;

            // Make Cummulative Distribution Array
            for (Int32 i = 0; i < length; i++) {

                sum += cda[i] * k; //k, the normalization constant is applied here
                cda[i] = (Single) sum; 
            }

            cda[length - 1] = 1f; //last item of CDA is always 1, I do this because numerical inaccurarcies add up and last item probably wont be 1
        }

        
        /// <summary>
        /// Linear search, good/faster for small arrays
        /// </summary>
        /// <param name="cda">Cummulative Distribution Array</param>
        /// <param name="randomValue">Uniform random value</param>
        /// <returns>Returns index of an value inside CDA</returns>
        public static Int32 SelectIndexLinearSearch(this Single[] cda, Single randomValue) {

            Int32 i = 0;

            // last element, CDA[CDA.Length-1] should always be 1
            while (cda[i] < randomValue)
            {
                i++;
            }

            return i;
        }


        /// <summary>
        /// Binary search, good/faster for big array
        /// Code taken out of C# array.cs Binary Search & modified
        /// </summary>
        /// <param name="cda">Cummulative Distribution Array</param>
        /// <param name="randomValue">Uniform random value</param>
        /// <returns>Returns index of an value inside CDA</returns>
        public static Int32 SelectIndexBinarySearch(this Single[] cda, Single randomValue) {

            Int32 lo = 0;
            Int32 hi = cda.Length - 1;
            Int32 index;

            while (lo <= hi) {

                // calculate median
                index = lo + ((hi - lo) >> 1);

                if (Math.Abs(cda[index] - randomValue) < Single.Epsilon) {
                    return index;
                }
                if (cda[index] < randomValue) {
                    // shrink left
                    lo = index + 1;
                }
                else {
                    // shrink right
                    hi = index - 1;
                }
            }

            index = lo;

            return index;
        }
        
        /// <summary>
        /// Linear search, good/faster for small lists
        /// </summary>
        /// <param name="cdl">Cummulative Distribution List</param>
        /// <param name="randomValue">Uniform random value</param>
        /// <returns>Returns index of an value inside CDA</returns>
        public static Int32 SelectIndexLinearSearch(this List<Single> cdl, Single randomValue) {

            Int32 i = 0;
            
            // last element, CDL[CDL.Length-1] should always be 1
            while (cdl[i] < randomValue)
            {
                i++;
            }

            return i;
        }

        /// <summary>
        /// Binary search, good/faster for big lists
        /// Code taken out of C# array.cs Binary Search & modified
        /// </summary>
        /// <param name="cdl">Cummulative Distribution List</param>
        /// <param name="randomValue">Uniform random value</param>
        /// <returns>Returns index of an value inside CDL</returns>
        public static Int32 SelectIndexBinarySearch(this List<Single> cdl, Single randomValue) {
        
            Int32 lo = 0;
            Int32 hi = cdl.Count - 1;
            Int32 index;

            while (lo <= hi) {

                // calculate median
                index = lo + ((hi - lo) >> 1);

                if (Math.Abs(cdl[index] - randomValue) < Single.Epsilon) {
                    return index;
                }
                if (cdl[index] < randomValue) {
                    // shrink left
                    lo = index + 1;
                }
                else {
                    // shrink right
                    hi = index - 1;
                }
            }

            index = lo;

            return index;
        }
        
        /// <summary>
        /// Returns identity, array[i] = i
        /// </summary>
        /// <param name="length">Length of an array</param>
        /// <returns>Identity array</returns>
        public static Single[] IdentityArray(Int32 length) {

            Single[] array = new Single[length];

            for (Int32 i = 0; i < array.Length; i++)
            {
                array[i] = i;
            }

            return array;
        }

        /// <summary>
        /// Gemerates uniform random values for all indexes in array.
        /// </summary>
        /// <param name="array">The array where all values will be randomized.</param>
        /// <param name="r">Random generator</param>
        public static void RandomWeightsArray(ref Single[] array, System.Random r) {
            
            for (Int32 i = 0; i < array.Length; i++) {
                array[i] = (Single) r.NextDouble();

                if (Math.Abs(array[i]) < Single.Epsilon)
                {
                    i--;
                }
            }
        }

        /// <summary>
        /// Creates new array with uniform random variables. 
        /// </summary>
        /// <param name="r">Random generator</param>
        /// <param name="length">Length of new array</param>
        /// <returns>Array with random uniform random variables</returns>
        public static Single[] RandomWeightsArray(System.Random r, Int32 length) {
        
            Single[] array = new Single[length];

            for (Int32 i = 0; i < length; i++) {
                array[i] = (Single) r.NextDouble();

                if (Math.Abs(array[i]) < Single.Epsilon)
                {
                    i--;
                }
            }
            return array;
        }


        /// <summary>
        /// Returns identity, list[i] = i
        /// </summary>
        /// <param name="length">Length of an list</param>
        /// <returns>Identity list</returns>
        public static List<Single> IdentityList(Int32 length) {

            List<Single> list = new List<Single>(length);

            for (Int32 i = 0; i < length; i++)
            {
                list.Add(i);
            }

            return list;
        }

        /// <summary>
        /// Gemerates uniform random values for all indexes in list.
        /// </summary>
        /// <param name="list">The list where all values will be randomized.</param>
        /// <param name="r">Random generator</param>
        public static void RandomWeightsList(ref List<Single> list, System.Random r) {

            for (Int32 i = 0; i < list.Count; i++) {
                list[i] = (Single) r.NextDouble();

                if (Math.Abs(list[i]) < Single.Epsilon)
                {
                    i--;
                }
            }
        }
    }
}