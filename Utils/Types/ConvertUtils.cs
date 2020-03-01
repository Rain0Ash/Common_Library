// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.ComponentModel;

namespace Common_Library.Utils
{
    public static class ConvertUtils
    {
        public static T Convert<T>(this String input)
        {
            Convert(input, out T value);
            return value;
        }
        
        public static Boolean Convert<T>(this String input, out T value)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                
                value = (T)converter.ConvertFromString(input);

                return true;
            }
            catch (Exception)
            {
                value = default;
                return false;
            }
        }
        
        public static T Convert<T>(Object obj)
        {
            Convert(obj, out T value);
            return value;
        }

        public static Boolean Convert<T1, T2>(T1 input, out T2 value)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T2));

                value = converter.ConvertFrom(input) is T2 ? (T2) converter.ConvertFrom(input) : default;

                return true;
            }
            catch (Exception)
            {
                value = default;
                return false;
            }
        }
    }
}