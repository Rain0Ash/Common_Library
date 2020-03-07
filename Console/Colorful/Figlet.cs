// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Common_Library.Colorful
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Drawing;

    public class Figlet
    {
        private readonly FigletFont _font;

        public Figlet()
        {
            _font = FigletFont.Default;
        }

        public Figlet(FigletFont font)
        {
            _font = font ?? throw new ArgumentNullException(nameof(font));
        }

        public StyledString ToAscii(String value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            if (Encoding.UTF8.GetByteCount(value) != value.Length) { throw new ArgumentException("String contains non-ascii characters"); }

            StringBuilder stringBuilder = new StringBuilder();

            Int32 stringWidth = GetStringWidth(_font, value);
            Char[,] characterGeometry = new Char[_font.Height + 1, stringWidth];
            Int32[,] characterIndexGeometry = new Int32[_font.Height + 1, stringWidth];
            Color[,] colorGeometry = new Color[_font.Height + 1, stringWidth];

            for (Int32 line = 1; line <= _font.Height; line++)
            {
                Int32 runningWidthTotal = 0;

                for (Int32 c = 0; c < value.Length; c++) 
                {
                    Char character = value[c];
                    String fragment = GetCharacter(_font, character, line);

                    stringBuilder.Append(fragment);
                    CalculateCharacterGeometries(fragment, c, runningWidthTotal, line, characterGeometry, characterIndexGeometry);

                    runningWidthTotal += fragment.Length;
                }

                stringBuilder.AppendLine();
            }

            StyledString styledString =
                new StyledString(value, stringBuilder.ToString())
                {
                    CharacterGeometry = characterGeometry,
                    CharacterIndexGeometry = characterIndexGeometry,
                    ColorGeometry = colorGeometry
                };

            return styledString;
        }

        private static void CalculateCharacterGeometries(String fragment, Int32 characterIndex, Int32 runningWidthTotal, Int32 line, Char[,] charGeometry, Int32[,] indexGeometry)
        {
            for (Int32 i = runningWidthTotal; i < runningWidthTotal + fragment.Length; i++)
            {
                charGeometry[line, i] = fragment[i - runningWidthTotal];
                indexGeometry[line, i] = characterIndex;
            }
        }

        private static Int32 GetStringWidth(FigletFont font, String value)
        {
            List<Int32> charWidths = new List<Int32>();
            foreach (Char character in value)
            {
                Int32 charWidth = 0;
                for (Int32 line = 1; line <= font.Height; line++)
                {
                    String figletCharacter = GetCharacter(font, character, line);

                    charWidth = figletCharacter.Length > charWidth ? figletCharacter.Length : charWidth;
                }

                charWidths.Add(charWidth);
            }

            return charWidths.Sum();
        }

        private static String GetCharacter(FigletFont font, Char character, Int32 line)
        {
            Int32 start = font.CommentLines + (Convert.ToInt32(character) - 32) * font.Height;
            String result = font.Lines[start + line];
            Char lineEnding = result[result.Length - 1];
            result = Regex.Replace(result, @"\" + lineEnding + "{1,2}$", String.Empty);

            if (font.Kerning > 0)
            {
                result += new String(' ', font.Kerning);
            }

            return result.Replace(font.HardBlank, " ");
        }
    }
}