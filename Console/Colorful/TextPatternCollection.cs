// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Represents a collection of TextPattern objects.
    /// </summary>
    public sealed class TextPatternCollection : PatternCollection<String>
    {
        /// <summary>
        /// Represents a collection of TextPattern objects.
        /// </summary>
        /// <param name="patterns">Patterns to be added to the collection.</param>
        public TextPatternCollection(String[] patterns)
        {
            foreach (String pattern in patterns)
            {
                Patterns.Add(new TextPattern(pattern));
            }
        }

        public new TextPatternCollection Prototype()
        {
            return new TextPatternCollection(Patterns.Select(pattern => pattern.Value).ToArray());
        }

        protected override PatternCollection<String> PrototypeCore()
        {
            return Prototype();
        }

        /// <summary>
        /// Attempts to match any of the TextPatternCollection's member TextPatterns against a string input.
        /// </summary>
        /// <param name="input">The input against which Patterns will potentially be matched.</param>
        /// <returns>Returns 'true' if any of the TextPatternCollection's member TextPatterns matches against
        /// the input string.</returns>
        public override Boolean MatchFound(String input)
        {
            return Patterns.Any(pattern => pattern.GetMatchLocations(input).Any());
        }
    }
}
