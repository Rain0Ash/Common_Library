// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Config
{
    public interface IConfigProperty<T> : IReadOnlyConfigProperty<T>, IConfigPropertyBase
    {
        public void SetValue(T value);
        public T GetOrSetValue();
        public void ChangeDefaultValue(T newValue, Boolean changeValue = true);
        public void ResetValue();
        public void RemoveValue();
    }
}