// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Common_Library.Utils
{
    public static class ConvertUtils
    {
        public static T Convert<T>(this Object obj)
        {
            TryConvert(obj, out T value);
            return value;
        }
        
        public static T Convert<T>(this String input)
        {
            TryConvert(input, out T value);
            return value;
        }
        
        public static Byte Convert(SByte value)
        {
            unchecked
            {
                if (value >= 0)
                {
                    return (Byte) value;
                }

                return (Byte) (value + SByte.MaxValue);
            }
        }
        
        public static UInt16 Convert(Int16 value)
        {
            unchecked
            {
                if (value >= 0)
                {
                    return (UInt16) value;
                }

                return (UInt16) (value + Int16.MaxValue);
            }
        }
        
        public static UInt32 Convert(Int32 value)
        {
            unchecked
            {
                if (value >= 0)
                {
                    return (UInt32) value;
                }

                return (UInt32) (value + Int32.MaxValue);
            }
        }
        
        public static UInt64 Convert(Int64 value)
        {
            unchecked
            {
                if (value >= 0)
                {
                    return (UInt64) value;
                }

                return (UInt64) (value + Int64.MaxValue);
            }
        }
        
        public static String Convert(this Object obj, IFormatProvider info = null)
        {
            return System.Convert.ToString(obj, info ?? CultureInfo.InvariantCulture);
        }

        public static Boolean TryConvert<T>(this String input, out T value)
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
        
        public static Boolean TryConvert<T1, T2>(this T1 input, out T2 value)
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
        
        public static Boolean ToBoolean<T>(this T obj)
        {
            return obj switch
            {
                String str => ToBoolean(str),
                ICollection collection => ToBoolean(collection),
                _ => !obj.Equals(default(T))
            };
        }
        
        public static Boolean ToBoolean(this String str)
        {
            return str?.ToUpper() switch
            {
                "TRUE" => true,
                "T" => true,
                "+" => true,
                "1" => true,
                _ => false
            };
        }

        public static Boolean ToBoolean(this ICollection collection)
        {
            return collection?.Count > 0;
        }
        
        public static String ToByteString(this Byte[] data)
        {
            return data == null ? null : BitConverter.ToString(data).Replace("-", String.Empty);
        }
    }
}