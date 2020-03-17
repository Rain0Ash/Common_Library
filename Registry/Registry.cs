// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using Common_Library.Exceptions;
using Common_Library.Utils.IO;
using Microsoft.Win32;
using Path = Common_Library.LongPath.Path;

namespace Common_Library.Registry
{
    public enum BaseKey
    {
        Default,
        Users,
        ClassesRoot,
        CurrentConfig,
        CurrentUser,
        LocalMachine,
        PerformanceData
    }
    
    public sealed class Registry : IDisposable
    {
        

        private static readonly Dictionary<BaseKey, RegistryKey> Keys = new Dictionary<BaseKey, RegistryKey>
        {
            {BaseKey.Default, Microsoft.Win32.Registry.CurrentUser},
            {BaseKey.Users, Microsoft.Win32.Registry.Users},
            {BaseKey.ClassesRoot, Microsoft.Win32.Registry.ClassesRoot},
            {BaseKey.CurrentConfig, Microsoft.Win32.Registry.CurrentConfig},
            {BaseKey.CurrentUser, Microsoft.Win32.Registry.CurrentUser},
            {BaseKey.LocalMachine, Microsoft.Win32.Registry.LocalMachine},
            {BaseKey.PerformanceData, Microsoft.Win32.Registry.PerformanceData}
        };

        public Boolean IsSafe { get; set; } = true;
        public Boolean IsReadOnly { get; set; } = true;
        private RegistryKey _registryKey;
        private Boolean _allowRequest = true;

        private String _initName;

        private Boolean CheckInitialize(Boolean autoInitialize = true, Boolean isThrow = false)
        {
            if (!String.IsNullOrEmpty(_initName))
            {
                return true;
            }

            if (autoInitialize)
            {
                _initName = Assembly.GetCallingAssembly().GetName().Name;
                return true;
            }

            if (isThrow)
            {
                throw new NotInitializedException(
                    $"Please initialize registry class by using \"Registry.Initialize(\"Your project name\")\" static method.\nOr use constructor autoInitialize = true, for initialize as \"{_initName}\".");
            }

            return false;
        }

        public String GetBasePath()
        {
            return $"{Path.Combine(Keys[BaseKey.Default].Name, @"Software", $"{_initName}")}";
        }

        public Registry(String pathName, Boolean readOnly = true, Boolean safe = true, Boolean autoInitialize = true)
        {
            _initName = pathName;
            CheckInitialize(autoInitialize, true);
            SetRegistryKey($"Software\\{_initName}", BaseKey.Default, readOnly, safe);
        }

        public Registry(RegistryKey registryPath, Boolean readOnly = true, Boolean safe = true, Boolean autoInitialize = true)
        {
            CheckInitialize(autoInitialize, true);
            SetRegistryKey(registryPath, readOnly, safe);
        }

        public Registry(String registryPath = null, BaseKey baseKey = BaseKey.Default, Boolean readOnly = true, Boolean safe = true, Boolean autoInitialize = true)
        {
            CheckInitialize(autoInitialize, true);
            SetRegistryKey(registryPath, baseKey, readOnly, safe);
        }

