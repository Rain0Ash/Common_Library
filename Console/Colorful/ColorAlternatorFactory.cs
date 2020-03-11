// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

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