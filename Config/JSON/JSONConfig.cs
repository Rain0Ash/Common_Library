// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Common_Library.Utils;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Common_Library.Config
{
    public class JSONConfig : Config
    {
        [NotNull]
        protected NestedDictionary<String, String> Config { get; set; }
        
        public JSONConfig(String configPath = null, Boolean isReadOnly = true)
            : base(PathUtils.IsValidFilePath(configPath) ? configPath : new LongPath.FileInfo($"{ConfigName}.json").FullName, isReadOnly)
        {
        }

        protected override void Initialize()
        {
            Config = ReadConfig();
            MessageBox.Show(Config["API"].Value);
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
                throw;
                return new NestedDictionary<String, String>();
            }
        }
        
        protected void WriteConfig(String configPath = null)
        {
            String json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            LongPath.File.WriteAllText(configPath ?? ConfigPath, json);
        }

        protected override String this[String key, params String[] sections]
        {
            get
            {
                return Config[key].Value;
            }
            set
            {
                if (CheckReadOnly())
                {
                    return;
                }

                Config[key].Value = value;
                
                WriteConfig();
            }
        }
    }
}