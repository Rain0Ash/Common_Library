using System;
using System.Collections.Generic;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes methods and properties representing a pattern.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Pattern<T> : IEquatable<Pattern<T>>
    {
        /// <summary>
        /// The value, or definition, of the pattern.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Exposes methods and properties representing a pattern.
        /// </summary>
        /// <param name="pattern">The value, or definition, of the pattern.</param>
        public Pattern(T pattern)
        {
            Value = pattern;
        }

        /// <summary>
        /// Finds the locations of matches between the Pattern instance and a given object.
        /// </summary>
        /// <param name="input">The object to which the Pattern instance should be compared.</param>
        /// <returns>Returns a collection of the locations in the object under comparison that
        /// match the Pattern instance.</returns>
        public abstract IEnumerable<MatchLocation> GetMatchLocations(T input);

        /// <summary>
        /// Finds matches between the Pattern instance and a given object.
        /// </summary>
        /// <param name="input">The object to which the Pattern instance should be compared.</param>
        /// <returns>Returns a collection of tokens in the object under comparison that
        /// match the Pattern instance.</returns>
        public abstract IEnumerable<T> GetMatches(T input);

        public Boolean Equals(Pattern<T> other)
        {
            if (other == null)
            {
                return false;
            }

            return Value.Equals(other.Value);
        }

        public override Boolean Equals(Object obj)
        {
            return Equals(obj as Pattern<T>);
        }

        public override Int32 GetHashCode()
        {
            Int32 hash = 163;

            hash *= 79 + Value.GetHashCode();

            return hash;
        }
    }
}
