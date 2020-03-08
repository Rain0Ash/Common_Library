// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Security.Cryptography;
using Common_Library.Utils;

namespace Common_Library.Crypto
{
    public static partial class Cryptography
    {
        public static class Hash
        {
            public static Byte[] Sha1(Byte[] data)
            {
                using SHA1 sha1 = new SHA1Managed();
                return sha1.ComputeHash(data);
            }
            
            public static String Sha1String(Byte[] data)
            {
                return Sha1(data).ToByteString();
            }

            public static Byte[] Sha256(Byte[] data)
            {
                using SHA256 sha256 = new SHA256Managed();
                return sha256.ComputeHash(data);
            }
            
            public static String Sha256String(Byte[] data)
            {
                return Sha256(data).ToByteString();
            }
            
            public static Byte[] Sha384(Byte[] data)
            {
                using SHA384 sha384 = new SHA384Managed();
                return sha384.ComputeHash(data);
            }

            public static String Sha384String(Byte[] data)
            {
                return Sha384(data).ToByteString();
            }

            public static Byte[] Sha512(Byte[] data)
            {
                using SHA512 sha512 = new SHA512Managed();
                return sha512.ComputeHash(data);
            }
            
            public static String Sha512String(Byte[] data)
            {
                return Sha512(data).ToByteString();
            }

            public static Byte[] MD5(Byte[] data)
            {
                using MD5 md5 = System.Security.Cryptography.MD5.Create();
                return md5.ComputeHash(data);
            }
            
            public static String MD5String(Byte[] data)
            {
                return MD5(data).ToByteString();
            }
            
            public static Byte Crc8(Byte[] data)
            {
                Int32 size = data.Length;
                return Crc8(data, size);
            }
            
            public static Byte Crc8(Byte[] data, Int32 size)
            {
                Int32 len = size;
                UInt32 checksum = 0;
                for (Int32 i = 0; i <= len - 1; i++)
                {
                    checksum *= 0x13;
                    checksum += data[i];
                }

                return (Byte)checksum;
            }
        }
    }
}