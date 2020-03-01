// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.IO;
using System.Security.Cryptography;

namespace Common_Library.Crypto
{
    public static partial class Cryptography
    {
        public static class AES
        {
            public static String Encrypt(String plainText, Byte[] key = null)
            {
                if (plainText == null)
                {
                    return null;
                }

                try
                {
                    key ??= CurrentUserCryptoSIDHash();
                    
                    Byte[] iv = new Byte[16];

                    key = Hash.MD5(key);
                    
                    using Aes aes = Aes.Create();
                    if (aes == null)
                    {
                        return null;
                    }

                    aes.Key = key;
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using MemoryStream memoryStream = new MemoryStream();
                    using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    Byte[] array = memoryStream.ToArray();

                    return Convert.ToBase64String(array);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            
            public static String Encrypt(String plainText, String key)
            {
                return Encrypt(plainText, StringToBytes(key));
            }

            public static String Decrypt(String cipherText, Byte[] key = null)
            {
                if (cipherText == null)
                {
                    return null;
                }

                try
                {
                    key ??= CurrentUserCryptoSIDHash();

                    Byte[] iv = new Byte[16];
                    Byte[] buffer = Convert.FromBase64String(cipherText);

                    key = Hash.MD5(key);
                    
                    using Aes aes = Aes.Create();
                    if (aes == null)
                    {
                        return null;
                    }

                    aes.Key = key;
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using MemoryStream memoryStream = new MemoryStream(buffer);
                    using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                    using StreamReader streamReader = new StreamReader(cryptoStream);
                    return streamReader.ReadToEnd();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            
            public static String Decrypt(String plainText, String key)
            {
                return Decrypt(plainText, StringToBytes(key));
            }
        }
    }
}