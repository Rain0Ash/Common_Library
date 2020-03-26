// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Common_Library.Config.REG;
using Common_Library.Config.INI;
using Common_Library.Config.XML;
using Common_Library.Config.RAM;
using Common_Library.Config.JSON;
using Common_Library.Crypto;
using Common_Library.Types.Other;
using Common_Library.Utils;

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

    public abstract partial class Config
    {
        protected const String DefaultName = "config";

        private static readonly IndexDictionary<Config, IndexDictionary<String, IConfigPropertyBase>> ConfigDictionary =
            new IndexDictionary<Config, IndexDictionary<String, IConfigPropertyBase>>();

        public static Config Factory(String configPath = null, Boolean isReadOnly = true, ConfigType configType = ConfigType.Registry)
        {
            return configType switch
            {
                ConfigType.Registry => new REGConfig(configPath, isReadOnly),
                ConfigType.INI => new INIConfig(configPath, isReadOnly),
                ConfigType.XML => new XMLConfig(configPath, isReadOnly),
                ConfigType.RAM => new RAMConfig(configPath, isReadOnly),
                ConfigType.JSON => new JSONConfig(configPath, isReadOnly),
                _ => throw new NotImplementedException()
            };
        }

        public FSWatcher ConfigPath { get; }

        public virtual Boolean IsReadOnly { get; set; }

        public Boolean ThrowOnReadOnly { get; set; } = true;

        public Boolean CryptByDefault { get; set; }

        public Boolean CachingByDefault { get; set; } = true;

        protected Config(String configPath, Boolean isReadOnly)
            : this(new FSWatcher(configPath), isReadOnly)
        {
        }

        protected Config(FSWatcher configPath, Boolean isReadOnly)
        {
            ConfigPath = configPath;
            IsReadOnly = isReadOnly;
        }

        protected virtual String ConvertToValue<T>(T value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        public void SetValue(String key, Object value, params String[] sections)
        {
            this[key, sections] = ConvertToValue(value);
        }

        public void SetValue(String key, Object value, Boolean encrypt, params String[] sections)
        {
            SetValue(key, value, encrypt, null, sections);
        }

        public void SetValue(String key, Object value, Boolean crypt, Byte[] cryptKey, params String[] sections)
        {
            if (crypt)
            {
                SetValue(key, Cryptography.AES.Encrypt(ConvertToValue(value), cryptKey), sections);
                return;
            }

            SetValue(key, value, sections);
        }

        protected virtual T ConvertFromValue<T>(String value)
        {
            return value.Convert<T>();
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

        public String GetValue(String key, Object defaultValue, Boolean decrypt, Byte[] cryptKey, params String[] sections)
        {
            String value = GetValue(key, defaultValue, sections);

            if (!decrypt || value == null)
            {
                return value;
            }

            return Cryptography.AES.Decrypt(value, cryptKey) ?? value;
        }

        public T GetValue<T>(String key, params String[] sections)
        {
            return ConvertFromValue<T>(this[key, sections]);
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

        public T GetValue<T>(String key, T defaultValue, Boolean decrypt, Byte[] cryptKey, params String[] sections)
        {
            String value = GetValue(key, Convert.ToString(defaultValue, CultureInfo.InvariantCulture), sections);
            T cval;

            if (!decrypt || value == null)
            {
                return value.TryConvert(out cval) ? cval : defaultValue;
            }

            return (Cryptography.AES.Decrypt(value, cryptKey) ?? value).TryConvert(out cval) ? cval : defaultValue;
        }

        public String GetOrSetValue(String key, Object defaultValue, params String[] sections)
        {
            return GetOrSetValue(key, defaultValue, CryptAction.Decrypt, sections);
        }

        public String GetOrSetValue(String key, Object defaultValue, CryptAction crypt, params String[] sections)
        {
            return GetOrSetValue(key, defaultValue, crypt, null, sections);
        }

        public String GetOrSetValue(String key, Object defaultValue, CryptAction crypt, Byte[] cryptKey, params String[] sections)
        {
            String value = GetValue(key, sections);

            if (value != null)
            {
                return crypt.HasFlag(CryptAction.Decrypt) ? Cryptography.AES.Decrypt(value, cryptKey) ?? value : value;
            }

            SetValue(key, defaultValue, crypt.HasFlag(CryptAction.Encrypt), cryptKey, sections);
            return Convert.ToString(defaultValue, CultureInfo.InvariantCulture);
        }

        public T GetOrSetValue<T>(String key, T defaultValue, params String[] sections)
        {
            return GetOrSetValue(key, defaultValue, CryptAction.Decrypt, sections);
        }

        public T GetOrSetValue<T>(String key, T defaultValue, CryptAction crypt, params String[] sections)
        {
            return GetOrSetValue(key, defaultValue, crypt, null, sections);
        }

        public T GetOrSetValue<T>(String key, T defaultValue, CryptAction crypt, Byte[] cryptKey, params String[] sections)
        {
            String value = GetValue(key, sections);

            if (value != null)
            {
                if (crypt.HasFlag(CryptAction.Decrypt))
                {
                    value = Cryptography.AES.Decrypt(value, cryptKey) ?? value;
                }

                return value.TryConvert(out T cval) ? cval : defaultValue;
            }

            SetValue(key, defaultValue, crypt.HasFlag(CryptAction.Encrypt), cryptKey, sections);
            return defaultValue;
        }

        public Boolean KeyExist(String key, params String[] sections)
        {
            return GetValue(key, sections) != null;
        }

        public void RemoveValue(String key, params String[] sections)
        {
            SetValue(key, null, sections);
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