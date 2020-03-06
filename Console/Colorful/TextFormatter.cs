using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes methods and properties used in batch styling of text.  In contrast to the TextAnnotator
    /// class, this class is meant to be used in the styling of *formatted* strings, i.e. strings that
    /// follow the "{0}, {1}...{n}" pattern.
    /// </summary>
    public sealed class TextFormatter
    {
        // NOTE: I still feel that there's too much overlap between this class and the TextAnnotator class.

        private readonly Color _defaultColor;
        private readonly TextPattern _textPattern;
        private readonly String _defaultFormatToken = "{[0-9][^}]*}";

        /// <summary>
        /// Exposes methods and properties used in batch styling of text.  In contrast to the TextAnnotator
        /// class, this class is meant to be used in the styling of *formatted* strings, i.e. strings that
        /// follow the "{0}, {1}...{n}" pattern.
        /// </summary>
        /// <param name="defaultColor">The color to be associated with unstyled text.</param>
        public TextFormatter(Color defaultColor)
        {
            _defaultColor = defaultColor;
            _textPattern = new TextPattern(_defaultFormatToken);
        }

        /// <summary>
        /// Exposes methods and properties used in batch styling of text.  In contrast to the TextAnnotator
        /// class, this class is meant to be used in the styling of *formatted* strings, i.e. strings that
        /// follow the "{0}, {1}...{n}" pattern.
        /// </summary>
        /// <param name="defaultColor">The color to be associated with unstyled text.</param>
        /// <param name="formatToken">A regular expression representing the format token.  By default,
        /// the TextFormatter will use a regular expression that matches the "{0}, {1}...{n}" pattern.</param>
        public TextFormatter(Color defaultColor, String formatToken)
        {
            _defaultColor = defaultColor;
            _textPattern = new TextPattern(formatToken);
        }

        /// <summary>
        /// Partitions the input text into styled and unstyled pieces.
        /// </summary>
        /// <param name="input">The text to be styled.</param>
        /// <param name="args">A collection of objects that will replace the format tokens in the input string.</param>
        /// <param name="colors"></param>
        /// <returns>Returns a map relating pieces of text to their corresponding styles.</returns>
        public List<KeyValuePair<String, Color>> GetFormatMap(String input, Object[] args, Color[] colors)
        {
            List<KeyValuePair<String, Color>> formatMap = new List<KeyValuePair<String, Color>>();
            List<MatchLocation> locations = _textPattern.GetMatchLocations(input).ToList();
            List<String> indices = _textPattern.GetMatches(input).ToList();

            TryExtendColors(ref args, ref colors);

            Int32 chocolateEnd = 0;
            for (Int32 i = 0; i < locations.Count(); i++)
			{
                Int32 styledIndex = Int32.Parse(indices[i].TrimStart('{').TrimEnd('}'));

                Int32 vanillaStart = 0;
                if (i > 0)
                {
                    vanillaStart = locations[i - 1].End;
                }

                Int32 vanillaEnd = locations[i].Beginning;
                chocolateEnd = locations[i].End;

                String vanilla = input.Substring(vanillaStart, vanillaEnd - vanillaStart);
                String chocolate = args[styledIndex].ToString();

                formatMap.Add(new KeyValuePair<String, Color>(vanilla, _defaultColor));
                formatMap.Add(new KeyValuePair<String, Color>(chocolate, colors[styledIndex]));
			}

            if (chocolateEnd < input.Length)
            {
                String vanilla = input.Substring(chocolateEnd, input.Length - chocolateEnd);
                formatMap.Add(new KeyValuePair<String, Color>(vanilla, _defaultColor));
            }

            return formatMap;
        }

        private void TryExtendColors(ref Object[] args, ref Color[] colors)
        {
            if (colors.Length < args.Length)
            {
                Color styledColor = colors[0];
                colors = new Color[args.Length];

                for (Int32 i = 0; i < args.Length; i++)
                {
                    colors[i] = styledColor;
                }
            }
        }
    }
}