        public Boolean SetRegistryKey(RegistryKey registryPath, Boolean readOnly = true, Boolean isSafe = true)
        {
            _registryKey?.Close();
            IsSafe = isSafe;
            IsReadOnly = readOnly;
            try
            {
                _registryKey = registryPath;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean SetRegistryKey(String registryPath, BaseKey baseKey = BaseKey.Default, Boolean readOnly = true, Boolean safe = true)
        {
            _registryKey?.Close();
            IsSafe = safe;
            IsReadOnly = readOnly;
            try
            {
                Keys.TryGetValue(baseKey, out RegistryKey regKey);
                RegistryKeyPermissionCheck regKeyPerm =
                    readOnly ? RegistryKeyPermissionCheck.ReadSubTree : RegistryKeyPermissionCheck.Default;

                _registryKey = readOnly
                    ? regKey?.OpenSubKey(registryPath, regKeyPerm)
                    : regKey?.CreateSubKey(registryPath, regKeyPerm);
                return true;
            }
            catch (Exception)
            {
                _registryKey = null;
                return false;
            }
        }

        public void EndRequest()
        {
            _allowRequest = true;
        }

        public Registry GoTo(String nextSubKey)
        {
            return GoTo(nextSubKey, out _);
        }

        public Registry GoTo(String nextSubKey, out Boolean successful)
        {
            if (!_allowRequest)
            {
                successful = false;
                return this;
            }

            try
            {
                _registryKey = IsReadOnly
                    ? _registryKey.OpenSubKey(nextSubKey, RegistryKeyPermissionCheck.Default)
                    : _registryKey.CreateSubKey(nextSubKey, RegistryKeyPermissionCheck.Default);
                successful = true;
            }
            catch (Exception)
            {
                successful = false;
                _allowRequest = false;
            }

            return this;
        }

        public Registry GoBack(Int32 steps = 1)
        {
            return GoBack(out _, steps);
        }

        public Registry GoBack(out Boolean successful, Int32 steps = 1)
        {
            if (!_allowRequest)
            {
                successful = false;
                return this;
            }

            try
            {
                String[] regKeyArray = _registryKey.Name.Split('\\', '/');
                for (Int32 i = 0; i < steps; i++)
                {
                    if (IsSafe && regKeyArray[^1] == _initName || regKeyArray.Length < 2)
                    {
                        break;
                    }

                    regKeyArray = regKeyArray.Take(regKeyArray.Length - 1).ToArray();
                }

                _registryKey = _registryKey.OpenSubKey(String.Join("\\", regKeyArray),
                    RegistryKeyPermissionCheck.Default);
                successful = true;
            }
            catch (Exception)
            {
                successful = false;
                _allowRequest = false;
            }

            return this;
        }

        private Boolean CheckSafe(Boolean isThrow = false)
        {
            if (_registryKey != null && !IsReadOnly)
            {
                Boolean check = !IsSafe || Regex.IsMatch(_registryKey.Name, $@"^.*\\{_initName}(\\.*$|$)");
                if (isThrow && !check)
                {
                    throw new SecurityException();
                }

                return check;
            }

            if (isThrow)
            {
                throw new SecurityException();
            }

            return false;
        }

        public Registry SetValueRequest(String name, Object value, RegistryValueKind registryValueKind = RegistryValueKind.String,
            Boolean verifyBeforeOverwrite = true, Boolean isThrow = false)
        {
            return SetValueRequest(name, value, out _, registryValueKind, verifyBeforeOverwrite, isThrow);
        }

        public Registry SetValueRequest(String name, Object value, out Boolean successful,
            RegistryValueKind registryValueKind = RegistryValueKind.String, Boolean verifyBeforeOverwrite = true, Boolean isThrow = false)
        {
            if (!_allowRequest)
            {
                successful = false;
                return this;
            }

            _allowRequest = successful = SetValue(name, value, registryValueKind, verifyBeforeOverwrite, isThrow);
            return this;
        }

        public Boolean SetValue(String name, Object value, RegistryValueKind registryValueKind = RegistryValueKind.String,
            Boolean verifyBeforeOverwrite = true, Boolean isThrow = false)
        {
            try
            {
                CheckSafe(true);
                if (verifyBeforeOverwrite && GetValue(name) == value)
                {
                    return true;
                }

                if (value == null)
                {
                    RemoveValue(name, isThrow);
                    return true;
                }

                _registryKey?.SetValue(name, value, registryValueKind);
                return true;
            }
            catch (Exception)
            {
                if (isThrow)
                {
                    throw;
                }
            }

            return false;
        }

        public Registry GetValueRequest(String name, out Object returnValue, Object defaultValue = null,
            RegistryValueOptions registryValueOptions = RegistryValueOptions.None, Boolean isThrow = false)
        {
            if (!_allowRequest)
            {
                returnValue = defaultValue;
                return this;
            }

            returnValue = GetValue(name, defaultValue, registryValueOptions, isThrow);
            return this;
        }

        public Object GetValue(String name, Object defaultValue = null,
            RegistryValueOptions registryValueOptions = RegistryValueOptions.None, Boolean isThrow = false)
        {
            try
            {
                return _registryKey?.GetValue(name, defaultValue, registryValueOptions);
            }
            catch (Exception)
            {
                if (isThrow)
                {
                    throw;
                }
            }

            return null;
        }

        public void SetValues(Dictionary<String, ValueTuple<Object, RegistryValueKind>> valuesDictionary,
            Boolean verifyBeforeOverwrite = true)
        {
            if (!CheckSafe())
            {
                return;
            }

            foreach (String key in valuesDictionary.Keys)
            {
                if (valuesDictionary.TryGetValue(key, out ValueTuple<Object, RegistryValueKind> value) && value.Item1 != null)
                {
                    SetValue(key, value.Item1, value.Item2, verifyBeforeOverwrite);
                }
            }
        }

        public Registry RemoveValueRequest(String name, Boolean isThrow = false)
        {
            return RemoveValueRequest(name, out _, isThrow);
        }

        public Registry RemoveValueRequest(String name, out Boolean successful, Boolean isThrow = false)
        {
            if (!_allowRequest)
            {
                successful = false;
                return this;
            }

            successful = RemoveValue(name, isThrow);
            return this;
        }

        public Boolean RemoveValue(String name, Boolean isThrow = false)
        {
            try
            {
                CheckSafe(true);
                _registryKey?.DeleteValue(name);
                return true;
            }
            catch (Exception)
            {
                if (isThrow)
                {
                    throw;
                }
            }

            return false;
        }

        public Registry RemoveSubKeyRequest(String name, Boolean isRecursive = false, Boolean isThrow = false)
        {
            return RemoveSubKeyRequest(name, out _, isRecursive, isThrow);
        }

        public Registry RemoveSubKeyRequest(String name, out Boolean successful, Boolean isRecursive = false, Boolean isThrow = false)
        {
            if (!_allowRequest)
            {
                successful = false;
                return this;
            }

            successful = RemoveSubKey(name, isRecursive, isThrow);
            return this;
        }

        public Boolean RemoveSubKey(String name, Boolean isRecursive = false, Boolean isThrow = false)
        {
            try
            {
                CheckSafe(true);
                if (isRecursive)
                {
                    _registryKey?.DeleteSubKeyTree(name);
                }
                else
                {
                    _registryKey?.DeleteSubKey(name);
                }

                return true;
            }
            catch (Exception)
            {
                if (isThrow)
                {
                    throw;
                }
            }

            return false;
        }

        public Boolean RemoveCurrentSubKeyAndDispose(Boolean isRecursive = false, Boolean isThrow = false)
        {
            try
            {
                CheckSafe(true);
                RemoveSubKey(_registryKey.Name, isRecursive, isThrow);
                Dispose();
                return true;
            }
            catch (Exception)
            {
                if (isThrow)
                {
                    throw;
                }
            }

            return false;
        }

        public Boolean RemoveInitializedSubKeyAndDispose(Boolean isRecursive = false, Boolean isThrow = false)
        {
            throw new NotImplementedException();
        }

        public Dictionary<String, Object> GetValues()
        {
            Dictionary<String, Object> valueDictionary = new Dictionary<String, Object>();

            if (_registryKey == null)
            {
                return valueDictionary;
            }

            foreach (String name in _registryKey.GetValueNames())
            {
                valueDictionary.Add(name, GetValue(name));
            }

            return valueDictionary;
        }

        public Dictionary<String, Object> GetValues(Dictionary<String, ValueTuple<Object, RegistryValueOptions?>?> valuesDictionary)
        {
            Dictionary<String, Object> valueDictionary = new Dictionary<String, Object>();
            foreach (String name in valueDictionary.Keys)
            {
                ValueTuple<Object, RegistryValueOptions?>? value = valuesDictionary[name];
                if (value == null)
                {
                    valueDictionary.Add(name, GetValue(name));
                    continue;
                }

                valueDictionary.Add(name, GetValue(name, value.Value.Item1, value.Value.Item2 ?? RegistryValueOptions.None));
            }

            return valueDictionary;
        }

        public void RemoveValues(IEnumerable<String> names)
        {
            if (_registryKey == null || !CheckSafe())
            {
                return;
            }

            foreach (String name in names)
            {
                RemoveValue(name);
            }
        }

        public String GetPath()
        {
            return String.Join("\\", _registryKey?.Name?.Split(PathUtils.Separators).Skip(1) ?? new String[] { });
        }

        public override String ToString()
        {
            return _registryKey?.Name ?? String.Empty;
        }

        public void Dispose()
        {
            _registryKey?.Close();
            _registryKey?.Dispose();
        }

        ~Registry()
        {
            Dispose();
        }
    }
}