// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using Common_Library.Crypto;

namespace Common_Library.Utils.Types
{
    public static class HashUtils
    {
        public static Byte[] GetHash(this Object obj, HashType type = HashType.MD5)
        {
            return obj switch
            {
                Byte[] bytes => Cryptography.Hash.Hashing(bytes, type),
                IEnumerable<Byte> bytes => Cryptography.Hash.Hashing(bytes.ToArray(), type),
                String str => Cryptography.Hash.Hashing(str.ToBytes(), type),
                _ => Cryptography.Hash.Hashing(obj.Serialize(), type),
            };
        }
    }
}