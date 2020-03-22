// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using System.Xml;
using Common_Library.LongPath;
using Common_Library.Utils.IO;

namespace Common_Library.Config.XML
{
    public class XMLConfig : Config
    {
        private readonly XmlDocument _document;

        public XMLConfig(String configPath = null, Boolean isReadOnly = true)
            : base(PathUtils.IsValidFilePath(configPath) ? configPath : new FileInfo($"{DefaultName}.xml").FullName, isReadOnly)
        {
            _document = new XmlDocument();

            try
            {
                _document.Load(ConfigPath);
            }
            catch
            {
                //ignored
            }
        }

        protected override String Get(String key, params String[] sections)
        {
            try
            {
                XmlNode node = _document.SelectSingleNode(String.Join("/", sections.Append(key)));

                return node?.InnerText;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected override void Set(String key, String value, params String[] sections)
        {
            _document.Set(String.Join("/", sections.Append(key)), value);
        }
    }
}