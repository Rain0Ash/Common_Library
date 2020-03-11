// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Config.INI;
using Common_Library.Utils;

namespace Common_Library.Config
{
    public class ConfigProperty : ConfigProperty<Object>
    {
        internal ConfigProperty(Config config, String key, Object defaultValue, Boolean crypt, Byte[] cryptKey, Boolean caching,
            params String[] sections)
            : base(config, key, defaultValue, crypt, cryptKey, caching, sections)
        {
        }
    }

    public class ConfigProperty<T> : ConfigPropertyBase
    {
        public static implicit operator String(ConfigProperty<T> property)
        {
            return property.ToString();
        }

        public T DefaultValue { get; set; }

        public T Cache { get; protected set; }

        internal ConfigProperty(Config config, String key, T defaultValue, Boolean crypt, Byte[] cryptKey, Boolean caching,
            params String[] sections)
            : base(config, key, crypt, cryptKey, caching, sections)
        {
            DefaultValue = defaultValue;

            Read();
        }

        public void SetValue(T value)
        {
            Cache = value;

            if (!Caching)
            {
                Save();
            }
        }

        public T GetValue()
        {
            if (!Caching)
            {
                Read();
            }

            return Cache;
        }

        public T GetOrSetValue()
        {
            return Config.GetOrSetValue(this);
        }

        public Boolean KeyExist()
        {
            return Config.KeyExist(Key, Sections);
        }

        public void RemoveValue()
        {
            Config.RemoveValue(Key, Sections);
        }

        public override void Save()
        {
            Config.SetValue(this, Cache);
        }

        public override void Read()
        {
            Cache = Config.GetValue(this);
        }

        public override void Reset()
        {
            Cache = DefaultValue;
            Save();
        }

        public override String ToString()
        {
            return GetValue().Convert();
        }
    }

    public abstract class ConfigPropertyBase
    {
        public static implicit operator String(ConfigPropertyBase property)
        {
            return property.ToString();
        }

        public Config Config { get; }
        public String Key { get; }
        public String[] Sections { get; }
        public Boolean Crypt { get; set; }
        public Byte[] CryptKey { get; set; }
        public Boolean Caching { get; set; }

        protected ConfigPropertyBase(Config config, String key, Boolean crypt, Byte[] cryptKey, Boolean caching, params String[] sections)
        {
            Config = config;
            Key = key;
            Crypt = crypt;
            CryptKey = cryptKey;
            Sections = sections;
            Caching = caching;
        }

        public abstract void Read();

        public abstract void Save();

        public abstract void Reset();

        public void Dispose()
        {
        }
    }
}