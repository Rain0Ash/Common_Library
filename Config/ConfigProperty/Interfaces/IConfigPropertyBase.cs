// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using Common_Library.Crypto;

namespace Common_Library.Config
{
    public interface IConfigPropertyBase : IReadOnlyConfigPropertyBase, IDisposable
    {
        public new CryptAction Crypt { get; set; }
        public new Byte[] CryptKey { get; set; }
        public new Boolean Caching { get; set; }

        public void Save();

        public void Reset();

        public void Dispose(Boolean disposing);
    }
}