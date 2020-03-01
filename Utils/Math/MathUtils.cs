// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Globalization;
using System.Linq;

namespace Common_Library.Utils
{
    public static class MathUtils
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

        public static void Range(ref Int32 value, Int32 minimum = 0, Int32 maximum = Int32.MaxValue, Boolean looped = false)
        {
            value = Range(value, minimum, maximum, looped);
        }
        
        public static Int32 Range(Int32 value, Int32 minimum = 0, Int32 maximum = Int32.MaxValue, Boolean looped = false)
        {
            if (value > maximum)
            {
                return looped ? minimum : maximum;
            }
            
            if (value < minimum)
            {
                return looped ? maximum : minimum;
            }

            return value;
        }

        public enum Position
        {
            None,
            Left,
            Right,
            LeftRight
        }

        private static Boolean InRangeNone(Int32 value,
            Int32 minimum = 0, Int32 maximum = Int32.MaxValue)
        {
            return value > minimum && value < maximum;
        }
        
        private static Boolean InRangeLeft(Int32 value,
            Int32 minimum = 0, Int32 maximum = Int32.MaxValue)
        {
            return value >= minimum && value < maximum;
        }
        
        private static Boolean InRangeRight(Int32 value,
            Int32 minimum = 0, Int32 maximum = Int32.MaxValue)
        {
            return value > minimum && value <= maximum;
        }
        
        private static Boolean InRangeLeftRight(Int32 value,
            Int32 minimum = 0, Int32 maximum = Int32.MaxValue)
        {
            return value >= minimum && value <= maximum;
        }

        public static Boolean InRange(Int32 value, Position comparison = Position.LeftRight, Int32 minimum = 0, Int32 maximum = Int32.MaxValue)
        {
            return comparison switch
            {
                Position.None => InRangeNone(value, minimum, maximum),
                Position.Left => InRangeLeft(value, minimum, maximum),
                Position.Right => InRangeRight(value, minimum, maximum),
                Position.LeftRight => InRangeLeftRight(value, minimum, maximum),
                _ => false
            };
        }

        public static void Range(ref Single value, Single minimum = 0, Single maximum = Single.MaxValue, Boolean looped = false)
        {
            value = Range(value, minimum, maximum, looped);
        }
        
        public static Single Range(Single value, Single minimum = 0, Single maximum = Single.MaxValue, Boolean looped = false)
        {
            if (value > maximum)
            {
                return looped ? minimum : maximum;
            }
            
            if (value < minimum)
            {
                return looped ? maximum : minimum;
            }

            return value;
        }
        
        public static void Range(ref Double value, Double minimum = 0, Double maximum = Double.MaxValue, Boolean looped = false)
        {
            value = Range(value, minimum, maximum, looped);
        }
        
        public static Double Range(Double value, Double minimum = 0, Double maximum = Double.MaxValue, Boolean looped = false)
        {
            if (value > maximum)
            {
                return looped ? minimum : maximum;
            }
            
            if (value < minimum)
            {
                return looped ? maximum : minimum;
            }

            return value;
        }

        public static Int32 GetDigitsAfterPoint(Double value)
        {
            return value.ToString(CultureInfo.InvariantCulture).Split('.').Length > 1
                ? value.ToString(CultureInfo.InvariantCulture).Split('.').ToList().ElementAt(1).Length
                : 0;
        }
        
        public static Int32 ZeroCheck(Int32 value, Int32 zero = 1)
        {
            return value == 0 ? zero : value;
        }
        
        private static Decimal RoundUp(Decimal number, Int32 digits) {
            return Math.Ceiling(number * (Decimal)Math.Pow(10, digits))
                   / (Decimal)Math.Pow(10, digits);
        }

        private static Double RoundUp(Double number, Int32 digits)
        {
            return (Double)RoundUp((Decimal)number, digits);
        }

        private static Decimal RoundDown(Decimal number, Int32 digits)
        {
            Decimal power = Convert.ToDecimal(Math.Pow(10, digits));
            return Math.Floor(number * power) / power;
        }
        
        private static Double RoundDown(Double number, Int32 digits)
        {
            return (Double)RoundDown((Decimal)number, digits);
        }

        public static Double Round(Double number, RoundType roundType)
        {
            return Round(number, 0, roundType);
        }
        
        public static Double Round(Double number, Int32 digits = 0, RoundType roundType = RoundType.Banking)
        {
            digits = Range(digits, 0, 15);
            return roundType switch
            {
                RoundType.Banking => Math.Round(number, digits, MidpointRounding.ToEven),
                RoundType.AwayToZero => Math.Round(number, digits, MidpointRounding.AwayFromZero),
                RoundType.Ceil => RoundUp(number, digits),
                RoundType.Floor => RoundDown(number, digits),
                _ => digits
            };
        }
    }
}