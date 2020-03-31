// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Common_Library.Random;

namespace Common_Library.Utils
{
    public static class EnumerableUtils
    {
        public static IEnumerable<T> AggregateAdd<T>(this IEnumerable<T> source, Func<T, T, T> aggregateFunc, Func<T, T> addFunc,
            Boolean prepend = false)
        {
            source = source.ToList();

            T value = addFunc(source.Aggregate(aggregateFunc));

            return prepend ? source.Prepend(value) : source.Append(value);
        }

        public static IEnumerable<TOut> SelectWhere<T, TOut>(this IEnumerable<T> source, Func<T, (Boolean, TOut)> func)
        {
            foreach (T item in source)
            {
                (Boolean ok, TOut output) = func(item);

                if (ok)
                {
                    yield return output;
                }
            }
        }

        public static IEnumerable<TOut> SelectWhere<T, TOut>(this IEnumerable<T> source, Func<T, Boolean> where, Func<T, TOut> select)
        {
            return source.Where(where).Select(select);
        }

        public static IEnumerable<T> Sort<T>(this IEnumerable<T> source, IComparer<T> comparer = null)
        {
            return source.OrderBy(item => item, comparer);
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }

            return source;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> ForEachWhere<T>(this IEnumerable<T> source, Func<T, Boolean> where, Action<T> action)
        {
            foreach (T item in source)
            {
                if (where(item))
                {
                    action(item);
                }
            }

            return source;
        }
        
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, Int32> action)
        {
            Int32 index = 0;
            foreach (T item in source)
            {
                action(item, index);
                ++index;
            }

            return source;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> ForEachWhere<T>(this IEnumerable<T> source, Func<T, Boolean> where, Action<T, Int32> action)
        {
            Int32 index = 0;
            foreach (T item in source)
            {
                if (where(item))
                {
                    action(item, index);
                }

                ++index;
            }

            return source;
        }

        public static T FirstOr<T>(this IEnumerable<T> source, T alternate)
        {
            return FirstOr(source, item => true, alternate);
        }

        public static T FirstOr<T>(this IEnumerable<T> source, Func<T, Boolean> predicate, T alternate)
        {
            foreach (T item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return alternate;
        }

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> source, T oldValue, T newValue, IEqualityComparer<T> comparer = null)
        {
            comparer ??= EqualityComparer<T>.Default;

            foreach (T item in source)
            {
                yield return
                    comparer.Equals(item, oldValue)
                        ? newValue
                        : item;
            }
        }
        
        /// <summary>
        /// Splits the given sequence into chunks of the given size.
        /// If the sequence length isn't evenly divisible by the chunk size,
        /// the last chunk will contain all remaining elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <param name="size">The number of elements per chunk.</param>
        /// <returns>The chunked sequence.</returns>
        public static IEnumerable<T[]> Chunk<T>(this IEnumerable<T> source, Int32 size)
        {
            T[] currentChunk = null;
            Int32 currentIndex = 0;

            foreach (T element in source)
            {
                currentChunk ??= new T[size];
                currentChunk[currentIndex++] = element;

                if (currentIndex != size)
                {
                    continue;
                }

                yield return currentChunk;
                currentIndex = 0;
                currentChunk = null;
            }

            // Do we have an incomplete chunk of remaining elements?
            if (currentChunk == null)
            {
                yield break;
            }

            // This last chunk is incomplete, otherwise it would've been returned already.
            // Thus, we have to create a new, shorter array to hold the remaining elements.
            T[] lastChunk = new T[currentIndex];
            Array.Copy(currentChunk, lastChunk, currentIndex);

            yield return lastChunk;
        }

        /// <summary>
        /// Turns a finite sequence into a circular one, or equivalently,
        /// repeats the original sequence indefinitely.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to cycle through.</param>
        /// <param name="count">Count of repeat, 0 is default IEnumerable</param>
        /// <returns>An infinite sequence cycling through the given sequence.</returns>
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> source, Int32 count = Int32.MaxValue)
        {
            if (count < 0)
            {
                throw new ArgumentException($"Value of {nameof(count)} argument = {count}, {count} < 0");
            }

            ICollection<T> values = source.ToArray();

            for (Int32 i = 0; i <= count; i++)
            {
                foreach (T item in values)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Concatenates all elements of a sequence using the specified separator between each element.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence that contains the objects to concatenate.</param>
        /// <param name="separator">The string to use as a separator.</param>
        /// <param name="start">Start with</param>
        /// <param name="end">End with</param>
        /// <returns>A string holding the concatenated values.</returns>
        public static String Join<T>(this IEnumerable<T> source, String separator = "", String start = "", String end = "")
        {
            return $"{start}{String.Join(separator, source)}{end}";
        }
        
        /// <summary>
        /// Returns a flattened OfType() sequence that contains the concatenation of all the nested sequences' elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of sequences to be flattened.</param>
        /// <returns>The concatenation of all the nested sequences' elements.</returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable source)
        {
            switch (source)
            {
                case IEnumerable<T> generic:
                {
                    foreach (T item in generic)
                    {
                        yield return item;
                    }

                    break;
                }
                case IEnumerable<IEnumerable> jagged:
                {
                    foreach (IEnumerable js in jagged)
                    {
                        foreach (T item in Flatten<T>(js))
                        {
                            yield return item;
                        }
                    }

                    break;
                }
            }
        }
        
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new MersenneTwister());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, System.Random generator)
        {
            T[] buffer = source.ToArray();
            
            for (Int32 i = 0; i < buffer.Length; i++)
            {
                Int32 j = generator.Next(i, buffer.Length);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
        
        /// <summary>
        /// Determines whether the specified sequence's element count is equal to or greater than <paramref name="count"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> whose elements to count.</param>
        /// <param name="count">The minimum number of elements the specified sequence is expected to contain.</param>
        /// <returns>
        ///   <c>true</c> if the element count of <paramref name="source"/> is equal to or greater than <paramref name="count"/>; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean HasAtLeast<T>(this IEnumerable<T> source, Int32 count)
        {
            if (source is ICollection sc && sc.Count < count)
            {
                return false;
            }

            if (count <= 0)
            {
                return true;
            }
            
            Int32 matches = 0;
            
            using IEnumerator<T> enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (matches++ >= count)
                {
                    return true;
                }
            }

            return matches >= count;
        }
        
        /// <summary>
        /// Determines whether the specified sequence contains exactly <paramref name="count"/> or more elements satisfying a condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> whose elements to count.</param>
        /// <param name="count">The minimum number of elements satisfying the specified condition the specified sequence is expected to contain.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///   <c>true</c> if the element count of satisfying elements is equal to or greater than <paramref name="count"/>; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean HasAtLeast<T>(this IEnumerable<T> source, Int32 count, Func<T, Boolean> predicate)
        {
            if (source is ICollection sc && sc.Count < count)
            {
                return false;
            }

            if (count <= 0)
            {
                return true;
            }
            
            Int32 matches = 0;
            
            if (source.Where(predicate).Any(_ => ++matches >= count))
            {
                return true;
            }

            return matches >= count;
        }
        
        /// <summary>
        /// Determines whether the specified sequence's element count is at most <paramref name="count"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> whose elements to count.</param>
        /// <param name="count">The maximum number of elements the specified sequence is expected to contain.</param>
        /// <returns>
        ///   <c>true</c> if the element count of <paramref name="source"/> is equal to or lower than <paramref name="count"/>; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean HasAtMost<T>(this IEnumerable<T> source, Int32 count)
        {
            return !HasAtLeast(source, count);
        }
        
        /// <summary>
        /// Determines whether the specified sequence contains at most <paramref name="count"/> elements satisfying a condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> whose elements to count.</param>
        /// <param name="count">The maximum number of elements satisfying the specified condition the specified sequence is expected to contain.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///   <c>true</c> if the element count of satisfying elements is equal to or less than <paramref name="count"/>; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean HasAtMost<T>(this IEnumerable<T> source, Int32 count, Func<T, Boolean> predicate)
        {
            return !HasAtLeast(source, count, predicate);
        }
        
        /// <summary>
        /// Determines whether the specified sequence contains exactly the specified number of elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to count.</param>
        /// <param name="count">The number of elements the specified sequence is expected to contain.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="source"/> contains exactly <paramref name="count"/> elements; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean HasExactly<T>(this IEnumerable<T> source, Int32 count)
        {
            if (source is ICollection sourceCollection)
            {
                return sourceCollection.Count == count;
            }

            Int32 matches = 0;
            
            using IEnumerator<T> enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (matches++ > count)
                {
                    return false;
                }
            }

            return matches == count;
        }

