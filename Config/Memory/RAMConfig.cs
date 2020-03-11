// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using Common_Library.Utils;

namespace Common_Library.Config.RAM
{
    public class RAMConfig : Config
    {
        protected NestedDictionary<String, String> Config;

        public RAMConfig(String configPath = null, Boolean isReadOnly = true)
            : this(new NestedDictionary<String, String>(), configPath ?? "RAM", isReadOnly)
        {
        }

        public RAMConfig(NestedDictionary<String, String> config, String configPath = null, Boolean isReadOnly = true)
            : base("RAM", isReadOnly)
        {
            Config = config;
        }

        protected override String Get(String key, params String[] sections)
        {
            return Config[key].Value;
        }

        protected override void Set(String key, String value, params String[] sections)
        {
            Config[key].Value = value;
        }
    }
}