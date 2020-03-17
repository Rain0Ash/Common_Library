// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using Common_Library.Config.RAM;
using Common_Library.Utils.IO;
using Newtonsoft.Json;

namespace Common_Library.Config.JSON
{
    public class JSONConfig : RAMConfig
    {
        public JSONConfig(String configPath = null, Boolean isReadOnly = true)
            : base(PathUtils.IsValidFilePath(configPath) ? configPath : new LongPath.FileInfo($"{DefaultName}.json").FullName, isReadOnly)
        {
            Config = ReadConfig();
        }

        protected NestedDictionary<String, String> ReadConfig(String configPath = null)
        {
            try
            {
                String json = LongPath.File.ReadAllText(configPath ?? ConfigPath);
                return JsonConvert.DeserializeObject<NestedDictionary<String, String>>(json);
            }
            catch (Exception)
            {
                return new NestedDictionary<String, String>();
            }
        }

        protected void WriteConfig(String configPath = null)
        {
            String json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            LongPath.File.WriteAllText(configPath ?? ConfigPath, json);
        }

        protected override void Set(String key, String value, params String[] sections)
        {
            base.Set(key, value, sections);

            WriteConfig();
        }
    }
}