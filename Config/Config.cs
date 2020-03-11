// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Concurrent;
using System.Data;
using System.Globalization;
using System.Linq;
using Common_Library.Config.Registry;
using Common_Library.Config.INI;
using Common_Library.Config.XML;
using Common_Library.Config.RAM;
using Common_Library.Config.JSON;
using Common_Library.Crypto;
using Common_Library.Types.Other;
using Common_Library.Utils;
using JetBrains.Annotations;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
// ReSharper disable ConstantNullCoalescingCondition

namespace Common_Library.Config
{
    public enum ConfigType : Byte
    {
        Registry,
        INI,
        XML,
        RAM,
        JSON
    }

    public abstract class Config : IDisposable
    {
        protected const String DefaultName = "config";

        private static readonly ConcurrentDictionary<Config, ConcurrentDictionary<String[], ConfigPropertyBase>> ConfigDictionary =
            new ConcurrentDictionary<Config, ConcurrentDictionary<String[], ConfigPropertyBase>>();

        public static Config Factory(String configPath = null, Boolean isReadOnly = true,
            ConfigType configType = ConfigType.Registry)
        {
            return configType switch
            {
                ConfigType.INI => new INIConfig(configPath, isReadOnly),
                ConfigType.XML => new XMLConfig(configPath, isReadOnly),
                ConfigType.RAM => new RAMConfig(configPath, isReadOnly),
                ConfigType.JSON => new JSONConfig(configPath, isReadOnly),
                _ => (Config) new REGConfig(configPath, isReadOnly)
            };
        }

        public PathObject ConfigPath { get; }
        public virtual Boolean IsReadOnly { get; set; }

        public Boolean ThrowOnReadOnly { get; set; } = true;

        public Boolean CryptByDefault { get; set; }

        public Boolean CachingByDefault { get; set; } = true;

        protected Config(String configPath, Boolean isReadOnly)
            : this(new PathObject(configPath), isReadOnly)
        {
        }

        protected Config(PathObject configPath, Boolean isReadOnly)
        {
            ConfigPath = configPath;
            IsReadOnly = isReadOnly;
        }

