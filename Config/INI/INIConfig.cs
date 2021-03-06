// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Common_Library.Utils;
using Common_Library.Utils.IO;

namespace Common_Library.Config.INI
{
    public class INIConfig : Config
    {
        public String DefaultSection { get; set; } = "Main";

        public INIConfig(String configPath = null, Boolean isReadOnly = true)
            : base(PathUtils.IsValidFilePath(configPath) ? configPath : new FileInfo($"{DefaultName}.ini").FullName, isReadOnly)
        {
        }

        protected override String Get(String key, params String[] sections)
        {
            StringBuilder builder = new StringBuilder(255);

            GetPrivateProfileString(GenericUtils<String>.TryGetValue(sections, 0, DefaultSection), key, String.Empty, builder, 255, ConfigPath);

            String returnValue = builder.ToString();
            return String.IsNullOrEmpty(returnValue) ? null : returnValue;
        }

        protected override void Set(String key, String value, params String[] sections)
        {
            WritePrivateProfileString(GenericUtils<String>.TryGetValue(sections, 0, DefaultSection), key, value, ConfigPath);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern Int64 WritePrivateProfileString(String section, String key, String value, String filePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 GetPrivateProfileString(String section, String key, String @default, StringBuilder retVal, Int32 size,
            String filePath);
    }
}