// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes methods and properties used for alternating over a set of colors according to
    /// frequency of use.
    /// </summary>
    public sealed class FrequencyBasedColorAlternator : ColorAlternator, IPrototypable<FrequencyBasedColorAlternator>
    {
        private readonly Int32 _alternationFrequency;
        private Int32 _writeCount;

        /// <summary>
        /// Exposes methods and properties used for alternating over a set of colors according to
        /// frequency of use.
        /// </summary>
        /// <param name="alternationFrequency">The number of times GetNextColor must be called in order for
        /// the color to alternate.</param>
        /// <param name="colors">The set of colors over which to alternate.</param>
        public FrequencyBasedColorAlternator(Int32 alternationFrequency, params Color[] colors)
            : base(colors)
        {
            _alternationFrequency = alternationFrequency;
        }

        public new FrequencyBasedColorAlternator Prototype()
        {
            return new FrequencyBasedColorAlternator(_alternationFrequency, Colors.DeepCopy().ToArray());
        }

        protected override ColorAlternator PrototypeCore()
        {
            return Prototype();
        }

        /// <summary>
        /// Alternates colors based on the number of times GetNextColor has been called.
        /// </summary>
        /// <param name="input">The string to be styled.</param>
        /// <returns>The current color of the ColorAlternator.</returns>
        public override Color GetNextColor(String input)
        {
            if (Colors.Length == 0)
            {
                throw new InvalidOperationException("No colors have been supplied over which to alternate!");
            }

            Color nextColor = Colors[NextColorIndex];
            TryIncrementColorIndex();

            return nextColor;
        }

        protected override void TryIncrementColorIndex()
        {
            if (_writeCount >= Colors.Length * _alternationFrequency - 1)
            {
                NextColorIndex = 0;
                _writeCount = 0;
            }
            else
            {
                _writeCount++;
                NextColorIndex = (Int32) Math.Floor(_writeCount / (Double) _alternationFrequency);
            }
        }
    }
}