        /// <summary>
        /// Determines whether the specified sequence contains exactly the specified number of elements satisfying the specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to count satisfying elements.</param>
        /// <param name="count">The number of matching elements the specified sequence is expected to contain.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="source"/> contains exactly <paramref name="count"/> elements satisfying the condition; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean HasExactly<T>(this IEnumerable<T> source, Int32 count, Func<T, Boolean> predicate)
        {
            if (source is ICollection sc && sc.Count < count)
            {
                return false;
            }

            Int32 matches = 0;

            if (source.Where(predicate).Any(_ => ++matches > count))
            {
                return false;
            }

            return matches == count;
        }

        /// <summary>
        /// Returns all elements of <paramref name="source"/> without <paramref name="without"/> using the specified equality comparer to compare values.
        /// Does not throw an exception if <paramref name="source"/> does not contain <paramref name="without"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to remove the specified elements from.</param>
        /// <param name="without">The elements to remove.</param>
        /// <returns>
        /// All elements of <paramref name="source"/> except <paramref name="without"/>.
        /// </returns>
        public static IEnumerable<T> Without<T>(IEnumerable<T> source,
            params T[] without)
        {
            return Without(source, without, null);
        }
        
        /// <summary>
        /// Returns all elements of <paramref name="source"/> without <paramref name="without"/> using the specified equality comparer to compare values.
        /// Does not throw an exception if <paramref name="source"/> does not contain <paramref name="without"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to remove the specified elements from.</param>
        /// <param name="without">The elements to remove.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <returns>
        /// All elements of <paramref name="source"/> except <paramref name="without"/>.
        /// </returns>
        public static IEnumerable<T> Without<T>(IEnumerable<T> source, IEqualityComparer<T> comparer, params T[] without)
        {
            return Without(source, without, comparer);
        }
        
