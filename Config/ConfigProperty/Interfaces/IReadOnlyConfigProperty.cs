// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Config
{
    public interface IReadOnlyConfigProperty<out T> : IReadOnlyConfigPropertyBase
    {
        public T DefaultValue { get; }
        public T Value { get; }
        public T GetValue();
        public T GetValue(Func<T, Boolean> validate);
    }
}