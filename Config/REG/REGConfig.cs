// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Reflection;

namespace Common_Library.Config.REG
{
    public sealed class REGConfig : Config
    {
        private readonly Common_Library.Registry.Registry _registry;

        public override Boolean IsReadOnly
        {
            get
            {
                return _registry.IsReadOnly;
            }
            set
            {
                if (_registry == null)
                {
                    return;
                }

                _registry.IsReadOnly = value;
            }
        }

        public REGConfig(String pathName = null, Boolean readOnly = true)
            : base(String.IsNullOrEmpty(pathName) ? Assembly.GetCallingAssembly().GetName().Name : pathName, readOnly)
        {
            _registry = new Common_Library.Registry.Registry(ConfigPath, readOnly);
        }

        public REGConfig(Registry.BaseKey baseKey, String path, Boolean readOnly = true)
            : base(path, readOnly)
        {
            _registry = new Common_Library.Registry.Registry(path, baseKey, readOnly);
        }

        protected override String Get(String key, params String[] sections)
        {
            return _registry.GetValue(key)?.ToString();
        }

        protected override void Set(String key, String value, params String[] sections)
        {
            _registry.SetValue(key, value);
        }

        public override void Dispose()
        {
            _registry.Dispose();
        }
    }
}