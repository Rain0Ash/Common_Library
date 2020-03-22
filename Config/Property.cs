// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using Common_Library.Crypto;
using Common_Library.Utils;

namespace Common_Library.Config
{
    public partial class Config
    {
        public void SetValue<T>(IConfigProperty<T> property, T value)
        {
            SetValue(property.Key, value, property.Crypt.HasFlag(CryptAction.Encrypt), property.CryptKey, property.Sections);
        }

        public T GetValue<T>(IConfigProperty<T> property)
        {
            return GetValue(property.Key, property.DefaultValue, property.Crypt.HasFlag(CryptAction.Encrypt), property.CryptKey, property.Sections);
        }

        public T GetOrSetValue<T>(IConfigProperty<T> property)
        {
            return GetOrSetValue(property.Key, property.DefaultValue, property.Crypt, property.CryptKey, property.Sections);
        }

        public Boolean KeyExist(IConfigPropertyBase property)
        {
            return KeyExist(property.Key, property.Sections);
        }

        public void RemoveValue(IConfigPropertyBase property)
        {
            RemoveValue(property.Key, property.Sections);
        }

        public IConfigProperty<T> GetProperty<T>(String key, params String[] sections)
        {
            return GetProperty(key, default(T), sections);
        }

        public IConfigProperty<T> GetProperty<T>(String key, T defaultValue, params String[] sections)
        {
            return GetProperty(key, defaultValue, null, sections);
        }

        public IConfigProperty<T> GetProperty<T>(String key, T defaultValue, Func<T, Boolean> validate, params String[] sections)
        {
            return GetProperty(key, defaultValue, validate, CryptAction.Decrypt, sections);
        }

        public IConfigProperty<T> GetProperty<T>(String key, T defaultValue, Func<T, Boolean> validate, CryptAction crypt, params String[] sections)
        {
            return GetProperty(key, defaultValue, validate, crypt, null, sections);
        }

        public IConfigProperty<T> GetProperty<T>(String key, T defaultValue, Func<T, Boolean> validate, CryptAction crypt, Byte[] cryptKey, params String[] sections)
        {
            return GetProperty(key, defaultValue, validate, crypt, cryptKey, CachingByDefault, sections);
        }

        public IConfigProperty<T> GetProperty<T>(String key, T defaultValue, Func<T, Boolean> validate, CryptAction crypt, Byte[] cryptKey, Boolean caching, params String[] sections)
        {
            return (IConfigProperty<T>) GetOrAddProperty(new ConfigProperty<T>(this, key, defaultValue, validate, crypt, cryptKey, caching, sections));
        }

        private static IConfigPropertyBase GetOrAddProperty(IConfigPropertyBase property)
        {
            return ConfigDictionary.GetOrAdd(property.Config, new IndexDictionary<String, IConfigPropertyBase>())
                .GetOrAdd(property.Path, property);
        }

        public IEnumerable<IConfigPropertyBase> GetProperties()
        {
            return ConfigDictionary.TryGetValue(this, out IndexDictionary<String, IConfigPropertyBase> dictionary) ? dictionary.Values : null;
        }

        public static void RemoveProperty(IConfigPropertyBase property)
        {
            if (!ConfigDictionary.TryGetValue(property.Config, out IndexDictionary<String, IConfigPropertyBase> dictionary))
            {
                return;
            }

            dictionary.Remove(property.Path);
            ClearProperty(property);
        }

        private void ForEachProperty(Action<IConfigPropertyBase> action)
        {
            if (ConfigDictionary.TryGetValue(this, out IndexDictionary<String, IConfigPropertyBase> dictionary))
            {
                dictionary.Values.ToList().ForEach(action);
            }
        }

        private static void ReadProperty(IConfigPropertyBase property)
        {
            property.Read();
        }

        private static void SaveProperty(IConfigPropertyBase property)
        {
            property.Save();
        }

        private static void ResetProperty(IConfigPropertyBase property)
        {
            property.Reset();
        }

        private static void ClearProperty(IConfigPropertyBase property)
        {
            property.Dispose(true);
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
    }
}