// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Common_Library.Utils
{
    public static class RegexUtils
    {
        public static Boolean IsValidRegex([RegexPattern] String pattern)
        {
            if (String.IsNullOrEmpty(pattern))
            {
                return false;
            }

            try
            {
                _ = Regex.Match(String.Empty, pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        public static String JoinMatches([NotNull] String str, [RegexPattern] [NotNull] String pattern,
            RegexOptions options = RegexOptions.None)
        {
            return JoinMatches(str, pattern, String.Empty, options);
        }

        public static String JoinMatches(String str, [RegexPattern] [NotNull] String pattern, String separator,
            RegexOptions options = RegexOptions.None)
        {
            return JoinMatches(Regex.Matches(str, pattern, options), separator);
        }

        public static String JoinMatches(this Regex regex, [NotNull] String str)
        {
            return JoinMatches(regex, str, String.Empty);
        }

        public static String JoinMatches(this Regex regex, [NotNull] String str, [NotNull] String separator)
        {
            return JoinMatches(regex.Matches(str), separator);
        }

        public static String JoinMatches(this MatchCollection matches)
        {
            return JoinMatches(matches, String.Empty);
        }

        public static String JoinMatches(this MatchCollection matches, String separator)
        {
            return String.Join(separator ?? String.Empty, GetCaptures(matches).Skip(1));
        }

        public static IEnumerable<String> GetCaptures([NotNull] String str, [RegexPattern] String pattern,
            RegexOptions options = RegexOptions.None)
        {
            return GetCaptures(Regex.Matches(str, pattern, options));
        }

        public static IEnumerable<String> GetCaptures(this Regex regex, [NotNull] String str)
        {
            return GetCaptures(regex.Matches(str));
        }

        public static IEnumerable<String> GetCaptures(this MatchCollection matches)
        {
            return matches
                .OfType<Match>()
                .SelectMany(match => match.Groups.OfType<Group>())
                .SelectMany(group => group.Captures.OfType<Capture>())
                .Select(capture => capture.Value);
        }

        public static IEnumerable<String> IfMatchGetCaptures([NotNull] String str, [RegexPattern] String pattern,
            RegexOptions options = RegexOptions.None)
        {
            return IfMatchGetCaptures(Regex.Matches(str, pattern, options), str);
        }

        public static IEnumerable<String> IfMatchGetCaptures(this Regex regex, [NotNull] String str)
        {
            return IfMatchGetCaptures(regex.Matches(str), str);
        }

        public static IEnumerable<String> IfMatchGetCaptures(this MatchCollection matches, [NotNull] String str)
        {
            String[] captures = GetCaptures(matches).ToArray();
            return captures.FirstOrDefault()?.Equals(str) == true ? captures : null;
        }

        public static Dictionary<String, List<String>> MatchNamedCaptures(this Regex regex, String input, Boolean onlyString = true)
        {
            MatchCollection matches = regex.Matches(input);
            Dictionary<String, List<String>> namedCaptureDictionary = new Dictionary<String, List<String>>();
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                String[] groupNames = regex.GetGroupNames();

                foreach (String groupName in groupNames)
                {
                    if (onlyString && Int32.TryParse(groupName, out _) || groups[groupName].Captures.Count <= 0)
                    {
                        continue;
                    }

                    if (!namedCaptureDictionary.ContainsKey(groupName))
                    {
                        namedCaptureDictionary.Add(groupName, new List<String>());
                    }

                    namedCaptureDictionary[groupName].Add(groups[groupName].Value);
                }
            }

            return namedCaptureDictionary;
        }

        public static String CreateReplacement(MatchCollection collection, String baseReplacement)
        {
            IEnumerable<Match> matches = Regex.Matches(baseReplacement, @"(\$[a-zA-Z0-9]+)").OfType<Match>();

            Dictionary<String, String> replaceDictionary = new Dictionary<String, String>();

            foreach (Match value in collection.OfType<Match>())
            {
                throw new NotImplementedException();
            }

            return baseReplacement.ReplaceFromDictionary(replaceDictionary);
        }
    }
}