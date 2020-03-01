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
            : base(PathUtils.IsValidFilePath(configPath) ? configPath : new FileInfo($"{ConfigName}.xml").FullName, isReadOnly)
        {
            throw new NotImplementedException();
        }
        
        protected override String this[String key, params String[] sections]
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
    }
}