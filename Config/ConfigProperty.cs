// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Globalization;

namespace Common_Library.Config
{
    public class ConfigProperty : ConfigProperty<Object>
    {
        public ConfigProperty(String key, params String[] sections)
            : base(key, sections)
        {
        }
        
        public ConfigProperty(Config config, String key, params String[] sections)
            : base(config, key, sections)
        {
        }
        
        public ConfigProperty(String key, Object defaultValue, params String[] sections)
            : base(key, defaultValue, sections)
        {
        }
        
        public ConfigProperty(Config config, String key, Object defaultValue, params String[] sections)
            : base(config, key, defaultValue, sections)
        {
        }
        
        public ConfigProperty(String key, Object defaultValue, Boolean crypt, params String[] sections)
            : base(key, defaultValue, crypt, sections)
        {
        }
        
        public ConfigProperty(Config config, String key, Object defaultValue, Boolean crypt, params String[] sections)
            : base(config, key, defaultValue, crypt, sections)
        {
        }
        
        public ConfigProperty(String key, Object defaultValue, Boolean crypt, Byte[] cryptKey, params String[] sections)
            : base(key, defaultValue, crypt, cryptKey, sections)
        {
        }
        
        public ConfigProperty(Config config, String key, Object defaultValue, Boolean crypt, Byte[] cryptKey, params String[] sections)
            : base(config, key, defaultValue, crypt, cryptKey, sections)
        {
        }
    }
    
    public class ConfigProperty<T>
    {
        public static implicit operator String(ConfigProperty<T> property)
        {
            return property.ToString();
        }

        private Config _config;
        
        public Config Config
        {
            get
            {
                return _config ?? Config.StandartConfig;
            }
            set
            {
                _config = value;
            }
        }

        public String Key { get; }
        public String[] Sections { get; }
        public T DefaultValue { get; set; }
        public Boolean Crypt { get; set; }
        public Byte[] CryptKey { get; set; }

        public ConfigProperty(String key, params String[] sections)
        {
            Key = key;
            Sections = sections;
        }
        
        public ConfigProperty(Config config, String key, params String[] sections)
            : this(key, sections)
        {
            Config = config;
        }
        
        public ConfigProperty(String key, T defaultValue, params String[] sections)
            : this(key, sections)
        {
            DefaultValue = defaultValue;
        }
        
        public ConfigProperty(Config config, String key, T defaultValue, params String[] sections)
            : this(key, defaultValue, sections)
        {
            Config = config;
        }

        public ConfigProperty(String key, T defaultValue, Boolean crypt, params String[] sections)
            : this(key, defaultValue, sections)
        {
            Crypt = crypt;
        }
        
        public ConfigProperty(Config config, String key, T defaultValue, Boolean crypt, params String[] sections)
            : this(key, defaultValue, crypt, sections)
        {
            Config = config;
        }
        
        public ConfigProperty(String key, T defaultValue, Boolean crypt, Byte[] cryptKey, params String[] sections)
            : this(key, defaultValue, crypt, sections)
        {
            CryptKey = cryptKey;
        }
        
        public ConfigProperty(Config config, String key, T defaultValue, Boolean crypt, Byte[] cryptKey, params String[] sections)
            : this(key, defaultValue, crypt, cryptKey, sections)
        {
            Config = config;
        }
        
        public void SetValue(T value, Config config = null)
        {
            config ??= Config;
            config.SetValue(this, value);
        }
        
        public T GetValue(Config config = null)
        {
            config ??= Config;
            return config.GetValue(this);
        }
        
        public T GetOrSetValue(Config config = null)
        {
            config ??= Config;
            return config.GetOrSetValue(this);
        }
        
        public Boolean KeyExist(Config config = null)
        {
            config ??= Config;
            return config.KeyExist(Key, Sections);
        }
        
        public void RemoveValue(Config config = null)
        {
            config ??= Config;
            config.RemoveValue(Key, Sections);
        }

        public override String ToString()
        {
            return Convert.ToString(GetValue(), CultureInfo.InvariantCulture);
        }
    }
}