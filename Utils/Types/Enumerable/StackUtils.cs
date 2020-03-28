// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Collections.Generic;

namespace Common_Library.Utils
{
    public static class StackUtils
    {
        /// <summary>
        /// Pushes a range of items into a stack
        /// </summary>
        /// <typeparam name="T">The type of items in the stack</typeparam>
        /// <param name="stack">The stack to push into</param>
        /// <param name="items">The items to push</param>
        public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                stack.Push(item);
            }
        }
    }
}