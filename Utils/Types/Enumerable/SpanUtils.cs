// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    public static class SpanUtils
    {
        public static Span<T> Select<T>(this Memory<T> memory, Func<T, T> selector)
        {
            return memory.Span.Select(selector);
        }

        public static Span<T> Select<T>(this Span<T> span, Func<T, T> selector)
        {
            for (Int32 i = 0; i < span.Length; i++)
            {
                span[i] = selector(span[i]);
            }

            return span;
        }

        public static Span<T> ForEach<T>(this Memory<T> memory, Action<T> action)
        {
            return memory.Span.ForEach(action);
        }

        public static Span<T> ForEach<T>(this Span<T> span, Action<T> action)
        {
            foreach (T item in span)
            {
                action(item);
            }

            return span;
        }

        public static ReadOnlySpan<T> ForEach<T>(this ReadOnlyMemory<T> memory, Action<T> action)
        {
            return memory.Span.ForEach(action);
        }

        public static ReadOnlySpan<T> ForEach<T>(this ReadOnlySpan<T> span, Action<T> action)
        {
            foreach (T item in span)
            {
                action(item);
            }

            return span;
        }

        public static Boolean Any<T>(this Memory<T> memory)
        {
            return !memory.IsEmpty;
        }

        public static Boolean Any<T>(this Span<T> span)
        {
            return !span.IsEmpty;
        }

        public static Boolean Any<T>(this ReadOnlyMemory<T> memory)
        {
            return !memory.IsEmpty;
        }

        public static Boolean Any<T>(this ReadOnlySpan<T> span)
        {
            return !span.IsEmpty;
        }

        public static Boolean Any<T>(this Memory<T> memory, Func<T, Boolean> predicate)
        {
            return memory.Span.Any(predicate);
        }

        public static Boolean Any<T>(this Span<T> span, Func<T, Boolean> predicate)
        {
            foreach (T item in span)
            {
                if (predicate(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean Any<T>(this ReadOnlyMemory<T> memory, Func<T, Boolean> predicate)
        {
            return memory.Span.Any(predicate);
        }

        public static Boolean Any<T>(this ReadOnlySpan<T> span, Func<T, Boolean> predicate)
        {
            foreach (T item in span)
            {
                if (predicate(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean All<T>(this Memory<T> memory, Func<T, Boolean> predicate)
        {
            return memory.Span.All(predicate);
        }

        public static Boolean All<T>(this Span<T> span, Func<T, Boolean> predicate)
        {
            return !Any(span, predicate);
        }

        public static Boolean All<T>(this ReadOnlyMemory<T> memory, Func<T, Boolean> predicate)
        {
            return memory.Span.All(predicate);
        }

        public static Boolean All<T>(this ReadOnlySpan<T> span, Func<T, Boolean> predicate)
        {
            return !Any(span, predicate);
        }

        public static Int32 Count<T>(this Memory<T> memory)
        {
            return memory.Length;
        }

        public static Int32 Count<T>(this Span<T> span)
        {
            return span.Length;
        }

        public static Int32 Count<T>(this ReadOnlyMemory<T> memory)
        {
            return memory.Length;
        }

        public static Int32 Count<T>(this ReadOnlySpan<T> span)
        {
            return span.Length;
        }

        public static Int32 Count<T>(this Memory<T> memory, Func<T, Boolean> predicate)
        {
            return memory.Span.Count(predicate);
        }

        public static Int32 Count<T>(this Span<T> span, Func<T, Boolean> predicate)
        {
            Int32 count = 0;

            foreach (T item in span)
            {
                if (predicate(item))
                {
                    count++;
                }
            }

            return count;
        }

        public static Int32 Count<T>(this ReadOnlyMemory<T> memory, Func<T, Boolean> predicate)
        {
            return memory.Span.Count(predicate);
        }

        public static Int32 Count<T>(this ReadOnlySpan<T> span, Func<T, Boolean> predicate)
        {
            Int32 count = 0;

            foreach (T item in span)
            {
                if (predicate(item))
                {
                    count++;
                }
            }

            return count;
        }
    }
}