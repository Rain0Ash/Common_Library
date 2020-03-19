// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common_Library.Utils;

namespace Common_Library.Config
{
    public class ConfigProperty : ConfigProperty<Object>
    {
        internal ConfigProperty(Config config, String key, Object defaultValue, Func<Object, Boolean> validate,
            Boolean crypt, Byte[] cryptKey, Boolean caching, params String[] sections)
            : base(config, key, defaultValue, validate, crypt, cryptKey, caching, sections)
        {
        }
    }

    public class ConfigProperty<T> : ConfigPropertyBase, IConfigProperty<T>
    {
        public static implicit operator T(ConfigProperty<T> property)
        {
            return property.GetValue();
        }
        
        public static explicit operator String(ConfigProperty<T> property)
        {
            return property.ToString();
        }

        public T DefaultValue { get; set; }

        public T Value { get; private set; }
        
        public Func<T, Boolean> Validate { get; set; }

        internal ConfigProperty(Config config, String key, T defaultValue, Func<T, Boolean> validate, Boolean crypt, Byte[] cryptKey, Boolean caching, params String[] sections)
            : base(config, key, crypt, cryptKey, caching, sections)
        {
            DefaultValue = defaultValue;
            Validate = validate;
            
            Read();
        }

        public void SetValue(T value)
        {
            if (Validate?.Invoke(value) == false)
            {
                throw new ValidationException($"{nameof(value)} is invalid");
            }
            
            Value = value;

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

            return Validate?.Invoke(Value) == false ? DefaultValue : Value;
        }

        public T GetOrSetValue()
        {
            if (!Caching)
            {
                Value = Config.GetOrSetValue(this);
                return Value;
            }

            Read();
            return Value;
        }

        public void ResetValue()
        {
            Value = DefaultValue;
        }

        public void RemoveValue()
        {
            if (!Caching)
            {
                Config.RemoveValue(Key, Sections);
            }

            ResetValue();
        }

        public override void Save()
        {
            Config.SetValue(this, Value);
        }

        public override void Read()
        {
            Value = Config.GetValue(this);
        }

        public override void Reset()
        {
            ResetValue();
            Save();
        }

        public override String ToString()
        {
            return GetValue().Convert();
        }
    }

    public abstract class ConfigPropertyBase : IConfigPropertyBase
    {
        public static implicit operator String(ConfigPropertyBase property)
        {
            return property.ToString();
        }

        public String Path
        {
            get
            {
                return String.Join("\\", Sections.Append(Key));
            }
        }

        public Config Config { get; }
        public String Key { get; }
        public String[] Sections { get; }
        public Boolean Crypt { get; set; }
        public Byte[] CryptKey { get; set; }
        public Boolean Caching { get; set; }

        private Boolean _disposed;

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

        public Boolean KeyExist()
        {
            return Config.KeyExist(Key, Sections);
        }
        
        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(_disposed);
            }
        }

        internal void Dispose(Boolean disposing)
        {
            if (!disposing)
            {
                Config.RemoveProperty(this);
            }

            _disposed = true;
            Dispose();
        }
    }
}