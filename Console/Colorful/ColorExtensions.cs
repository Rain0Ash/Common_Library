// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// <see cref="Color"/> extension methods.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts the specified <see cref="Color"/> to it's nearest <see cref="ConsoleColor"/> equivalent.
        /// </summary>
        /// <remarks>Code taken from Glenn Slayden at https://stackoverflow.com/questions/1988833/converting-color-to-consolecolor</remarks>
        public static ConsoleColor ToNearestConsoleColor(this Color color)
        {
            ConsoleColor closestConsoleColor = 0;
            Double delta = Double.MaxValue;

            foreach (ConsoleColor consoleColor in Enum.GetValues(typeof(ConsoleColor)))
            {
                String consoleColorName = Enum.GetName(typeof(ConsoleColor), consoleColor);
                consoleColorName = String.Equals(consoleColorName, nameof(ConsoleColor.DarkYellow), StringComparison.Ordinal) ? nameof(Color.Orange) : consoleColorName;
                Color rgbColor = Color.FromName(consoleColorName);
                Double sum = Math.Pow(rgbColor.R - color.R, 2.0) + Math.Pow(rgbColor.G - color.G, 2.0) + Math.Pow(rgbColor.B - color.B, 2.0);

                Double epsilon = 0.001;
                if (sum < epsilon)
                {
                    return consoleColor;
                }

                if (!(sum < delta))
                {
                    continue;
                }

                delta = sum;
                closestConsoleColor = consoleColor;
            }

            return closestConsoleColor;
        }
    }
}
