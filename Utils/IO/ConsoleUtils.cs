// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using Common_Library.Types.Map;

namespace Common_Library.Utils
{
    public interface IConsoleMsg
    {
        void ToConsole();
        void ToConsole(Boolean newLine);
        void ToConsole(Boolean newLine, ConsoleColor color);
    }
    
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

        public static void Write(Object obj)
        {
            ToConsole(obj, false);
        }
        
        public static void Write(Object obj, ConsoleColor color)
        {
            ToConsole(obj, color, false);
        }
        
        public static void Write(Object obj, ConsoleColor color, ConsoleColor bColor)
        {
            ToConsole(obj, color, bColor, false);
        }
        
        public static void WriteLine(Object obj)
        {
            ToConsole(obj);
        }
        
        public static void WriteLine(Object obj, ConsoleColor color)
        {
            ToConsole(obj, color);
        }
        
        public static void WriteLine(Object obj, ConsoleColor color, ConsoleColor bColor)
        {
            ToConsole(obj, color, bColor);
        }
        
        public static void ToConsole(this Object obj, Boolean newLine = true)
        {
            if (newLine)
            {
                Console.WriteLine(obj);
            }
            else
            {
                Console.Write(obj);
            }
        }
        
        public static void ToConsole(this Object obj, ConsoleColor color, Boolean newLine = true)
        {
            ConsoleColor consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            
            ToConsole(obj, newLine);
                
            Console.ForegroundColor = consoleColor;
        }
        
        public static void ToConsole(this Object obj, ConsoleColor color, ConsoleColor bColor, Boolean newLine = true)
        {
            ConsoleColor backgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = bColor;
            
            ToConsole(obj, color, newLine);
                
            Console.BackgroundColor = backgroundColor;
        }
    }
}