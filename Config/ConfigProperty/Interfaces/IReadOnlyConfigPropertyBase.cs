// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Config
{
    public interface IReadOnlyConfigPropertyBase
    {
        public String Path { get; }
        public Config Config { get; }
        public String Key { get; }
        public String[] Sections { get; }
        public Boolean Crypt { get; }
        public Byte[] CryptKey { get; }
        public Boolean Caching { get; }
        
        public void Read();
        
        public Boolean KeyExist();
    }
}