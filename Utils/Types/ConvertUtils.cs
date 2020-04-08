// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Common_Library.Exceptions;
using Common_Library.Utils.Math;

namespace Common_Library.Utils
{
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
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
        
        public static IEnumerable<T> Convert<T>(this IEnumerable source)
        {
            return source.Cast<T>();
        }
        
        public static IEnumerable<T> TryConvert<T>(this IEnumerable source)
        {
            return source.OfType<T>();
        }

        #region DecimalConvert

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SByte ToSByte(this Decimal value)
        {
            return System.Convert.ToSByte(MathUtils.Range(value, SByte.MinValue, SByte.MaxValue));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte ToByte(this Decimal value)
        {
            return value >= 0
                ? System.Convert.ToByte(MathUtils.Range(value, Byte.MinValue, Byte.MaxValue))
                : Convert(ToSByte(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16(this Decimal value)
        {
            return System.Convert.ToInt16(MathUtils.Range(value, Int16.MinValue, Int16.MaxValue));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16(this Decimal value)
        {
            return value >= 0
                ? System.Convert.ToUInt16(MathUtils.Range(value, UInt16.MinValue, UInt16.MaxValue))
                : Convert(ToInt16(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32(this Decimal value)
        {
            return System.Convert.ToInt32(MathUtils.Range(value, Int32.MinValue, Int32.MaxValue));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32(this Decimal value)
        {
            return value >= 0
                ? System.Convert.ToUInt32(MathUtils.Range(value, UInt32.MinValue, UInt32.MaxValue))
                : Convert(ToInt32(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64(this Decimal value)
        {
            return System.Convert.ToInt64(MathUtils.Range(value, Int64.MinValue, Int64.MaxValue));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64(this Decimal value)
        {
            return value >= 0
                ? System.Convert.ToUInt64(MathUtils.Range(value, UInt64.MinValue, UInt64.MaxValue))
                : Convert(ToInt64(value));
        }

        #endregion

        #region UTypeConvert

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        #endregion

        #region EnumConvert

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SByte ToSByte<T>(this T value) where T : Enum
        {
            return (SByte) (Object) value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte ToByte<T>(this T value) where T : Enum
        {
            return (Byte) (Object) value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16<T>(this T value) where T : Enum
        {
            return (Int16) (Object) value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16<T>(this T value) where T : Enum
        {
            return (UInt16) (Object) value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32<T>(this T value) where T : Enum
        {
            return (Int32) (Object) value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32<T>(this T value) where T : Enum
        {
            return (UInt32) (Object) value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64<T>(this T value) where T : Enum
        {
            return (Int64) (Object) value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64<T>(this T value) where T : Enum
        {
            return (UInt64) (Object) value;
        }

        #endregion

        public static Byte[] ToBytes(this String input)
        {
            return ToBytes(input, Encoding.UTF8);
        }

        public static Byte[] ToBytes(this String input, Encoding encoding)
        {
            return input == null ? null : encoding.GetBytes(input);
        }

        #region ToString

        public static String GetString(this Object obj)
        {
            return GetString(obj, CultureInfo.InvariantCulture);
        }
        
        public static String GetString(this Object value, IFormatProvider provider, Boolean escape = false)
        {
            return value switch
            {
                null => null,
                Char chr => escape ? $"\'{chr.GetString(provider)}\'" : chr.GetString(provider), 
                String str => escape ? $"\"{str}\"" : str,
                IDictionary id => id.GetString(provider),
                IEnumerable ie => ie.GetString(provider),
                IFormattable formattable => formattable.ToString(null, provider),
                IConvertible ic => ic.ToString(provider),
                _ => value.ToString()
            };
        }

        public static String GetString(this Boolean value)
        {
            return value.ToString();
        }
        
        public static String GetString(this Boolean value, IFormatProvider provider)
        {
            return value.ToString();
        }

        public static String GetString(this Char value)
        {
            return Char.ToString(value);
        }

        public static String GetString(this Char value, IFormatProvider provider)
        {
            return Char.ToString(value);
        }
        
        public static String GetString(this SByte value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
        
        public static String GetString(this SByte value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static String GetString(this Byte value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String GetString(this Byte value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static String GetString(this Int16 value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String GetString(this Int16 value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

                public static String GetString(this UInt16 value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

                public static String GetString(this UInt16 value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static String GetString(this Int32 value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String GetString(this Int32 value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

                public static String GetString(this UInt32 value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

                public static String GetString(this UInt32 value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static String GetString(this Int64 value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String GetString(this Int64 value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static String GetString(this UInt64 value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String GetString(UInt64 value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static String GetString(this Single value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String GetString(this Single value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static String GetString(this Double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String GetString(this Double value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static String GetString(this Decimal value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String GetString(this Decimal value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static String GetString(this DateTime value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String GetString(this DateTime value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }
        
        public static String GetString(this String value)
        {
            return value;
        }
        
        public static String GetString(this String value, IFormatProvider provider)
        {
            return value;
        }

        public static String GetString(this IEnumerable source)
        {
            return GetString(source, CultureInfo.InvariantCulture);
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static String GetString(this IEnumerable source, IFormatProvider provider)
        {
            return source switch
            {
                null => null,
                String str => str,
                IDictionary dict => $"{{{String.Join(", ", dict.Keys.Cast<Object>().Select(key => $"{key.GetString(provider, true)} : {dict[key].GetString(provider, true)}"))}}}",
                IEnumerable<IEnumerable> jagged => $"[{String.Join(", ", jagged.Select(e => e.GetString(provider, true)))}]",
                _ => $"[{String.Join(", ", source.Cast<Object>().Select(item => item.GetString(provider, true)))}]"
            };
        }
        
        #endregion
        
        public static Boolean TryConvert<T>(this String input, out T value)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                value = (T) converter.ConvertFromString(input);

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
                null => false,
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

        public static String Text(this Byte[] data)
        {
            return data == null ? null : BitConverter.ToString(data).Replace("-", String.Empty);
        }

        public static T Clone<T>(this Object obj) where T : class
        {
            return obj switch
            {
                null => null,
                ICloneable cloneable => cloneable.Clone<T>(),
                _ => (T)obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(obj, null) ??
                     throw new CloneException($"{obj.GetType()} is not have MemberwiseClone method")
            };
        }

        public static T Clone<T>(this ICloneable cloneable)
        {
            if (cloneable.Clone() is T clone)
            {
                return clone;
            }

            throw new CloneException($"{cloneable.GetType()} is not {typeof(T)}");
        }
    }
}