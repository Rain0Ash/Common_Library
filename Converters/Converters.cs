// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Converters
{
    public static class Converters
    {
        public static Boolean ToBoolean(Object? obj)
        {
            return obj?.ToString().ToUpper() switch
            {
                "TRUE" => true,
                "T" => true,
                "+" => true,
                "1" => true,
                _ => false
            };
        }

        public static String ToByteString(this Byte[] data)
        {
            return data == null ? null : BitConverter.ToString(data).Replace("-", String.Empty);
        }
    }
}