// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Globalization;

namespace Common_Library.Utils.Math
{
    public static partial class MathUtils
    {
        public enum RoundType
        {
            Banking,
            AwayToZero,
            Floor,
            Ceil
        }

        public enum DisplayType
        {
            Value,
            ValueAndPercent,
            Percent
        }

        public static void Range(ref IConvertible value, Decimal minimum = Decimal.Zero, Decimal maximum = Decimal.MaxValue,
            Boolean looped = false)
        {
            value = Range(value, minimum, maximum, looped);
        }

        public static Decimal Range(IConvertible value, Decimal minimum = Decimal.Zero, Decimal maximum = Decimal.MaxValue,
            Boolean looped = false)
        {
            Decimal @decimal = Convert.ToDecimal(value);

            if (@decimal > maximum)
            {
                return looped ? minimum : maximum;
            }

            if (@decimal < minimum)
            {
                return looped ? maximum : minimum;
            }

            return @decimal;
        }

        public enum Position
        {
            None,
            Left,
            Right,
            LeftRight
        }

        public static Boolean IsPositive(Decimal value)
        {
            return value >= 0;
        }

        public static Boolean IsPowerOf2(UInt64 value)
        {
            return value > 0 && (value & (value - 1)) == 0;
        }

        public static Int32 GetDigitsAfterPoint<T>(T value)
        {
            String[] splitted = value.Convert(CultureInfo.InvariantCulture).Split('.');

            return splitted.Length <= 1 ? 0 : splitted[1].Length;
        }

        public static Int32 ZeroCheck(Int32 value, Int32 onZero = 1)
        {
            return value == 0 ? onZero : value;
        }

        private static Decimal RoundUp(Decimal number, Int32 digits)
        {
            return System.Math.Ceiling(number * (Decimal) System.Math.Pow(10, digits))
                   / (Decimal) System.Math.Pow(10, digits);
        }

        private static Double RoundUp(Double number, Int32 digits)
        {
            return (Double) RoundUp((Decimal) number, digits);
        }

        private static Decimal RoundDown(Decimal number, Int32 digits)
        {
            Decimal power = Convert.ToDecimal(System.Math.Pow(10, digits));
            return System.Math.Floor(number * power) / power;
        }

        private static Double RoundDown(Double number, Int32 digits)
        {
            return (Double) RoundDown((Decimal) number, digits);
        }

        public static Double Round(Double number, RoundType roundType)
        {
            return Round(number, 0, roundType);
        }

        public static Double Round(Double number, Int32 digits = 0, RoundType roundType = RoundType.Banking)
        {
            digits = ToRange(digits, 0, 15);
            return roundType switch
            {
                RoundType.Banking => System.Math.Round(number, digits, MidpointRounding.ToEven),
                RoundType.AwayToZero => System.Math.Round(number, digits, MidpointRounding.AwayFromZero),
                RoundType.Ceil => RoundUp(number, digits),
                RoundType.Floor => RoundDown(number, digits),
                _ => digits
            };
        }
    }
}