// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes methods and properties used for alternating over a set of colors according to
    /// the occurrences of patterns.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class PatternBasedColorAlternator<T> : ColorAlternator, IPrototypable<PatternBasedColorAlternator<T>>
    {
        private readonly PatternCollection<T> _patternMatcher;
        private Boolean _isFirstRun = true;

        /// <summary>
        /// Exposes methods and properties used for alternating over a set of colors according to
        /// the occurrences of patterns.
        /// </summary>
        /// <param name="patternMatcher">The PatternMatcher instance which will dictate what will
        /// need to happen in order for the color to alternate.</param>
        /// <param name="colors">The set of colors over which to alternate.</param>
        public PatternBasedColorAlternator(PatternCollection<T> patternMatcher, params Color[] colors)
            : base(colors)
        {
            _patternMatcher = patternMatcher;
        }

        public new PatternBasedColorAlternator<T> Prototype()
        {
            return new PatternBasedColorAlternator<T>(_patternMatcher.Prototype(), Colors.DeepCopy().ToArray());
        }

        protected override ColorAlternator PrototypeCore()
        {
            return Prototype();
        }

        /// <summary>
        /// Alternates colors based on patterns matched in the input string.
        /// </summary>
        /// <param name="input">The string to be styled.</param>
        /// <returns>The current color of the ColorAlternator.</returns>
        public override Color GetNextColor(String input)
        {
            if (Colors.Length == 0)
            {
                throw new InvalidOperationException("No colors have been supplied over which to alternate!");
            }

            if (_isFirstRun)
            {
                _isFirstRun = false;
                return Colors[NextColorIndex];
            }

            if (_patternMatcher.MatchFound(input))
            {
                TryIncrementColorIndex();
            }

            Color nextColor = Colors[NextColorIndex];

            return nextColor;
        }

        protected override void TryIncrementColorIndex()
        {
            if (NextColorIndex >= Colors.Length - 1)
            {
                NextColorIndex = 0;
            }
            else
            {
                NextColorIndex++;
            }
        }
    }
}