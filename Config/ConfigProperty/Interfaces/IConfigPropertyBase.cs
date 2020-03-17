// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Config
{
    public interface IConfigPropertyBase : IReadOnlyConfigPropertyBase
    {
        public new Boolean Crypt { get; set; }
        public new Byte[] CryptKey { get; set; }
        public new Boolean Caching { get; set; }

        public void Save();

        public void Reset();
    }
}