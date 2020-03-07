// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Common_Library.Colorful
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FigletFont
    {
        public static FigletFont Default => Parse(DefaultFonts.SmallSlant);

        public Int32 BaseLine { get; private set; }

        public Int32 CodeTagCount { get; private set; }

        public Int32 CommentLines { get; private set; }

        public Int32 FullLayout { get; private set; }

        public String HardBlank { get; private set; }

        public Int32 Height { get; private set; }

        public Int32 Kerning { get; private set; }

        public String[] Lines { get; private set; }

        public Int32 MaxLength { get; private set; }

        public Int32 OldLayout { get; private set; }

        public Int32 PrintDirection { get; private set; }

        public String Signature { get; private set; }

        public static FigletFont Load(Byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return Load(stream);
            }
        }

        public static FigletFont Load(Stream stream)
        {
            if (stream == null) { throw new ArgumentNullException(nameof(stream)); }

            List<String> fontLines = new List<String>();
            using (StreamReader streamReader = new StreamReader(stream))
            {
                while (!streamReader.EndOfStream)
                {
                    fontLines.Add(streamReader.ReadLine());
                }
            }

            return Parse(fontLines);
        }

        public static FigletFont Load(String filePath)
        {
            if (filePath == null) { throw new ArgumentNullException(nameof(filePath)); }

            return Parse(File.ReadLines(filePath));
        }

        public static FigletFont Parse(String fontContent)
        {
            if (fontContent == null) { throw new ArgumentNullException(nameof(fontContent)); }

            return Parse(fontContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));
        }

        public static FigletFont Parse(IEnumerable<String> fontLines)
        {
            if (fontLines == null) { throw new ArgumentNullException(nameof(fontLines)); }

            FigletFont font = new FigletFont()
            {
                Lines = fontLines.ToArray()
            };
            String configString = font.Lines.First();
            String[] configArray = configString.Split(' ');
            font.Signature = configArray.First().Remove(configArray.First().Length - 1);
            if (font.Signature == "flf2a")
            {
                font.HardBlank = configArray.First().Last().ToString();
                font.Height = ParseIntValue(configArray, 1);
                font.BaseLine = ParseIntValue(configArray, 2);
                font.MaxLength = ParseIntValue(configArray, 3);
                font.OldLayout = ParseIntValue(configArray, 4);
                font.CommentLines = ParseIntValue(configArray, 5);
                font.PrintDirection = ParseIntValue(configArray, 6);
                font.FullLayout = ParseIntValue(configArray, 7);
                font.CodeTagCount = ParseIntValue(configArray, 8);
            }

            return font;
        }

        private static Int32 ParseIntValue(String[] values, Int32 index)
        {
            Int32 integer = 0;

            if (values.Length > index)
            {
                Int32.TryParse(values[index], out integer);
            }

            return integer;
        }
    }
}