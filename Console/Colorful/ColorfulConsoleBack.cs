using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Wraps around the System.Console class, adding enhanced styling functionality.
    /// </summary>
    public static partial class Console
    {
        private static ColorStore colorStore;
        private static ColorManagerFactory colorManagerFactory;
        private static ColorManager colorManager;
        private static readonly Dictionary<String, COLORREF> DefaultColorMap;
        private static readonly Boolean IsInCompatibilityMode;
        private static readonly Boolean IsWindows;

        // Limitation of the Windows console window.
        private const Int32 MAX_COLOR_CHANGES = 16;

        // Note that if you set ConsoleColor.Black to a different color, then the background of the
        // console will change as a side-effect!  The index of Black (in the ConsoleColor definition) is 0,
        // so avoid that index.
        private const Int32 INITIAL_COLOR_CHANGE_COUNT_VALUE = 1;

        private static readonly String WritelineTrailer = "\r\n";
        private static readonly String WriteTrailer = "";

#if !NET40
        private static TaskQueue Queue { get; } = new TaskQueue();
#endif

        private static void MapToScreen(IEnumerable<KeyValuePair<String, Color>> styleMap, String trailer)
        {
#if !NET40
            Queue.EnqueueAsync(() => Task.Factory.StartNew(() =>
            {
#endif
                ConsoleColor oldSystemColor = System.Console.ForegroundColor;
                Int32 writeCount = 1;
                foreach (KeyValuePair<String, Color> textChunk in styleMap)
                {
                    System.Console.ForegroundColor = colorManager.GetConsoleColor(textChunk.Value);

                    if (writeCount == styleMap.Count())
                    {
                        System.Console.Write(textChunk.Key + trailer);
                    }
                    else
                    {
                        System.Console.Write(textChunk.Key);
                    }

                    writeCount++;
                }

                System.Console.ForegroundColor = oldSystemColor;
#if !NET40
            })).ConfigureAwait(false);
#endif
        }

        private static void MapToScreen(StyledString styledString, String trailer)
        {
            ConsoleColor oldSystemColor = System.Console.ForegroundColor;
            Int32 rowLength = styledString.CharacterGeometry.GetLength(0);
            Int32 columnLength = styledString.CharacterGeometry.GetLength(1);
            for (Int32 row = 0; row < rowLength; row++)
            {
                for (Int32 column = 0; column < columnLength; column++)
                {
                    System.Console.ForegroundColor = colorManager.GetConsoleColor(styledString.ColorGeometry[row, column]);

                    if (row == rowLength - 1 && column == columnLength - 1)
                    {
                        System.Console.Write(styledString.CharacterGeometry[row, column] + trailer);
                    }
                    else if (column == columnLength - 1)
                    {
                        System.Console.Write(styledString.CharacterGeometry[row, column] + "\r\n");
                    }
                    else
                    {
                        System.Console.Write(styledString.CharacterGeometry[row, column]);
                    }
                }
            }

            System.Console.ForegroundColor = oldSystemColor;
        }

        private static void WriteInColor<T>(Action<T> action, T target, Color color)
        {
            ConsoleColor oldSystemColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = colorManager.GetConsoleColor(color);
            action.Invoke(target);
            System.Console.ForegroundColor = oldSystemColor;
        }

        private static void WriteChunkInColor(Action<String> action, Char[] buffer, Int32 index, Int32 count, Color color)
        {
            String chunk = buffer.AsString().Substring(index, count);

            WriteInColor(action, chunk, color);
        }

        private static void WriteInColorAlternating<T>(Action<T> action, T target, ColorAlternator alternator)
        {
            Color color = alternator.GetNextColor(target.AsString());

            ConsoleColor oldSystemColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = colorManager.GetConsoleColor(color);
            action.Invoke(target);
            System.Console.ForegroundColor = oldSystemColor;
        }

        private static void WriteChunkInColorAlternating(Action<String> action, Char[] buffer, Int32 index, Int32 count, ColorAlternator alternator)
        {
            String chunk = buffer.AsString().Substring(index, count);

            WriteInColorAlternating(action, chunk, alternator);
        }

        private static void WriteInColorStyled<T>(String trailer, T target, StyleSheet styleSheet)
        {
            TextAnnotator annotator = new TextAnnotator(styleSheet);
            List<KeyValuePair<String, Color>> annotationMap = annotator.GetAnnotationMap(target.AsString());

            MapToScreen(annotationMap, trailer);
        }

        private static void WriteAsciiInColorStyled(String trailer, StyledString target, StyleSheet styleSheet)
        {
            TextAnnotator annotator = new TextAnnotator(styleSheet);
            List<KeyValuePair<String, Color>> annotationMap = annotator.GetAnnotationMap(target.AbstractValue); // Should eventually be target.AsStyledString() everywhere...?

            PopulateColorGeometry(annotationMap, target);

            MapToScreen(target, trailer);
        }

        private static void PopulateColorGeometry(IEnumerable<KeyValuePair<String, Color>> annotationMap, StyledString target)
        {
            Int32 abstractCharCount = 0;
            foreach (KeyValuePair<String, Color> fragment in annotationMap)
            {
                for (Int32 i = 0; i < fragment.Key.Length; i++)
                {
                    // This will run O(n^2) times...but with DP, could be O(n).
                    // Just need to keep a third array that keeps track of each abstract char's width, so you never iterate past that.
                    // This third array would be one-dimensional.

                    Int32 rowLength = target.CharacterIndexGeometry.GetLength(0);
                    Int32 columnLength = target.CharacterIndexGeometry.GetLength(1);
                    for (Int32 row = 0; row < rowLength; row++)
                    {
                        for (Int32 column = 0; column < columnLength; column++)
                        {
                            if (target.CharacterIndexGeometry[row, column] == abstractCharCount)
                            {
                                target.ColorGeometry[row, column] = fragment.Value;
                            }
                        }
                    }

                    abstractCharCount++;
                }
            }
        }

        private static void WriteChunkInColorStyled(String trailer, Char[] buffer, Int32 index, Int32 count, StyleSheet styleSheet)
        {
            String chunk = buffer.AsString().Substring(index, count);

            WriteInColorStyled(trailer, chunk, styleSheet);
        }

        private static void WriteInColor<T, TU>(Action<T, TU> action, T target0, TU target1, Color color)
        {
            ConsoleColor oldSystemColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = colorManager.GetConsoleColor(color);
            action.Invoke(target0, target1);
            System.Console.ForegroundColor = oldSystemColor;
        }

        private static void WriteInColorAlternating<T, TU>(Action<T, TU> action, T target0, TU target1, ColorAlternator alternator)
        {
            String formatted = String.Format(target0.ToString(), target1.Normalize());
            Color color = alternator.GetNextColor(formatted);

            ConsoleColor oldSystemColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = colorManager.GetConsoleColor(color);
            action.Invoke(target0, target1);
            System.Console.ForegroundColor = oldSystemColor;
        }

        private static void WriteInColorStyled<T, TU>(String trailer, T target0, TU target1, StyleSheet styleSheet)
        {
            TextAnnotator annotator = new TextAnnotator(styleSheet);

            String formatted = String.Format(target0.ToString(), target1.Normalize());
            List<KeyValuePair<String, Color>> annotationMap = annotator.GetAnnotationMap(formatted);

            MapToScreen(annotationMap, trailer);
        }

        private static void WriteInColorFormatted<T, TU>(String trailer, T target0, TU target1, Color styledColor, Color defaultColor)
        {
            TextFormatter formatter = new TextFormatter(defaultColor);
            List<KeyValuePair<String, Color>> formatMap = formatter.GetFormatMap(target0.ToString(), target1.Normalize(), new[] { styledColor });

            MapToScreen(formatMap, trailer);
        }

        private static void WriteInColorFormatted<T>(String trailer, T target0, Formatter target1, Color defaultColor)
        {
            TextFormatter formatter = new TextFormatter(defaultColor);
            List<KeyValuePair<String, Color>> formatMap = formatter.GetFormatMap(target0.ToString(), new[] { target1.Target }, new[] { target1.Color });

            MapToScreen(formatMap, trailer);
        }

        private static void WriteInColor<T, TU>(Action<T, TU, TU> action, T target0, TU target1, TU target2, Color color)
        {
            ConsoleColor oldSystemColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = colorManager.GetConsoleColor(color);
            action.Invoke(target0, target1, target2);
            System.Console.ForegroundColor = oldSystemColor;
        }

        private static void WriteInColorAlternating<T, TU>(Action<T, TU, TU> action, T target0, TU target1, TU target2, ColorAlternator alternator)
        {
            String formatted = String.Format(target0.ToString(), target1, target2); // NOT FORMATTING
            Color color = alternator.GetNextColor(formatted);

            ConsoleColor oldSystemColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = colorManager.GetConsoleColor(color);
            action.Invoke(target0, target1, target2);
            System.Console.ForegroundColor = oldSystemColor;
        }

        private static void WriteInColorStyled<T, TU>(String trailer, T target0, TU target1, TU target2, StyleSheet styleSheet)
        {
            TextAnnotator annotator = new TextAnnotator(styleSheet);

            String formatted = String.Format(target0.ToString(), target1, target2);
            List<KeyValuePair<String, Color>> annotationMap = annotator.GetAnnotationMap(formatted);

            MapToScreen(annotationMap, trailer);
        }

        private static void WriteInColorFormatted<T, TU>(String trailer, T target0, TU target1, TU target2, Color styledColor, Color defaultColor)
        {
            TextFormatter formatter = new TextFormatter(defaultColor);
            List<KeyValuePair<String, Color>> formatMap = formatter.GetFormatMap(target0.ToString(), new[] { target1, target2 }.Normalize(), new[] { styledColor });

            MapToScreen(formatMap, trailer);
        }

        private static void WriteInColorFormatted<T>(String trailer, T target0, Formatter target1, Formatter target2, Color defaultColor)
        {
            TextFormatter formatter = new TextFormatter(defaultColor);
            List<KeyValuePair<String, Color>> formatMap = formatter.GetFormatMap(target0.ToString(), new[] { target1.Target, target2.Target }, new[] { target1.Color, target2.Color });

            MapToScreen(formatMap, trailer);
        }

        private static void WriteInColor<T, TU>(Action<T, TU, TU, TU> action, T target0, TU target1, TU target2, TU target3, Color color)
        {
            ConsoleColor oldSystemColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = colorManager.GetConsoleColor(color);
            action.Invoke(target0, target1, target2, target3);
            System.Console.ForegroundColor = oldSystemColor;
        }

        private static void WriteInColorAlternating<T, TU>(Action<T, TU, TU, TU> action, T target0, TU target1, TU target2, TU target3, ColorAlternator alternator)
        {
            String formatted = String.Format(target0.ToString(), target1, target2, target3);
            Color color = alternator.GetNextColor(formatted);

            ConsoleColor oldSystemColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = colorManager.GetConsoleColor(color);
            action.Invoke(target0, target1, target2, target3);
            System.Console.ForegroundColor = oldSystemColor;
        }

        private static void WriteInColorStyled<T, TU>(String trailer, T target0, TU target1, TU target2, TU target3, StyleSheet styleSheet)
        {
            TextAnnotator annotator = new TextAnnotator(styleSheet);

            String formatted = String.Format(target0.ToString(), target1, target2, target3);
            List<KeyValuePair<String, Color>> annotationMap = annotator.GetAnnotationMap(formatted);

            MapToScreen(annotationMap, trailer);
        }

        private static void WriteInColorFormatted<T, TU>(String trailer, T target0, TU target1, TU target2, TU target3, Color styledColor, Color defaultColor)
        {
            TextFormatter formatter = new TextFormatter(defaultColor);
            List<KeyValuePair<String, Color>> formatMap = formatter.GetFormatMap(target0.ToString(), new[] { target1, target2, target3 }.Normalize(), new[] { styledColor });

            MapToScreen(formatMap, trailer);
        }

        private static void WriteInColorFormatted<T>(String trailer, T target0, Formatter target1, Formatter target2, Formatter target3, Color defaultColor)
        {
            TextFormatter styler = new TextFormatter(defaultColor);
            List<KeyValuePair<String, Color>> formatMap = styler.GetFormatMap(target0.ToString(), new[] { target1.Target, target2.Target, target3.Target }, new[] { target1.Color, target2.Color, target3.Color });

            MapToScreen(formatMap, trailer);
        }

        private static void WriteInColorFormatted<T>(String trailer, T target0, Formatter[] targets, Color defaultColor)
        {
            TextFormatter styler = new TextFormatter(defaultColor);
            List<KeyValuePair<String, Color>> formatMap = styler.GetFormatMap(target0.ToString(), targets.Select(formatter => formatter.Target).ToArray(), targets.Select(formatter => formatter.Color).ToArray());

            MapToScreen(formatMap, trailer);
        }

        private static void DoWithGradient<T>(Action<Object, Color> writeAction, IEnumerable<T> input, Color startColor, Color endColor, Int32 maxColorsInGradient)
        {
            GradientGenerator generator = new GradientGenerator();
            List<StyleClass<T>> gradient = generator.GenerateGradient(input, startColor, endColor, maxColorsInGradient);

            foreach (StyleClass<T> item in gradient)
            {
                writeAction(item.Target, item.Color);
            }
        }

        private static Figlet GetFiglet(FigletFont font = null)
        {
            return font == null ? new Figlet() : new Figlet(font);
        }

        private static readonly Color BlackEquivalent = Color.FromArgb(0, 0, 0);
        private static readonly Color BlueEquivalent = Color.FromArgb(0, 0, 255);
        private static readonly Color CyanEquivalent = Color.FromArgb(0, 255, 255);
        private static readonly Color DarkBlueEquivalent = Color.FromArgb(0, 0, 128);
        private static readonly Color DarkCyanEquivalent = Color.FromArgb(0, 128, 128);
        private static readonly Color DarkGrayEquivalent = Color.FromArgb(128, 128, 128);
        private static readonly Color DarkGreenEquivalent = Color.FromArgb(0, 128, 0);
        private static readonly Color DarkMagentaEquivalent = Color.FromArgb(128, 0, 128);
        private static readonly Color DarkRedEquivalent = Color.FromArgb(128, 0, 0);
        private static readonly Color DarkYellowEquivalent = Color.FromArgb(128, 128, 0);
        private static readonly Color GrayEquivalent = Color.FromArgb(192, 192, 192);
        private static readonly Color GreenEquivalent = Color.FromArgb(0, 255, 0);
        private static readonly Color MagentaEquivalent = Color.FromArgb(255, 0, 255);
        private static readonly Color RedEquivalent = Color.FromArgb(255, 0, 0);
        private static readonly Color WhiteEquivalent = Color.FromArgb(255, 255, 255);
        private static readonly Color YellowEquivalent = Color.FromArgb(255, 255, 0);

        private static ColorStore GetColorStore()
        {
            ConcurrentDictionary<Color, ConsoleColor> colorMap = new ConcurrentDictionary<Color, ConsoleColor>();
            ConcurrentDictionary<ConsoleColor, Color> consoleColorMap = new ConcurrentDictionary<ConsoleColor, Color>();

            consoleColorMap.TryAdd(ConsoleColor.Black, BlackEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.Blue, BlueEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.Cyan, CyanEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.DarkBlue, DarkBlueEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.DarkCyan, DarkCyanEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.DarkGray, DarkGrayEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.DarkGreen, DarkGreenEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.DarkMagenta, DarkMagentaEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.DarkRed, DarkRedEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.DarkYellow, DarkYellowEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.Gray, GrayEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.Green, GreenEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.Magenta, MagentaEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.Red, RedEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.White, WhiteEquivalent);
            consoleColorMap.TryAdd(ConsoleColor.Yellow, YellowEquivalent);

            return new ColorStore(colorMap, consoleColorMap);
        }
    }
}
