// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Text;
using Common_Library.Workstation;

namespace Common_Library.Crypto
{
    [Flags]
    public enum CryptAction
    {
        None = 0,
        Decrypt = 1,
        Encrypt = 2,
        Crypt = 3
    }

    public static partial class Cryptography
    {
        public static readonly Byte[] DefaultHash = CurrentUserCryptoSIDHash();

        public static Byte[] CurrentUserCryptoSIDHash()
        {
            return Hash.MD5(Encoding.UTF8.GetBytes(WorkStation.CurrentUserSID));
        }
    }
}