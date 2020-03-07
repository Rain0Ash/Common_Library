// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Represents a collection of Pattern objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PatternCollection<T> : IPrototypable<PatternCollection<T>>
    {
        protected List<Pattern<T>> Patterns = new List<Pattern<T>>();

        /// <summary>
        /// Represents a collection of Pattern objects.
        /// </summary>
   
        public PatternCollection<T> Prototype()
        {
            return PrototypeCore();
        }

        protected abstract PatternCollection<T> PrototypeCore();

        /// <summary>
        /// Attempts to match any of the PatternCollection's member Patterns against a string input.
        /// </summary>
        /// <param name="input">The input against which Patterns will potentially be matched.</param>
        /// <returns>Returns 'true' if any of the PatternCollection's member Patterns matches against
        /// the input string.</returns>
        public abstract Boolean MatchFound(String input);
    }
}
