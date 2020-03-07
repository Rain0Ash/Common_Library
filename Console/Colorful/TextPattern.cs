// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes methods and properties representing a text pattern.
    /// </summary>
    public sealed class TextPattern : Pattern<String>
    {
        private readonly Regex _regexPattern;

        /// <summary>
        /// Exposes methods and properties representing a text pattern.
        /// </summary>
        /// <param name="pattern">A string representation of the pattern.  This can be either a 
        /// regular string *or* a regular expression (as string).</param>
        public TextPattern(String pattern)
            : base(pattern)
        {
            _regexPattern = new Regex(pattern);
        }

        /// <summary>
        /// Finds matches between the TextPattern instance and a given object.
        /// </summary>
        /// <param name="input">The string to which the TextPattern instance should be compared.</param>
        /// <returns>Returns a collection of the locations in the string under comparison that
        /// match the TextPattern instance.</returns>
        public override IEnumerable<MatchLocation> GetMatchLocations(String input)
        {
            MatchCollection matches = _regexPattern.Matches(input);

            if (matches.Count == 0)
            {
                yield break;
            }

            foreach (Match match in matches)
            {
                Int32 beginning = match.Index;
                Int32 end = beginning + match.Length;

                MatchLocation location = new MatchLocation(beginning, end);

                yield return location;
            }
        }

        /// <summary>
        /// Finds matches between the TextPattern instance and a given object.
        /// </summary>
        /// <param name="input">The string to which the TextPattern instance should be compared.</param>
        /// <returns>Returns a collection of the locations in the string under comparison that
        /// match the TextPattern instance.</returns>
        public override IEnumerable<String> GetMatches(String input)
        {
            MatchCollection matches = _regexPattern.Matches(input);

            if (matches.Count == 0)
            {
                yield break;
            }

            foreach (Match match in matches)
            {
                yield return match.Value;
            }
        }
    }
}
