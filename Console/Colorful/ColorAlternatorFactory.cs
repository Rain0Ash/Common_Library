using System;
using System.Drawing;

namespace Common_Library.Colorful
{
    public sealed class ColorAlternatorFactory
    {
        public ColorAlternator GetAlternator(String[] patterns, params Color[] colors)
        {
            return new PatternBasedColorAlternator<String>(new TextPatternCollection(patterns), colors);
        }

        public ColorAlternator GetAlternator(Int32 frequency, params Color[] colors)
        {
            return new FrequencyBasedColorAlternator(frequency, colors);
        }
    }
}
