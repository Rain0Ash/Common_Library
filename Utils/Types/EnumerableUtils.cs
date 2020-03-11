// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;

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

        public static T AsDefault<T>(this T value, T alternate)
        {
            return value.Equals(default(T)) ? alternate : value;
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
    }
}