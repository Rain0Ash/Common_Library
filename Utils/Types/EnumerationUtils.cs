// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    public static class EnumerationUtils
    {
        public static T RandomEnumValue<T>()
        {  
            Type type = typeof(T);
            Array values = Enum.GetValues(type);
            Object value = values.GetValue(RandomUtils.Next(0, values.Length));
            
            return (T)Convert.ChangeType(value, type);
        }
    }
}