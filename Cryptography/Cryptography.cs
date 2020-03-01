// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Text;
using Common_Library.Workstation;

namespace Common_Library.Crypto
{
    public static partial class Cryptography
    {
        public static Byte[] StringToBytes(String str)
        {
            return str != null ? Encoding.UTF8.GetBytes(str) : null;
        }

        public static Byte[] CurrentUserCryptoSIDHash()
        {
            return Hash.MD5(Encoding.UTF8.GetBytes(WorkStation.CurrentUserSID()));
        }
    }
}