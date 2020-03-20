// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using Common_Library.Crypto;
using Common_Library.Utils;

namespace Common_Library.Config
{
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
        
        public T DefaultValue { get; private set; }

        public event Handlers.EmptyHandler Changed; 
        
        private T _value;
        public T Value
        {
            get
            {
                return IsValid ? _value : DefaultValue;
            }
            private set
            {
                if (_value?.Equals(value) == true)
                {
                    return;
                }

                _value = value;

                IsValid = Validate?.Invoke(_value) != false;

                Changed?.Invoke();
            }
        }

        public Boolean IsValid { get; private set; }

        public Func<T, Boolean> Validate { get; }

        internal ConfigProperty(Config config, String key, T defaultValue, Func<T, Boolean> validate, CryptAction crypt, Byte[] cryptKey, Boolean caching,
            params String[] sections)
            : base(config, key, crypt, cryptKey, caching, sections)
        {
            DefaultValue = defaultValue;
            Validate = validate;

            Read();
        }

        public void SetValue(T value)
        {
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

            return Value;
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

        public void ChangeDefaultValue(T value, Boolean changeValue = true)
        {
            if (changeValue && DefaultValue.Equals(Value))
            {
                Value = value;
            }
            
            DefaultValue = value;
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
        public CryptAction Crypt { get; set; }
        public Byte[] CryptKey { get; set; }
        public Boolean Caching { get; set; }

        private Boolean _disposed;

        protected ConfigPropertyBase(Config config, String key, CryptAction crypt, Byte[] cryptKey, Boolean caching, params String[] sections)
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

        public void Dispose(Boolean disposing)
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