        /// <summary>
        /// Returns all elements of <paramref name="source"/> without <paramref name="without"/> using the specified equality comparer to compare values.
        /// Does not throw an exception if <paramref name="source"/> does not contain <paramref name="without"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to remove the specified elements from.</param>
        /// <param name="without">The elements to remove.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <returns>
        /// All elements of <paramref name="source"/> except <paramref name="without"/>.
        /// </returns>
        public static IEnumerable<T> Without<T>(IEnumerable<T> source,
            IEnumerable<T> without, IEqualityComparer<T> comparer = null)
        {
            HashSet<T> remove = new HashSet<T>(without, comparer);

            return source.Where(item => !remove.Contains(item));
        }
        
        /// <summary>
        /// Determines whether the given sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check for emptiness.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="source"/> is empty; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsEmpty<T>(this IEnumerable<T> source)
        {
            return source?.Any() == false;
        }

        /// <summary>
        /// Determines whether the given sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check for emptiness.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="source"/> is empty; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsEmpty<T>(this IEnumerable<T> source, Func<T, Boolean> predicate)
        {
            return source?.Any(predicate) == false;
        }

        /// <summary>
        /// Determines whether the given sequence is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check for null or emptiness.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="source"/> is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source?.Any() != true;
        }
        
        /// <summary>
        /// Determines whether the given sequence is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check for null or emptiness.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="source"/> is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> source, Func<T, Boolean> predicate)
        {
            return source?.Any(predicate) != true;
        }
    }
}