// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using Common_Library.LongPath;
using Common_Library.Utils;

namespace Common_Library.Config.XML
{
    public class XMLConfig : Config
    {
        public XMLConfig(String configPath = null, Boolean isReadOnly = true)
            : base(PathUtils.IsValidFilePath(configPath) ? configPath : new FileInfo($"{DefaultName}.xml").FullName, isReadOnly)
        {
        }

        protected override String Get(String key, params String[] sections)
        {
            try
            {
                Object content = Serialization.XML.XMLToObject<Object>(FileUtils.GetFileContents(ConfigPath), key);
                return content.GetType().GetField(key)?.GetValue(content)?.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected override void Set(String key, String value, params String[] sections)
        {
            File.AppendAllText(ConfigPath, Serialization.XML.ObjectToXML(value, key));
        }
    }
}