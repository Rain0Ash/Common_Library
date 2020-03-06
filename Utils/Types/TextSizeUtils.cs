// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Common_Library.Utils
{
    public static class TextSizeUtils
    {
        #if NETFRAMEWORK
        public static IEnumerable<IEnumerable<String>> SplitStringToMaxWidthSubstrings(String text, Font font, Int32 maxWidth)
        {
            
            if (text == null || font == null)
            {
                throw new ArgumentNullException();
            }

            if (maxWidth < font.Size)
            {
                throw new ArgumentException("maxWidth argument can't be less than font size");
            }
            
            List<List<String>> stringsList = new List<List<String>>();
            
            String[] newLineStrings = text.Split(new []{LocalizationBase.NewLine, @"\n"}, StringSplitOptions.None);

            foreach (String newLineString in newLineStrings)
            {
                String[] strings = newLineString.Split(' ');

                if (strings.Length <= 0)
                {
                    continue;
                }

                List<String> strList = new List<String>();
                    
                if (strings.Length > 1)
                {
                    Int32 skip = 0;
                    for (Int32 i = 1; i <= strings.Length; i++)
                    {
                        Int32 take = i - skip;
                        String[] builder = strings.Skip(skip).Take(take).ToArray();

                        
                        //TODO:
                        #if NETFRAMEWORK
                        if (TextRenderer.MeasureText(String.Join(" ", builder), font).Width > maxWidth)
                        {
                            continue;
                        }

                        if (strings.Skip(skip).Count() > take &&
                            TextRenderer.MeasureText(String.Join(" ", strings.Skip(skip).Take(take + 1)), font).Width <= maxWidth)
                        {
                            continue;
                        }
                        #endif
                        

                        skip = i;
                        strList.Add(String.Join(" ", builder));
                    }
                }
                else
                {
                    strList.Add(strings[0]);
                }
                    
                stringsList.Add(strList);
            }

            return stringsList;
        }

        public static String StringToMaxWidthSubstrings(String text, Font font, Int32 maxWidth)
        {
            return String.Join("\n", SplitStringToMaxWidthSubstrings(text, font, maxWidth).Select(list => String.Join(" ", list)));
        }
        #endif
    }
}