        public void SetValue(String key, Object value, params String[] sections)
        {
            this[key, sections] = Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        public void SetValue(String key, Object value, Boolean encrypt, params String[] sections)
        {
            SetValue(key, value, encrypt, null, sections);
        }

        public void SetValue(String key, Object value, Boolean encrypt, Byte[] encryptKey, params String[] sections)
        {
            if (encrypt)
            {
                SetValue(key, Cryptography.AES.Encrypt(Convert.ToString(value, CultureInfo.InvariantCulture), encryptKey), sections);
                return;
            }

            SetValue(key, value, sections);
        }

        public void SetValue(ConfigProperty property, Object value)
        {
            SetValue(property.Key, value, property.Crypt, property.CryptKey, property.Sections);
        }

        public void SetValue<T>(ConfigProperty<T> property, T value)
        {
            SetValue(property.Key, value, property.Crypt, property.CryptKey, property.Sections);
        }

        public String GetValue(String key, params String[] sections)
        {
            return this[key, sections];
        }

        public String GetValue(String key, String defaultValue, params String[] sections)
        {
            return GetValue(key, sections) ?? defaultValue;
        }

        public String GetValue(String key, Object defaultValue, params String[] sections)
        {
            return GetValue(key, Convert.ToString(defaultValue, CultureInfo.InvariantCulture), sections);
        }

        public String GetValue(String key, Object defaultValue, Boolean decrypt, params String[] sections)
        {
            return GetValue(key, defaultValue, decrypt, null, sections);
        }

        public String GetValue(String key, Object defaultValue, Boolean decrypt, Byte[] decryptKey, params String[] sections)
        {
            String value = GetValue(key, defaultValue, sections);

            if (!decrypt || value == null)
            {
                return value;
            }

            return Cryptography.AES.Decrypt(value, decryptKey) ?? value;
        }

        public String GetValue(ConfigProperty property)
        {
            return GetValue(property.Key, property.DefaultValue, property.Crypt, property.CryptKey, property.Sections);
        }

        public T GetValue<T>(String key, params String[] sections)
        {
            return this[key, sections].Convert<T>();
        }

        public T GetValue<T>(String key, T defaultValue, params String[] sections)
        {
            String value = GetValue(key, sections);

            return value.TryConvert(out T cval) ? cval : defaultValue;
        }

        public T GetValue<T>(String key, T defaultValue, Boolean decrypt, params String[] sections)
        {
            return GetValue(key, defaultValue, decrypt, null, sections);
        }

        public T GetValue<T>(String key, T defaultValue, Boolean decrypt, Byte[] decryptKey, params String[] sections)
        {
            String value = GetValue(key, Convert.ToString(defaultValue, CultureInfo.InvariantCulture), sections);
            T cval;

            if (!decrypt || value == null)
            {
                return value.TryConvert(out cval) ? cval : defaultValue;
            }

            return (Cryptography.AES.Decrypt(value, decryptKey) ?? value).TryConvert(out cval) ? cval : defaultValue;
        }

        public T GetValue<T>(ConfigProperty<T> property)
        {
            return GetValue(property.Key, property.DefaultValue, property.Crypt, property.CryptKey, property.Sections);
        }

        public String GetOrSetValue(String key, Object defaultValue, params String[] sections)
        {
            return GetOrSetValue(key, defaultValue, false, null, sections);
        }

        public String GetOrSetValue(String key, Object defaultValue, Boolean crypt, params String[] sections)
        {
            return GetOrSetValue(key, defaultValue, crypt, null, sections);
        }

        public String GetOrSetValue(String key, Object defaultValue, Boolean crypt, Byte[] cryptKey, params String[] sections)
        {
            String value = GetValue(key, sections);

            if (value != null)
            {
                return crypt ? Cryptography.AES.Decrypt(value, cryptKey) ?? value : value;
            }

            SetValue(key, defaultValue, crypt, cryptKey, sections);
            return Convert.ToString(defaultValue, CultureInfo.InvariantCulture);
        }

        public String GetOrSetValue(ConfigProperty property)
        {
            return GetOrSetValue(property.Key, property.DefaultValue, property.Crypt, property.CryptKey, property.Sections);
        }

        public T GetOrSetValue<T>(String key, T defaultValue, params String[] sections)
        {
            return GetOrSetValue(key, defaultValue, false, null, sections);
        }

        public T GetOrSetValue<T>(String key, T defaultValue, Boolean crypt, params String[] sections)
        {
            return GetOrSetValue(key, defaultValue, crypt, null, sections);
        }

        public T GetOrSetValue<T>(String key, T defaultValue, Boolean crypt, Byte[] cryptKey, params String[] sections)
        {
            String value = GetValue(key, sections);

            if (value != null)
            {
                T cval;

                if (crypt)
                {
                    return (Cryptography.AES.Decrypt(value, cryptKey) ?? value).TryConvert(out cval) ? cval : defaultValue;
                }

                return value.TryConvert(out cval) ? cval : defaultValue;
            }

            SetValue(key, defaultValue, crypt, cryptKey, sections);
            return defaultValue;
        }

        public T GetOrSetValue<T>(ConfigProperty<T> property)
        {
            return GetOrSetValue(property.Key, property.DefaultValue, property.Crypt, property.CryptKey, property.Sections);
        }

        public Boolean KeyExist(String key, params String[] sections)
        {
            return GetValue(key, sections) != null;
        }

        public Boolean KeyExist(ConfigProperty property)
        {
            return KeyExist(property.Key, property.Sections);
        }

        public void RemoveValue(String key, params String[] sections)
        {
            SetValue(key, null, sections);
        }

        public void RemoveValue(ConfigProperty property)
        {
            RemoveValue(property.Key, property.Sections);
        }

        protected abstract String Get(String key, params String[] sections);
        protected abstract void Set(String key, String value, params String[] sections);

        protected String this[String key, params String[] sections]
        {
            get
            {
                return Get(key, sections);
            }
            set
            {
                if (CheckReadOnly())
                {
                    return;
                }

                Set(key, value, sections);
            }
        }


        public ConfigProperty GetProperty(String key, params String[] sections)
        {
            return GetProperty(key, null, sections);
        }

        public ConfigProperty GetProperty(String key, Object defaultValue, params String[] sections)
        {
            return GetProperty(key, defaultValue, CryptByDefault, sections);
        }

        public ConfigProperty GetProperty(String key, Object defaultValue, Boolean crypt, params String[] sections)
        {
            return GetProperty(key, defaultValue, crypt, null, sections);
        }

        public ConfigProperty GetProperty(String key, Object defaultValue, Boolean crypt, Byte[] cryptKey, params String[] sections)
        {
            return GetProperty(key, defaultValue, crypt, cryptKey, CachingByDefault, sections);
        }

        public ConfigProperty GetProperty(String key, Object defaultValue, Boolean crypt, Byte[] cryptKey, Boolean caching,
            params String[] sections)
        {
            return (ConfigProperty) GetOrAddProperty(new ConfigProperty(this, key, defaultValue, crypt, cryptKey, caching, sections));
        }

        public ConfigProperty<T> GetProperty<T>(String key, params String[] sections)
        {
            return GetProperty(key, default(T), sections);
        }

        public ConfigProperty<T> GetProperty<T>(String key, T defaultValue, params String[] sections)
        {
            return GetProperty(key, defaultValue, CryptByDefault, sections);
        }

        public ConfigProperty<T> GetProperty<T>(String key, T defaultValue, Boolean crypt, params String[] sections)
        {
            return GetProperty(key, defaultValue, crypt, null, sections);
        }

        public ConfigProperty<T> GetProperty<T>(String key, T defaultValue, Boolean crypt, Byte[] cryptKey, params String[] sections)
        {
            return GetProperty(key, defaultValue, crypt, cryptKey, CachingByDefault, sections);
        }

        public ConfigProperty<T> GetProperty<T>(String key, T defaultValue, Boolean crypt, Byte[] cryptKey, Boolean caching,
            params String[] sections)
        {
            return (ConfigProperty<T>) GetOrAddProperty(new ConfigProperty<T>(this, key, defaultValue, crypt, cryptKey, caching, sections));
        }

        private static ConfigPropertyBase GetOrAddProperty(ConfigPropertyBase property)
        {
            return ConfigDictionary.GetOrAdd(property.Config, new ConcurrentDictionary<String[], ConfigPropertyBase>())
                .GetOrAdd(property.Sections.Append(property.Key).ToArray(), property);
        }

        public void ForEachProperty(Action<ConfigPropertyBase> action)
        {
            if (ConfigDictionary.TryGetValue(this, out ConcurrentDictionary<String[], ConfigPropertyBase> dictionary))
            {
                dictionary.Values.ToList().ForEach(action);
            }
        }

        private static void ReadProperty(ConfigPropertyBase property)
        {
            property.Read();
        }

        private static void SaveProperty(ConfigPropertyBase property)
        {
            property.Save();
        }

        private static void ResetProperty(ConfigPropertyBase property)
        {
            property.Reset();
        }

        private static void ClearProperty(ConfigPropertyBase property)
        {
            property.Dispose();
        }

        public void ReadProperties()
        {
            ForEachProperty(ReadProperty);
        }

        public void SaveProperties()
        {
            ForEachProperty(SaveProperty);
        }

        public void ResetProperties()
        {
            ForEachProperty(ResetProperty);
        }

        public void ClearProperties()
        {
            ForEachProperty(ClearProperty);
            ConfigDictionary.TryGetValue(this)?.Clear();
        }

        protected Boolean CheckReadOnly()
        {
            if (IsReadOnly && ThrowOnReadOnly)
            {
                throw new ReadOnlyException("Readonly mode");
            }

            return IsReadOnly;
        }

        public override String ToString()
        {
            return ConfigPath;
        }

        public virtual void Dispose()
        {
        }
    }
}