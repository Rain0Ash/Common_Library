// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes methods and properties used in batch styling of text.
    /// </summary>
    public sealed class TextAnnotator
    {
        // NOTE: I still feel that there's too much overlap between this class and the TextFormatter class.

        private readonly StyleSheet _styleSheet;
        private readonly Dictionary<StyleClass<TextPattern>, Styler.MatchFound> _matchFoundHandlers = new Dictionary<StyleClass<TextPattern>, Styler.MatchFound>();

        /// <summary>
        /// Exposes methods and properties used in batch styling of text.
        /// </summary>
        /// <param name="styleSheet">The StyleSheet instance that defines the way in which text should be styled.</param>
        public TextAnnotator(StyleSheet styleSheet)
        {
            _styleSheet = styleSheet;

            foreach (StyleClass<TextPattern> styleClass in styleSheet.Styles)
            {
                _matchFoundHandlers.Add(styleClass, (styleClass as Styler)?.MatchFoundHandler);
            }
        }
        
        /// <summary>
        /// Partitions the input text into styled and unstyled pieces.
        /// </summary>
        /// <param name="input">The text to be styled.</param>
        /// <returns>Returns a map relating pieces of text to their corresponding styles.</returns>
        public List<KeyValuePair<String, Color>> GetAnnotationMap(String input)
        {
            IEnumerable<KeyValuePair<StyleClass<TextPattern>, MatchLocation>> targets = GetStyleTargets(input);

            return GenerateStyleMap(targets, input);
        }

        private List<KeyValuePair<StyleClass<TextPattern>, MatchLocation>> GetStyleTargets(String input)
        {
            List<KeyValuePair<StyleClass<TextPattern>, MatchLocation>> matches = new List<KeyValuePair<StyleClass<TextPattern>, MatchLocation>>();
            List<MatchLocation> locations = new List<MatchLocation>();

            foreach (StyleClass<TextPattern> pattern in _styleSheet.Styles)
            {
                foreach (MatchLocation location in pattern.Target.GetMatchLocations(input))
                {
                    if (locations.Contains(location))
                    {
                        Int32 index = locations.IndexOf(location);

                        matches.RemoveAt(index);
                        locations.RemoveAt(index);
                    }

                    matches.Add(new KeyValuePair<StyleClass<TextPattern>, MatchLocation>(pattern, location));
                    locations.Add(location);
                }
            }

            matches = matches.OrderBy(match => match.Value).ToList();
            return matches;
        }

        private List<KeyValuePair<String, Color>> GenerateStyleMap(IEnumerable<KeyValuePair<StyleClass<TextPattern>, MatchLocation>> targets, String input)
        {
            List<KeyValuePair<String, Color>> styleMap = new List<KeyValuePair<String, Color>>();

            MatchLocation previousLocation = new MatchLocation(0, 0);
            Int32 chocolateEnd = 0;
            String vanilla;
            
            foreach ((StyleClass<TextPattern> key, MatchLocation value) in targets)
            {
                MatchLocation currentLocation = value;

                if (previousLocation.End > currentLocation.Beginning)
                {
                    currentLocation = new MatchLocation(previousLocation.End, Math.Max(previousLocation.End, currentLocation.End));
                }

                Int32 vanillaStart = previousLocation.End;
                Int32 vanillaEnd = currentLocation.Beginning;
                Int32 chocolateStart = vanillaEnd;
                chocolateEnd = currentLocation.End;

                vanilla = input.Substring(vanillaStart, vanillaEnd - vanillaStart);

                String chocolate = _matchFoundHandlers[key].Invoke(input, value, input.Substring(chocolateStart, chocolateEnd - chocolateStart));

                if (vanilla != "")
                {
                    styleMap.Add(new KeyValuePair<String, Color>(vanilla, _styleSheet.UnstyledColor));
                }
                if (chocolate != "")
                {
                    styleMap.Add(new KeyValuePair<String, Color>(chocolate, key.Color));
                }

                previousLocation = currentLocation.Prototype();
            }

            if (chocolateEnd >= input.Length)
            {
                return styleMap;
            }

            vanilla = input.Substring(chocolateEnd, input.Length - chocolateEnd);
            styleMap.Add(new KeyValuePair<String, Color>(vanilla, _styleSheet.UnstyledColor));

            return styleMap;
        }
    }
}
