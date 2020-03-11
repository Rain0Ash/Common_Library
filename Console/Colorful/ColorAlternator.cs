// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes methods and properties used for alternating over a set of colors.
    /// </summary>
    public abstract class ColorAlternator : IPrototypable<ColorAlternator>
    {
        /// <summary>
        /// The set of colors over which to alternate.
        /// </summary>
        public Color[] Colors { get; set; }

        protected Int32 NextColorIndex = 0;

        /// <summary>
        /// Exposes methods and properties used for alternating over a set of colors.
        /// </summary>
        public ColorAlternator()
        {
            Colors = new Color[] { };
        }

        /// <summary>
        /// Exposes methods and properties used for alternating over a set of colors.
        /// </summary>
        public ColorAlternator(params Color[] colors)
        {
            Colors = colors;
        }

        public ColorAlternator Prototype()
        {
            return PrototypeCore();
        }

        protected abstract ColorAlternator PrototypeCore();

        /// <summary>
        /// Alternates colors based on the state of the ColorAlternator instance.
        /// </summary>
        /// <param name="input">The string to be styled.</param>
        /// <returns>The current color of the ColorAlternator.</returns>
        public abstract Color GetNextColor(String input);

        protected abstract void TryIncrementColorIndex();
    }
}