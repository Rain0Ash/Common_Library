// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using Common_Library.Exceptions.Enum;
using Common_Library.Utils.Math;

namespace Common_Library.Utils
{
    public static class EnumUtils
    {
        public static Boolean TypeIsFlagsEnum<TEnum>() where TEnum : Enum
        {
            return TypeIsFlagsEnum(typeof(TEnum));
        }

        public static Boolean TypeIsFlagsEnum<TEnum>(TEnum value) where TEnum : Enum
        {
            return TypeIsFlagsEnum(value.GetType());
        }

        public static Boolean TypeIsFlagsEnum(Type type)
        {
            return type.IsEnum && Attribute.GetCustomAttribute(type, typeof(FlagsAttribute), true) != null;
        }

        public static TEnum RandomEnumValue<TEnum>() where TEnum : Enum
        {
            Type type = typeof(TEnum);
            Array values = Enum.GetValues(type);
            Object value = values.GetValue(RandomUtils.Next(0, values.Length));

            return (TEnum) Convert.ChangeType(value, type);
        }

        public static IEnumerable<Decimal> AsDecimal(this Enum @enum)
        {
            return AsDecimal(@enum.GetType());
        }

        public static IEnumerable<Decimal> AsDecimal(Type type)
        {
            if (!type.IsEnum)
            {
                throw new NotEnumTypeException(type.ToString());
            }

            return Enum.GetValues(type).ToDecimal();
        }

        public static IEnumerable<Decimal> AsDecimal<TEnum>() where TEnum : Enum
        {
            return AsDecimal(typeof(TEnum));
        }

        public static IEnumerable<UInt64> AsUInt64(this Enum @enum, Boolean ignoreNegative = true)
        {
            return AsUInt64(@enum.GetType(), ignoreNegative);
        }

        public static IEnumerable<UInt64> AsUInt64(Type type, Boolean ignoreNegative = true)
        {
            IEnumerable<Decimal> decimals = AsDecimal(type);

            if (ignoreNegative)
            {
                decimals = decimals.Where(MathUtils.IsPositive);
            }

            return decimals.Select(ConvertUtils.ToUInt64);
        }

        public static IEnumerable<UInt64> AsUInt64<TEnum>(Boolean ignoreNegative = true) where TEnum : Enum
        {
            return AsUInt64(typeof(TEnum), ignoreNegative);
        }

        public static Int32 GetCountOfFlags<TEnum>() where TEnum : Enum
        {
            Type type = typeof(TEnum);

            if (!TypeIsFlagsEnum(type))
            {
                throw new NotFlagsEnumTypeException<TEnum>();
            }

            UInt64[] values = AsUInt64(type).ToArray();

            return values.Length < 2 ? values.Length : values.Count(MathUtils.IsPowerOf2);
        }
    }
}