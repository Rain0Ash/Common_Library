// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;

namespace Common_Library.Colorful
{
    /// <summary>
    ///     Wraps around the System.Console class, adding enhanced styling functionality.
    /// </summary>
    public static partial class Console
    {
        public static Color BackgroundColor
        {
            get => colorManager.GetColor(System.Console.BackgroundColor);
            set => System.Console.BackgroundColor = colorManager.GetConsoleColor(value);
        }

        public static Int32 BufferHeight
        {
            get => System.Console.BufferHeight;
            set => System.Console.BufferHeight = value;
        }

        public static Int32 BufferWidth
        {
            get => System.Console.BufferWidth;
            set => System.Console.BufferWidth = value;
        }

        public static Boolean CapsLock => System.Console.CapsLock;

        public static Int32 CursorLeft
        {
            get => System.Console.CursorLeft;
            set => System.Console.CursorLeft = value;
        }

        public static Int32 CursorSize
        {
            get => System.Console.CursorSize;
            set => System.Console.CursorSize = value;
        }

        public static Int32 CursorTop
        {
            get => System.Console.CursorTop;
            set => System.Console.CursorTop = value;
        }

        public static Boolean CursorVisible
        {
            get => System.Console.CursorVisible;
            set => System.Console.CursorVisible = value;
        }

        public static TextWriter Error => System.Console.Error;

        public static Color ForegroundColor
        {
            get => colorManager.GetColor(System.Console.ForegroundColor);
            set => System.Console.ForegroundColor = colorManager.GetConsoleColor(value);
        }

        public static TextReader In => System.Console.In;

        public static Encoding InputEncoding
        {
            get => System.Console.InputEncoding;
            set => System.Console.InputEncoding = value;
        }

#if !NET40
        public static Boolean IsErrorRedirected => System.Console.IsErrorRedirected;

        public static Boolean IsInputRedirected => System.Console.IsInputRedirected;

        public static Boolean IsOutputRedirected => System.Console.IsOutputRedirected;
#endif

        public static Boolean KeyAvailable => System.Console.KeyAvailable;

        public static Int32 LargestWindowHeight => System.Console.LargestWindowHeight;

        public static Int32 LargestWindowWidth => System.Console.LargestWindowWidth;

        public static Boolean NumberLock => System.Console.NumberLock;

        public static TextWriter Out => System.Console.Out;

        public static Encoding OutputEncoding
        {
            get => System.Console.OutputEncoding;
            set => System.Console.OutputEncoding = value;
        }

        public static String Title
        {
            get => System.Console.Title;
            set => System.Console.Title = value;
        }

        public static Boolean TreatControlCAsInput
        {
            get => System.Console.TreatControlCAsInput;
            set => System.Console.TreatControlCAsInput = value;
        }

        public static Int32 WindowHeight
        {
            get => System.Console.WindowHeight;
            set => System.Console.WindowHeight = value;
        }

        public static Int32 WindowLeft
        {
            get => System.Console.WindowLeft;
            set => System.Console.WindowLeft = value;
        }

        public static Int32 WindowTop
        {
            get => System.Console.WindowTop;
            set => System.Console.WindowTop = value;
        }

        public static Int32 WindowWidth
        {
            get => System.Console.WindowWidth;
            set => System.Console.WindowWidth = value;
        }

        public static event ConsoleCancelEventHandler CancelKeyPress = delegate { };

        static Console()
        {
            IsInCompatibilityMode = false;
            IsWindows = ColorManager.IsWindows();
            try
            {
                if (IsWindows) DefaultColorMap = new ColorMapper().GetBufferColors();
            }
            catch (ConsoleAccessException)
            {
                IsInCompatibilityMode = true;
            }

            ReplaceAllColorsWithDefaults();
            System.Console.CancelKeyPress += Console_CancelKeyPress;
        }

        public static void Write(Boolean value)
        {
            System.Console.Write(value);
        }

