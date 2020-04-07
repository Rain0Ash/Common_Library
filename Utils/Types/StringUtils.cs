// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common_Library.Utils
{
    public enum SplitByEnum
    {
        NewLine,
        UpperCase
    }

    public static class StringUtils
    {
        public const String FormatVariableRegexPattern = @"\{([^\{\}]+)\}";

        public static IEnumerable<String> GetFormatVariables(String str)
        {
            return Regex.Matches(str, FormatVariableRegexPattern, RegexOptions.Compiled)
                .OfType<Match>().Select(match => match.Value)
                .Select(format => Regex.Replace(format.ToLower(), @"\{|\}", String.Empty));
        }

        public static Boolean IsBracketsWellFormed(String str)
        {
            IReadOnlyDictionary<Char, Char> bracketPairs = new Dictionary<Char, Char>
            {
                {'(', ')'},
                {'{', '}'},
                {'[', ']'},
                {'<', '>'}
            };

            Stack<Char> brackets = new Stack<Char>();

            try
            {
                foreach (Char c in str)
                {
                    if (bracketPairs.Keys.Contains(c))
                    {
                        brackets.Push(c);
                    }
                    else
                    {
                        if (!bracketPairs.Values.Contains(c))
                        {
                            continue;
                        }

                        if (c == bracketPairs[brackets.First()])
                        {
                            brackets.Pop();
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            // Ensure all brackets are closed
            return !brackets.Any();
        }

        public static String ReplaceFromDictionary(this String source, IDictionary<String, Object> replaceDict)
        {
            return ReplaceFromDictionary(source, replaceDict.ToDictionary(key => key.Key, value => value.Value?.ToString()));
        }

        public static String ReplaceFromDictionary(this String source, IDictionary<String, String> replaceDict)
        {
            return Regex.Replace(source, $@"\b({String.Join("|", replaceDict.Keys)})\b",
                m => replaceDict[m.Value]?.ToString() ?? String.Empty);
        }

        public static String FormatFromDictionary(this String source, IDictionary<String, Object> valueDict)
        {
            Int32 i = 0;
            StringBuilder newFormatString = new StringBuilder(source);
            Dictionary<String, Int32> keyToInt = new Dictionary<String, Int32>();
            foreach (KeyValuePair<String, Object> tuple in valueDict)
            {
                newFormatString = newFormatString.Replace("{" + tuple.Key + "}", "{" + i + "}");
                keyToInt.Add(tuple.Key, i);
                i++;
            }

            return String.Format(newFormatString.ToString(), valueDict.OrderBy(x => keyToInt[x.Key]).Select(x => x.Value).ToArray());
        }

        public static Int32 FormatArgsExpected(String str)
        {
            const String pattern = @"(?<!\{)(?>\{\{)*\{\d(.*?)";
            MatchCollection matches = Regex.Matches(str, pattern, RegexOptions.Compiled);
            return matches.OfType<Match>().Select(m => m.Value).Distinct().Count();
        }

        public static String BeforeFormatVariables(String str)
        {
            Match match = Regex.Match(str, FormatVariableRegexPattern, RegexOptions.Compiled);
            return match.Success ? str.Substring(0, match.Index) : null;
        }

        public static String Format(this String source, params Object[] args)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            Int32 argsExpected = FormatArgsExpected(source);

            if (argsExpected <= 0)
            {
                return source;
            }

            const String nullArgs = @"null";

            args = args.Length < argsExpected
                ? args.Concat(Enumerable.Repeat(nullArgs, argsExpected - args.Length)).ToArray()
                : args.Take(argsExpected).ToArray();

            return String.Format(source, args);
        }

        public static Boolean EndsWith(this String str, IEnumerable<Char> chars)
        {
            return chars.Any(str.EndsWith);
        }
        
        public static Boolean EndsWith(this String str, IEnumerable<String> substrings)
        {
            return substrings.Any(str.EndsWith);
        }
        
        public static Boolean IsNull(this String str)
        {
            return str == null;
        }

        public static Boolean IsEmpty(this String str)
        {
            return str == String.Empty;
        }

        public static Boolean IsWhiteSpace(this String str)
        {
            return str.Length > 0 && IsEmptyOrWhiteSpace(str);
        }

        public static Boolean IsEmptyOrWhiteSpace(this String str)
        {
            return str.All(Char.IsWhiteSpace);
        }

        public static Boolean IsNullOrEmpty(this String str)
        {
            return String.IsNullOrEmpty(str);
        }

        public static Boolean IsNullOrWhiteSpace(this String str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        public static String[] SplitByUpperCase(String str)
        {
            //TODO: perfomance improvements
            return str.Split(' ')
                .Select(val =>
                    Regex.Replace(
                        Regex.Replace(val, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                        @"(\p{Ll})(\P{Ll})", "$1 $2"))
                .SelectMany(split => split.Split(' ')).ToArray();
        }

        public static String[] SplitBy(this String str, SplitByEnum splitByEnum = SplitByEnum.NewLine)
        {
            return splitByEnum switch
            {
                SplitByEnum.NewLine => str.Split('\n', StringSplitOptions.RemoveEmptyEntries),
                SplitByEnum.UpperCase => SplitByUpperCase(str),
                _ => throw new NotImplementedException()
            };
        }

        public static String Join<T>(this String str, IEnumerable<T> values)
        {
            return String.Join(str, values);
        }

        public static String Join(this String str, IEnumerable<String> values)
        {
            return String.Join(str, values);
        }

        public static String Join(this String str, params Object[] value)
        {
            return String.Join(str, value);
        }

        public static String Join(this String str, params String[] value)
        {
            return String.Join(str, value);
        }

        public static String CapitalizeFirstChar(this String input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }

            return input[0].ToString().ToUpper() + input.Substring(1);
        }
    }
}