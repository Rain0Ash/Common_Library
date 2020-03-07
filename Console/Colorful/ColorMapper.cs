// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes methods used for mapping System.Drawing.Colors to System.ConsoleColors.
    /// Based on code that was originally written by Alex Shvedov, and that was then modified by MercuryP.
    /// </summary>
    public sealed class ColorMapper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Coord
        {
            internal readonly Int16 X;
            internal readonly Int16 Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SmallRect
        {
            internal readonly Int16 Left;
            internal readonly Int16 Top;
            internal Int16 Right;
            internal Int16 Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ConsoleScreenBufferInfoEx
        {
            internal Int32 cbSize;
            internal readonly Coord dwSize;
            internal readonly Coord dwCursorPosition;
            internal readonly UInt16 wAttributes;
            internal SmallRect srWindow;
            internal readonly Coord dwMaximumWindowSize;
            internal readonly UInt16 wPopupAttributes;
            internal readonly Boolean bFullscreenSupported;
            internal COLORREF black;
            internal COLORREF darkBlue;
            internal COLORREF darkGreen;
            internal COLORREF darkCyan;
            internal COLORREF darkRed;
            internal COLORREF darkMagenta;
            internal COLORREF darkYellow;
            internal COLORREF gray;
            internal COLORREF darkGray;
            internal COLORREF blue;
            internal COLORREF green;
            internal COLORREF cyan;
            internal COLORREF red;
            internal COLORREF magenta;
            internal COLORREF yellow;
            internal COLORREF white;
        }

        private const Int32 STD_OUTPUT_HANDLE = -11;                               // per WinBase.h
        private static readonly IntPtr InvalidHandleValue = new IntPtr(-1);    // per WinBase.h

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(Int32 nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Boolean GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref ConsoleScreenBufferInfoEx csbe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Boolean SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref ConsoleScreenBufferInfoEx csbe);

        /// <summary>
        /// Maps a System.Drawing.Color to a System.ConsoleColor.
        /// </summary>
        /// <param name="oldColor">The color to be replaced.</param>
        /// <param name="newColor">The color to be mapped.</param>
        public void MapColor(ConsoleColor oldColor, Color newColor)
        {
            // NOTE: The default console colors used are gray (foreground) and black (background).
            MapColor(oldColor, newColor.R, newColor.G, newColor.B);
        }

        /// <summary>
        /// Gets a collection of all 16 colors in the console buffer.
        /// </summary>
        /// <returns>Returns all 16 COLORREFs in the console buffer as a dictionary keyed by the COLORREF's alias
        /// in the buffer's ColorTable.</returns>
        public Dictionary<String, COLORREF> GetBufferColors()
        {
            Dictionary<String, COLORREF> colors = new Dictionary<String, COLORREF>();
            IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);    // 7
            ConsoleScreenBufferInfoEx csbe = GetBufferInfo(hConsoleOutput);

            colors.Add("black", csbe.black);
            colors.Add("darkBlue", csbe.darkBlue);
            colors.Add("darkGreen", csbe.darkGreen);
            colors.Add("darkCyan", csbe.darkCyan);
            colors.Add("darkRed", csbe.darkRed);
            colors.Add("darkMagenta", csbe.darkMagenta);
            colors.Add("darkYellow", csbe.darkYellow);
            colors.Add("gray", csbe.gray);
            colors.Add("darkGray", csbe.darkGray);
            colors.Add("blue", csbe.blue);
            colors.Add("green", csbe.green);
            colors.Add("cyan", csbe.cyan);
            colors.Add("red", csbe.red);
            colors.Add("magenta", csbe.magenta);
            colors.Add("yellow", csbe.yellow);
            colors.Add("white", csbe.white);

            return colors;
        }

        /// <summary>
        /// Sets all 16 colors in the console buffer using colors supplied in a dictionary.
        /// </summary>
        /// <param name="colors">A dictionary containing COLORREFs keyed by the COLORREF's alias in the buffer's 
        /// ColorTable.</param>
        public void SetBatchBufferColors(Dictionary<String, COLORREF> colors)
        {
            IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE); // 7
            ConsoleScreenBufferInfoEx csbe = GetBufferInfo(hConsoleOutput);

            csbe.black = colors["black"];
            csbe.darkBlue = colors["darkBlue"];
            csbe.darkGreen = colors["darkGreen"];
            csbe.darkCyan = colors["darkCyan"];
            csbe.darkRed = colors["darkRed"];
            csbe.darkMagenta = colors["darkMagenta"];
            csbe.darkYellow = colors["darkYellow"];
            csbe.gray = colors["gray"];
            csbe.darkGray = colors["darkGray"];
            csbe.blue = colors["blue"];
            csbe.green = colors["green"];
            csbe.cyan = colors["cyan"];
            csbe.red = colors["red"];
            csbe.magenta = colors["magenta"];
            csbe.yellow = colors["yellow"];
            csbe.white = colors["white"];

            SetBufferInfo(hConsoleOutput, csbe);
        }

        private ConsoleScreenBufferInfoEx GetBufferInfo(IntPtr hConsoleOutput)
        {
            ConsoleScreenBufferInfoEx csbe = new ConsoleScreenBufferInfoEx();
            csbe.cbSize = Marshal.SizeOf(csbe); // 96 = 0x60

            if (hConsoleOutput == InvalidHandleValue)
            {
                throw CreateException(Marshal.GetLastWin32Error());
            }

            Boolean brc = GetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);

            if (!brc)
            {
                throw CreateException(Marshal.GetLastWin32Error());
            }

            return csbe;
        }

        private void MapColor(ConsoleColor color, UInt32 r, UInt32 g, UInt32 b)
        {
            IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE); // 7
            ConsoleScreenBufferInfoEx csbe = GetBufferInfo(hConsoleOutput);

            switch (color)
            {
                case ConsoleColor.Black:
                    csbe.black = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkBlue:
                    csbe.darkBlue = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGreen:
                    csbe.darkGreen = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkCyan:
                    csbe.darkCyan = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkRed:
                    csbe.darkRed = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkMagenta:
                    csbe.darkMagenta = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkYellow:
                    csbe.darkYellow = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Gray:
                    csbe.gray = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGray:
                    csbe.darkGray = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Blue:
                    csbe.blue = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Green:
                    csbe.green = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Cyan:
                    csbe.cyan = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Red:
                    csbe.red = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Magenta:
                    csbe.magenta = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Yellow:
                    csbe.yellow = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.White:
                    csbe.white = new COLORREF(r, g, b);
                    break;
            }

            SetBufferInfo(hConsoleOutput, csbe);
        }

        private void SetBufferInfo(IntPtr hConsoleOutput, ConsoleScreenBufferInfoEx csbe)
        {
            csbe.srWindow.Bottom++;
            csbe.srWindow.Right++;

            Boolean brc = SetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                throw CreateException(Marshal.GetLastWin32Error());
            }
        }

        private Exception CreateException(Int32 errorCode)
        {
            const Int32 errorInvalidHandle = 6;
            if (errorCode == errorInvalidHandle) // Raised if the console is being run via another application, for example.
            {
                return new ConsoleAccessException();
            }

            return new ColorMappingException(errorCode);
        }
    }
}
