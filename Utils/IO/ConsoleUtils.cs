// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using Common_Library.Types.Map;

namespace Common_Library.Utils
{
    public static class ConsoleUtils
    {
        private static readonly Map<Color, ConsoleColor> ColorMap = new Map<Color, ConsoleColor>
        {
            {Color.Black, ConsoleColor.Black},
            {Color.Blue, ConsoleColor.Blue},
            {Color.Cyan, ConsoleColor.Cyan},
            {Color.Gray, ConsoleColor.Gray},
            {Color.Green, ConsoleColor.Green},
            {Color.Magenta, ConsoleColor.Magenta},
            {Color.Red, ConsoleColor.Red},
            {Color.White, ConsoleColor.White},
            {Color.Yellow, ConsoleColor.Yellow},
            {Color.DarkBlue, ConsoleColor.DarkBlue},
            {Color.DarkCyan, ConsoleColor.DarkCyan},
            {Color.DarkGray, ConsoleColor.DarkGray},
            {Color.DarkGreen, ConsoleColor.DarkGreen},
            {Color.DarkMagenta, ConsoleColor.DarkMagenta},
            {Color.DarkRed, ConsoleColor.DarkRed},
            {Color.Orange, ConsoleColor.DarkYellow}
        };

        public static ConsoleColor GetColor(Color color)
        {
            return ColorMap.TryGetValue(color, out ConsoleColor consoleColor) ? consoleColor : ConsoleColor.White;
        }

        public static Color GetColor(ConsoleColor consoleColor)
        {
            return ColorMap.TryGetValue(consoleColor, out Color color) ? color : Color.White;
        }

        public static void Write(Object obj, IFormatProvider info = null)
        {
            ToConsole(obj, false, info);
        }

        public static void Write(Object obj, ConsoleColor color, IFormatProvider info = null)
        {
            ToConsole(obj, color, false, info);
        }

        public static void Write(Object obj, ConsoleColor color, ConsoleColor bColor, IFormatProvider info = null)
        {
            ToConsole(obj, color, bColor, false, info);
        }

        public static void WriteLine(IFormatProvider info = null)
        {
            WriteLine(String.Empty, info);
        }

        public static void WriteLine(Object obj, IFormatProvider info = null)
        {
            ToConsole(obj, info);
        }

        public static void WriteLine(Object obj, ConsoleColor color, IFormatProvider info = null)
        {
            ToConsole(obj, color, true, info);
        }

        public static void WriteLine(Object obj, Color color, IFormatProvider info = null)
        {
            ToConsole(obj, color, true, info);
        }

        public static void WriteLine(Object obj, ConsoleColor color, ConsoleColor bColor, IFormatProvider info = null)
        {
            ToConsole(obj, color, bColor, true, info);
        }

        public static void WriteLine(Object obj, Color color, Color bColor, IFormatProvider info = null)
        {
            ToConsole(obj, color, bColor, true, info);
        }

        public static void ToConsole(this Object obj, IFormatProvider info)
        {
            ToConsole(obj, true, info);
        }

        public static void ToConsole(this Object obj, Boolean newLine = true, IFormatProvider info = null)
        {
            if (newLine)
            {
                Console.WriteLine(obj.Convert(info));
            }
            else
            {
                Console.Write(obj.Convert(info));
            }
        }

        public static void ToConsole(this Object obj, ConsoleColor color, Boolean newLine = true, IFormatProvider info = null)
        {
            ConsoleColor consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            ToConsole(obj, newLine, info);

            Console.ForegroundColor = consoleColor;
        }

        public static void ToConsole(this Object obj, Color color, Boolean newLine = true, IFormatProvider info = null)
        {
            Color consoleColor = Colorful.Console.ForegroundColor;
            Colorful.Console.ForegroundColor = color;

            if (newLine)
            {
                Colorful.Console.WriteLine(obj.Convert(info));
            }
            else
            {
                Colorful.Console.Write(obj.Convert(info));
            }

            Colorful.Console.ResetColor();
            Colorful.Console.ForegroundColor = consoleColor;
        }

        public static void ToConsole(this Object obj, ConsoleColor color, ConsoleColor bColor, Boolean newLine = true,
            IFormatProvider info = null)
        {
            ConsoleColor backgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = bColor;

            ToConsole(obj, color, newLine, info);

            Console.BackgroundColor = backgroundColor;
        }

        public static void ToConsole(this Object obj, Color color, Color bColor, Boolean newLine = true, IFormatProvider info = null)
        {
            Color backgroundColor = Colorful.Console.BackgroundColor;
            Colorful.Console.BackgroundColor = bColor;

            ToConsole(obj, color, newLine);

            Colorful.Console.BackgroundColor = backgroundColor;
        }
    }
}