        public static void Write(Boolean value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(Boolean value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(Boolean value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(Char value)
        {
            System.Console.Write(value);
        }

        public static void Write(Char value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(Char value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(Char value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(Char[] value)
        {
            System.Console.Write(value);
        }

        public static void Write(Char[] value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(Char[] value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(Char[] value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(Decimal value)
        {
            System.Console.Write(value);
        }

        public static void Write(Decimal value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(Decimal value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(Decimal value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(Double value)
        {
            System.Console.Write(value);
        }

        public static void Write(Double value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(Double value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(Double value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(Single value)
        {
            System.Console.Write(value);
        }

        public static void Write(Single value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(Single value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(Single value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(Int32 value)
        {
            System.Console.Write(value);
        }

        public static void Write(Int32 value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(Int32 value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(Int32 value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(Int64 value)
        {
            System.Console.Write(value);
        }

        public static void Write(Int64 value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(Int64 value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(Int64 value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(Object value)
        {
            System.Console.Write(value);
        }

        public static void Write(Object value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(Object value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(Object value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(String value)
        {
            System.Console.Write(value);
        }

        public static void Write(String value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(String value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(String value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(UInt32 value)
        {
            System.Console.Write(value);
        }

        public static void Write(UInt32 value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(UInt32 value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(UInt32 value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(UInt64 value)
        {
            System.Console.Write(value);
        }

        public static void Write(UInt64 value, Color color)
        {
            WriteInColor(System.Console.Write, value, color);
        }

        public static void WriteAlternating(UInt64 value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, value, alternator);
        }

        public static void WriteStyled(UInt64 value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, value, styleSheet);
        }

        public static void Write(String format, Object arg0)
        {
            System.Console.Write(format, arg0);
        }

        public static void Write(String format, Object arg0, Color color)
        {
            WriteInColor(System.Console.Write, format, arg0, color);
        }

        public static void WriteAlternating(String format, Object arg0, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, format, arg0, alternator);
        }

        public static void WriteStyled(String format, Object arg0, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, format, arg0, styleSheet);
        }

        public static void WriteFormatted(String format, Object arg0, Color styledColor, Color defaultColor)
        {
            WriteInColorFormatted(WriteTrailer, format, arg0, styledColor, defaultColor);
        }

        public static void WriteFormatted(String format, Formatter arg0, Color defaultColor)
        {
            WriteInColorFormatted(WriteTrailer, format, arg0, defaultColor);
        }

        public static void Write(String format, params Object[] args)
        {
            System.Console.Write(format, args);
        }

        public static void Write(String format, Color color, params Object[] args)
        {
            WriteInColor(System.Console.Write, format, args, color);
        }

        public static void WriteAlternating(String format, ColorAlternator alternator, params Object[] args)
        {
            WriteInColorAlternating(System.Console.Write, format, args, alternator);
        }

        public static void WriteStyled(StyleSheet styleSheet, String format, params Object[] args)
        {
            WriteInColorStyled(WriteTrailer, format, args, styleSheet);
        }

        public static void WriteFormatted(String format, Color styledColor, Color defaultColor, params Object[] args)
        {
            WriteInColorFormatted(WriteTrailer, format, args, styledColor, defaultColor);
        }

        public static void WriteFormatted(String format, Color defaultColor, params Formatter[] args)
        {
            WriteInColorFormatted(WriteTrailer, format, args, defaultColor);
        }

        public static void Write(Char[] buffer, Int32 index, Int32 count)
        {
            System.Console.Write(buffer, index, count);
        }

        public static void Write(Char[] buffer, Int32 index, Int32 count, Color color)
        {
            WriteChunkInColor(System.Console.Write, buffer, index, count, color);
        }

        public static void WriteAlternating(Char[] buffer, Int32 index, Int32 count, ColorAlternator alternator)
        {
            WriteChunkInColorAlternating(System.Console.Write, buffer, index, count, alternator);
        }

        public static void WriteStyled(Char[] buffer, Int32 index, Int32 count, StyleSheet styleSheet)
        {
            WriteChunkInColorStyled(WriteTrailer, buffer, index, count, styleSheet);
        }

        public static void Write(String format, Object arg0, Object arg1)
        {
            System.Console.Write(format, arg0, arg1);
        }

        public static void Write(String format, Object arg0, Object arg1, Color color)
        {
            WriteInColor(System.Console.Write, format, arg0, arg1, color);
        }

        public static void WriteAlternating(String format, Object arg0, Object arg1, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, format, arg0, arg1, alternator);
        }

        public static void WriteStyled(String format, Object arg0, Object arg1, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, format, arg0, arg1, styleSheet);
        }

        public static void WriteFormatted(String format, Object arg0, Object arg1, Color styledColor,
            Color defaultColor)
        {
            WriteInColorFormatted(WriteTrailer, format, arg0, arg1, styledColor, defaultColor);
        }

        public static void WriteFormatted(String format, Formatter arg0, Formatter arg1, Color defaultColor)
        {
            WriteInColorFormatted(WriteTrailer, format, arg0, arg1, defaultColor);
        }

        public static void Write(String format, Object arg0, Object arg1, Object arg2)
        {
            System.Console.Write(format, arg0, arg1, arg2);
        }

        public static void Write(String format, Object arg0, Object arg1, Object arg2, Color color)
        {
            WriteInColor(System.Console.Write, format, arg0, arg1, arg2, color);
        }

        public static void WriteAlternating(String format, Object arg0, Object arg1, Object arg2,
            ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, format, arg0, arg1, arg2, alternator);
        }

        public static void WriteStyled(String format, Object arg0, Object arg1, Object arg2, StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, format, arg0, arg1, arg2, styleSheet);
        }

        public static void WriteFormatted(String format, Object arg0, Object arg1, Object arg2, Color styledColor,
            Color defaultColor)
        {
            WriteInColorFormatted(WriteTrailer, format, arg0, arg1, arg2, styledColor, defaultColor);
        }

        public static void WriteFormatted(String format, Formatter arg0, Formatter arg1, Formatter arg2,
            Color defaultColor)
        {
            WriteInColorFormatted(WriteTrailer, format, arg0, arg1, arg2, defaultColor);
        }

        public static void Write(String format, Object arg0, Object arg1, Object arg2, Object arg3)
        {
            System.Console.Write(format, arg0, arg1, arg2, arg3);
        }

        public static void Write(String format, Object arg0, Object arg1, Object arg2, Object arg3, Color color)
        {
            // NOTE: The Intellisense for this overload of System.Console.Write is misleading, as the C# compiler
            //       actually resolves this overload to System.Console.Write(string format, object[] args)!

            WriteInColor(System.Console.Write, format, new[] {arg0, arg1, arg2, arg3}, color);
        }

        public static void WriteAlternating(String format, Object arg0, Object arg1, Object arg2, Object arg3,
            ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, format, new[] {arg0, arg1, arg2, arg3}, alternator);
        }

        public static void WriteStyled(String format, Object arg0, Object arg1, Object arg2, Object arg3,
            StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, format, new[] {arg0, arg1, arg2, arg3}, styleSheet);
        }

        public static void WriteFormatted(String format, Object arg0, Object arg1, Object arg2, Object arg3,
            Color styledColor, Color defaultColor)
        {
            WriteInColorFormatted(WriteTrailer, format, new[] {arg0, arg1, arg2, arg3}, styledColor, defaultColor);
        }

        public static void WriteFormatted(String format, Formatter arg0, Formatter arg1, Formatter arg2, Formatter arg3,
            Color defaultColor)
        {
            WriteInColorFormatted(WriteTrailer, format, new[] {arg0, arg1, arg2, arg3}, defaultColor);
        }

        public static void WriteLine()
        {
            System.Console.WriteLine();
        }

        public static void WriteLineAlternating(ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.Write, WritelineTrailer, alternator);
        }

        public static void WriteLineStyled(StyleSheet styleSheet)
        {
            WriteInColorStyled(WriteTrailer, WritelineTrailer, styleSheet);
        }

        public static void WriteLine(Boolean value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(Boolean value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(Boolean value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(Boolean value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(Char value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(Char value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(Char value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(Char value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(Char[] value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(Char[] value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(Char[] value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(Char[] value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(Decimal value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(Decimal value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(Decimal value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(Decimal value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(Double value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(Double value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(Double value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(Double value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(Single value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(Single value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(Single value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(Single value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(Int32 value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(Int32 value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(Int32 value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(Int32 value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(Int64 value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(Int64 value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(Int64 value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(Int64 value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(Object value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(Object value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(Object value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(StyledString value, StyleSheet styleSheet)
        {
            WriteAsciiInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(String value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(String value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(String value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(String value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(UInt32 value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(UInt32 value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(UInt32 value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(UInt32 value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(UInt64 value)
        {
            System.Console.WriteLine(value);
        }

        public static void WriteLine(UInt64 value, Color color)
        {
            WriteInColor(System.Console.WriteLine, value, color);
        }

        public static void WriteLineAlternating(UInt64 value, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, value, alternator);
        }

        public static void WriteLineStyled(UInt64 value, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, value, styleSheet);
        }

        public static void WriteLine(String format, Object arg0)
        {
            System.Console.WriteLine(format, arg0);
        }

        public static void WriteLine(String format, Object arg0, Color color)
        {
            WriteInColor(System.Console.WriteLine, format, arg0, color);
        }

        public static void WriteLineAlternating(String format, Object arg0, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, format, arg0, alternator);
        }

        public static void WriteLineStyled(String format, Object arg0, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, format, arg0, styleSheet);
        }

        public static void WriteLineFormatted(String format, Object arg0, Color styledColor, Color defaultColor)
        {
            WriteInColorFormatted(WritelineTrailer, format, arg0, styledColor, defaultColor);
        }

        public static void WriteLineFormatted(String format, Formatter arg0, Color defaultColor)
        {
            WriteInColorFormatted(WritelineTrailer, format, arg0, defaultColor);
        }

        public static void WriteLine(String format, params Object[] args)
        {
            System.Console.WriteLine(format, args);
        }

        public static void WriteLine(String format, Color color, params Object[] args)
        {
            WriteInColor(System.Console.WriteLine, format, args, color);
        }

        public static void WriteLineAlternating(String format, ColorAlternator alternator, params Object[] args)
        {
            WriteInColorAlternating(System.Console.WriteLine, format, args, alternator);
        }

        public static void WriteLineStyled(StyleSheet styleSheet, String format, params Object[] args)
        {
            WriteInColorStyled(WritelineTrailer, format, args, styleSheet);
        }

        public static void WriteLineFormatted(String format, Color styledColor, Color defaultColor,
            params Object[] args)
        {
            WriteInColorFormatted(WritelineTrailer, format, args, styledColor, defaultColor);
        }

        public static void WriteLineFormatted(String format, Color styledColor, Color defaultColor,
            IEnumerable<Object> args)
        {
            WriteInColorFormatted(WritelineTrailer, format, args.ToArray(), styledColor, defaultColor);
        }

        public static void WriteLineFormatted(String format, Color defaultColor, params Formatter[] args)
        {
            WriteInColorFormatted(WritelineTrailer, format, args, defaultColor);
        }

        public static void WriteLine(Char[] buffer, Int32 index, Int32 count)
        {
            System.Console.WriteLine(buffer, index, count);
        }

        public static void WriteLine(Char[] buffer, Int32 index, Int32 count, Color color)
        {
            WriteChunkInColor(System.Console.WriteLine, buffer, index, count, color);
        }

        public static void WriteLineAlternating(Char[] buffer, Int32 index, Int32 count, ColorAlternator alternator)
        {
            WriteChunkInColorAlternating(System.Console.WriteLine, buffer, index, count, alternator);
        }

        public static void WriteLineStyled(Char[] buffer, Int32 index, Int32 count, StyleSheet styleSheet)
        {
            WriteChunkInColorStyled(WritelineTrailer, buffer, index, count, styleSheet);
        }

        public static void WriteLine(String format, Object arg0, Object arg1)
        {
            System.Console.WriteLine(format, arg0, arg1);
        }

        public static void WriteLine(String format, Object arg0, Object arg1, Color color)
        {
            WriteInColor(System.Console.WriteLine, format, arg0, arg1, color);
        }

        public static void WriteLineAlternating(String format, Object arg0, Object arg1, ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, format, arg0, arg1, alternator);
        }

        public static void WriteLineStyled(String format, Object arg0, Object arg1, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, format, arg0, arg1, styleSheet);
        }

        public static void WriteLineFormatted(String format, Object arg0, Object arg1, Color styledColor,
            Color defaultColor)
        {
            WriteInColorFormatted(WritelineTrailer, format, arg0, arg1, styledColor, defaultColor);
        }

        public static void WriteLineFormatted(String format, Formatter arg0, Formatter arg1, Color defaultColor)
        {
            WriteInColorFormatted(WritelineTrailer, format, arg0, arg1, defaultColor);
        }

        public static void WriteLine(String format, Object arg0, Object arg1, Object arg2)
        {
            System.Console.WriteLine(format, arg0, arg1, arg2);
        }

        public static void WriteLine(String format, Object arg0, Object arg1, Object arg2, Color color)
        {
            WriteInColor(System.Console.WriteLine, format, arg0, arg1, arg2, color);
        }

        public static void WriteLineAlternating(String format, Object arg0, Object arg1, Object arg2,
            ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, format, arg0, arg1, arg2, alternator);
        }

        public static void WriteLineStyled(String format, Object arg0, Object arg1, Object arg2, StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, format, arg0, arg1, arg2, styleSheet);
        }

        public static void WriteLineFormatted(String format, Object arg0, Object arg1, Object arg2, Color styledColor,
            Color defaultColor)
        {
            WriteInColorFormatted(WritelineTrailer, format, arg0, arg1, arg2, styledColor, defaultColor);
        }

        public static void WriteLineFormatted(String format, Formatter arg0, Formatter arg1, Formatter arg2,
            Color defaultColor)
        {
            WriteInColorFormatted(WritelineTrailer, format, arg0, arg1, arg2, defaultColor);
        }

        public static void WriteLine(String format, Object arg0, Object arg1, Object arg2, Object arg3)
        {
            System.Console.WriteLine(format, arg0, arg1, arg2, arg3);
        }

        public static void WriteLine(String format, Object arg0, Object arg1, Object arg2, Object arg3, Color color)
        {
            // NOTE: The Intellisense for this overload of System.Console.WriteLine is misleading, as the C# compiler
            //       actually resolves this overload to System.Console.WriteLine(string format, object[] args)!

            WriteInColor(System.Console.WriteLine, format, new[] {arg0, arg1, arg2, arg3}, color);
        }

        public static void WriteLineAlternating(String format, Object arg0, Object arg1, Object arg2, Object arg3,
            ColorAlternator alternator)
        {
            WriteInColorAlternating(System.Console.WriteLine, format, new[] {arg0, arg1, arg2, arg3}, alternator);
        }

        public static void WriteLineStyled(String format, Object arg0, Object arg1, Object arg2, Object arg3,
            StyleSheet styleSheet)
        {
            WriteInColorStyled(WritelineTrailer, format, new[] {arg0, arg1, arg2, arg3}, styleSheet);
        }

        public static void WriteLineFormatted(String format, Object arg0, Object arg1, Object arg2, Object arg3,
            Color styledColor, Color defaultColor)
        {
            WriteInColorFormatted(WritelineTrailer, format, new[] {arg0, arg1, arg2, arg3}, styledColor, defaultColor);
        }

        public static void WriteLineFormatted(String format, Formatter arg0, Formatter arg1, Formatter arg2,
            Formatter arg3, Color defaultColor)
        {
            WriteInColorFormatted(WritelineTrailer, format, new[] {arg0, arg1, arg2, arg3}, defaultColor);
        }

        public static void WriteAscii(String value)
        {
            WriteAscii(value, null);
        }

        public static void WriteAscii(String value, FigletFont font)
        {
            WriteLine(GetFiglet(font).ToAscii(value).ConcreteValue);
        }

        public static void WriteAscii(String value, Color color)
        {
            WriteAscii(value, null, color);
        }

        public static void WriteAscii(String value, FigletFont font, Color color)
        {
            WriteLine(GetFiglet(font).ToAscii(value).ConcreteValue, color);
        }

        public static void WriteAsciiAlternating(String value, ColorAlternator alternator)
        {
            WriteAsciiAlternating(value, null, alternator);
        }

        public static void WriteAsciiAlternating(String value, FigletFont font, ColorAlternator alternator)
        {
            foreach (String line in GetFiglet(font).ToAscii(value).ConcreteValue.Split('\n'))
                WriteLineAlternating(line, alternator);
        }

        public static void WriteAsciiStyled(String value, StyleSheet styleSheet)
        {
            WriteAsciiStyled(value, null, styleSheet);
        }

        public static void WriteAsciiStyled(String value, FigletFont font, StyleSheet styleSheet)
        {
            WriteLineStyled(GetFiglet(font).ToAscii(value), styleSheet);
        }

        public static void WriteWithGradient<T>(IEnumerable<T> input, Color startColor, Color endColor,
            Int32 maxColorsInGradient = MAX_COLOR_CHANGES)
        {
            DoWithGradient(Write, input, startColor, endColor, maxColorsInGradient);
        }

        public static void WriteLineWithGradient<T>(IEnumerable<T> input, Color startColor, Color endColor,
            Int32 maxColorsInGradient = MAX_COLOR_CHANGES)
        {
            DoWithGradient(WriteLine, input, startColor, endColor, maxColorsInGradient);
        }

        public static Int32 Read()
        {
            return System.Console.Read();
        }

        public static ConsoleKeyInfo ReadKey()
        {
            return System.Console.ReadKey();
        }

        public static ConsoleKeyInfo ReadKey(Boolean intercept)
        {
            return System.Console.ReadKey(intercept);
        }

        public static String ReadLine()
        {
            return System.Console.ReadLine();
        }

        public static void ResetColor()
        {
            System.Console.ResetColor();
        }

        public static void SetBufferSize(Int32 width, Int32 height)
        {
            System.Console.SetBufferSize(width, height);
        }

        public static void SetCursorPosition(Int32 left, Int32 top)
        {
            System.Console.SetCursorPosition(left, top);
        }

        public static void SetError(TextWriter newError)
        {
            System.Console.SetError(newError);
        }

        public static void SetIn(TextReader newIn)
        {
            System.Console.SetIn(newIn);
        }

        public static void SetOut(TextWriter newOut)
        {
            System.Console.SetOut(newOut);
        }

        public static void SetWindowPosition(Int32 left, Int32 top)
        {
            System.Console.SetWindowPosition(left, top);
        }

        public static void SetWindowSize(Int32 width, Int32 height)
        {
            System.Console.SetWindowSize(width, height);
        }

        public static Stream OpenStandardError()
        {
            return System.Console.OpenStandardError();
        }

#if !NETSTANDARD2_0
        public static Stream OpenStandardError(Int32 bufferSize)
        {
            return System.Console.OpenStandardError(bufferSize);
        }
#endif

        public static Stream OpenStandardInput()
        {
            return System.Console.OpenStandardInput();
        }

#if !NETSTANDARD2_0
        public static Stream OpenStandardInput(Int32 bufferSize)
        {
            return System.Console.OpenStandardInput(bufferSize);
        }
#endif

        public static Stream OpenStandardOutput()
        {
            return System.Console.OpenStandardOutput();
        }

#if !NETSTANDARD2_0
        public static Stream OpenStandardOutput(Int32 bufferSize)
        {
            return System.Console.OpenStandardOutput(bufferSize);
        }
#endif

        public static void MoveBufferArea(Int32 sourceLeft, Int32 sourceTop, Int32 sourceWidth, Int32 sourceHeight,
            Int32 targetLeft, Int32 targetTop)
        {
            System.Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
        }

        public static void MoveBufferArea(Int32 sourceLeft, Int32 sourceTop, Int32 sourceWidth, Int32 sourceHeight,
            Int32 targetLeft, Int32 targetTop, Char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            System.Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop,
                sourceChar, sourceForeColor, sourceBackColor);
        }

        public static void Clear()
        {
            System.Console.Clear();
        }

        public static void ReplaceColor(Color oldColor, Color newColor)
        {
            colorManager.ReplaceColor(oldColor, newColor);
        }

        public static void ReplaceAllColorsWithDefaults()
        {
            colorStore = GetColorStore();
            colorManagerFactory = new ColorManagerFactory();
            colorManager = colorManagerFactory.GetManager(colorStore, MAX_COLOR_CHANGES,
                INITIAL_COLOR_CHANGE_COUNT_VALUE, IsInCompatibilityMode);

            // There's no need to do this if in compatibility mode (or if not on Windows), as more than 16 colors won't be used, anyway.
            if (!colorManager.IsInCompatibilityMode && IsWindows)
                new ColorMapper().SetBatchBufferColors(DefaultColorMap);
        }

        public static void Beep(Int32 frequency, Int32 duration)
        {
            System.Console.Beep(frequency, duration);
        }

        private static void Console_CancelKeyPress(Object sender, ConsoleCancelEventArgs e)
        {
            CancelKeyPress.Invoke(sender, e);
        }
    }
}