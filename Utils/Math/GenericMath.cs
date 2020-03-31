// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Common_Library.Utils.Math
{   
    public static partial class MathUtils
    {
		public static IEnumerable<SByte> Range(SByte stop)
		{
			return Range((SByte)0, stop);
		}

		public static IEnumerable<SByte> Range(SByte start, SByte stop, SByte step = 1)
		{
			for (SByte i = start; i < stop; i += step)
			{
				yield return i;
			}
		}

		public static IEnumerable<Byte> Range(Byte stop)
		{
			return Range((Byte)0, stop);
		}

		public static IEnumerable<Byte> Range(Byte start, Byte stop, Byte step = 1)
		{
			for (Byte i = start; i < stop; i += step)
			{
				yield return i;
			}
		}

		public static IEnumerable<Int16> Range(Int16 stop)
		{
			return Range((Int16)0, stop);
		}

		public static IEnumerable<Int16> Range(Int16 start, Int16 stop, Int16 step = 1)
		{
			for (Int16 i = start; i < stop; i += step)
			{
				yield return i;
			}
		}

		public static IEnumerable<UInt16> Range(UInt16 stop)
		{
			return Range((UInt16)0, stop);
		}

		public static IEnumerable<UInt16> Range(UInt16 start, UInt16 stop, UInt16 step = 1)
		{
			for (UInt16 i = start; i < stop; i += step)
			{
				yield return i;
			}
		}

		public static IEnumerable<Int32> Range(Int32 stop)
		{
			return Range(0, stop);
		}

		public static IEnumerable<Int32> Range(Int32 start, Int32 stop, Int32 step = 1)
		{
			for (Int32 i = start; i < stop; i += step)
			{
				yield return i;
			}
		}

		public static IEnumerable<UInt32> Range(UInt32 stop)
		{
			return Range(0, stop);
		}

		public static IEnumerable<UInt32> Range(UInt32 start, UInt32 stop, UInt32 step = 1)
		{
			for (UInt32 i = start; i < stop; i += step)
			{
				yield return i;
			}
		}

		public static IEnumerable<Int64> Range(Int64 stop)
		{
			return Range(0, stop);
		}

		public static IEnumerable<Int64> Range(Int64 start, Int64 stop, Int64 step = 1)
		{
			for (Int64 i = start; i < stop; i += step)
			{
				yield return i;
			}
		}

		public static IEnumerable<UInt64> Range(UInt64 stop)
		{
			return Range(0, stop);
		}

		public static IEnumerable<UInt64> Range(UInt64 start, UInt64 stop, UInt64 step = 1)
		{
			for (UInt64 i = start; i < stop; i += step)
			{
				yield return i;
			}
		}

		public static IEnumerable<Single> Range(Single stop)
		{
			return Range(0, stop);
		}

		public static IEnumerable<Single> Range(Single start, Single stop, Single step = 1)
		{
			for (Single i = start; i < stop; i += step)
			{
				yield return i;
			}
		}

		public static IEnumerable<Double> Range(Double stop)
		{
			return Range(0, stop);
		}

		public static IEnumerable<Double> Range(Double start, Double stop, Double step = 1)
		{
			for (Double i = start; i < stop; i += step)
			{
				yield return i;
			}
		}

		public static IEnumerable<Decimal> Range(Decimal stop)
		{
			return Range(0, stop);
		}

		public static IEnumerable<Decimal> Range(Decimal start, Decimal stop, Decimal step = 1)
		{
			for (Decimal i = start; i < stop; i += step)
			{
				yield return i;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref Char value, Char minimum = Char.MinValue, Char maximum = Char.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Char ToRange(Char value, Char minimum = Char.MinValue, Char maximum = Char.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref SByte value, SByte minimum = 0, SByte maximum = SByte.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SByte ToRange(SByte value, SByte minimum = 0, SByte maximum = SByte.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref Byte value, Byte minimum = 0, Byte maximum = Byte.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Byte ToRange(Byte value, Byte minimum = 0, Byte maximum = Byte.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref Int16 value, Int16 minimum = 0, Int16 maximum = Int16.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int16 ToRange(Int16 value, Int16 minimum = 0, Int16 maximum = Int16.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref UInt16 value, UInt16 minimum = 0, UInt16 maximum = UInt16.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UInt16 ToRange(UInt16 value, UInt16 minimum = 0, UInt16 maximum = UInt16.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref Int32 value, Int32 minimum = 0, Int32 maximum = Int32.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int32 ToRange(Int32 value, Int32 minimum = 0, Int32 maximum = Int32.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref UInt32 value, UInt32 minimum = 0, UInt32 maximum = UInt32.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UInt32 ToRange(UInt32 value, UInt32 minimum = 0, UInt32 maximum = UInt32.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref Int64 value, Int64 minimum = 0, Int64 maximum = Int64.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int64 ToRange(Int64 value, Int64 minimum = 0, Int64 maximum = Int64.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref UInt64 value, UInt64 minimum = 0, UInt64 maximum = UInt64.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UInt64 ToRange(UInt64 value, UInt64 minimum = 0, UInt64 maximum = UInt64.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref Single value, Single minimum = 0, Single maximum = Single.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Single ToRange(Single value, Single minimum = 0, Single maximum = Single.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref Double value, Double minimum = 0, Double maximum = Double.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Double ToRange(Double value, Double minimum = 0, Double maximum = Double.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToRange(ref Decimal value, Decimal minimum = 0, Decimal maximum = Decimal.MaxValue, Boolean looped = false)
		{
			 value = ToRange(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Decimal ToRange(Decimal value, Decimal minimum = 0, Decimal maximum = Decimal.MaxValue, Boolean looped = false)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(Char value, Position comparison = Position.LeftRight, Char minimum = Char.MinValue, Char maximum = Char.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(SByte value, Position comparison = Position.LeftRight, SByte minimum = 0, SByte maximum = SByte.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(Byte value, Position comparison = Position.LeftRight, Byte minimum = 0, Byte maximum = Byte.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(Int16 value, Position comparison = Position.LeftRight, Int16 minimum = 0, Int16 maximum = Int16.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(UInt16 value, Position comparison = Position.LeftRight, UInt16 minimum = 0, UInt16 maximum = UInt16.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(Int32 value, Position comparison = Position.LeftRight, Int32 minimum = 0, Int32 maximum = Int32.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(UInt32 value, Position comparison = Position.LeftRight, UInt32 minimum = 0, UInt32 maximum = UInt32.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(Int64 value, Position comparison = Position.LeftRight, Int64 minimum = 0, Int64 maximum = Int64.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(UInt64 value, Position comparison = Position.LeftRight, UInt64 minimum = 0, UInt64 maximum = UInt64.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(Single value, Position comparison = Position.LeftRight, Single minimum = 0, Single maximum = Single.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(Double value, Position comparison = Position.LeftRight, Double minimum = 0, Double maximum = Double.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean InRange(Decimal value, Position comparison = Position.LeftRight, Decimal minimum = 0, Decimal maximum = Decimal.MaxValue)
		{
			return comparison switch
			{
				Position.None => value > minimum && value < maximum,
				Position.Left => value >= minimum && value < maximum,
				Position.Right => value > minimum && value <= maximum,
				Position.LeftRight => value >= minimum && value <= maximum,
				_ => throw new NotSupportedException(comparison.ToString())
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Char Abs(this Char value)
		{
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SByte Abs(this SByte value)
		{
			if (value >= 0)
			{
				return value;
			}

			Int32 val = -value;
			return Unsafe.As<Int32, SByte>(ref val);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Byte Abs(this Byte value)
		{
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int16 Abs(this Int16 value)
		{
			if (value >= 0)
			{
				return value;
			}

			Int32 val = -value;
			return Unsafe.As<Int32, Int16>(ref val);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UInt16 Abs(this UInt16 value)
		{
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int32 Abs(this Int32 value)
		{
			return value >= 0 ? value : -value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UInt32 Abs(this UInt32 value)
		{
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int64 Abs(this Int64 value)
		{
			return value >= 0 ? value : -value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UInt64 Abs(this UInt64 value)
		{
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Single Abs(this Single value)
		{
			return value >= 0 ? value : -value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Double Abs(this Double value)
		{
			return value >= 0 ? value : -value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Decimal Abs(this Decimal value)
		{
			return value >= 0 ? value : -value;
		}
    }

    [SuppressMessage("ReSharper", "InvertIf")]
    public static class MathUnsafe
    {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Add<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				Char value = (Char)(Unsafe.As<T, Char>(ref left) + Unsafe.As<T, Char>(ref right));
				return Unsafe.As<Char, T>(ref value);
			}

			if (typeof(T) == typeof(SByte))
			{
				SByte value = (SByte)(Unsafe.As<T, SByte>(ref left) + Unsafe.As<T, SByte>(ref right));
				return Unsafe.As<SByte, T>(ref value);
			}

			if (typeof(T) == typeof(Byte))
			{
				Byte value = (Byte)(Unsafe.As<T, Byte>(ref left) + Unsafe.As<T, Byte>(ref right));
				return Unsafe.As<Byte, T>(ref value);
			}

			if (typeof(T) == typeof(Int16))
			{
				Int16 value = (Int16)(Unsafe.As<T, Int16>(ref left) + Unsafe.As<T, Int16>(ref right));
				return Unsafe.As<Int16, T>(ref value);
			}

			if (typeof(T) == typeof(UInt16))
			{
				UInt16 value = (UInt16)(Unsafe.As<T, UInt16>(ref left) + Unsafe.As<T, UInt16>(ref right));
				return Unsafe.As<UInt16, T>(ref value);
			}

			if (typeof(T) == typeof(Int32))
			{
				Int32 value = Unsafe.As<T, Int32>(ref left) + Unsafe.As<T, Int32>(ref right);
				return Unsafe.As<Int32, T>(ref value);
			}

			if (typeof(T) == typeof(UInt32))
			{
				UInt32 value = Unsafe.As<T, UInt32>(ref left) + Unsafe.As<T, UInt32>(ref right);
				return Unsafe.As<UInt32, T>(ref value);
			}

			if (typeof(T) == typeof(Int64))
			{
				Int64 value = Unsafe.As<T, Int64>(ref left) + Unsafe.As<T, Int64>(ref right);
				return Unsafe.As<Int64, T>(ref value);
			}

			if (typeof(T) == typeof(UInt64))
			{
				UInt64 value = Unsafe.As<T, UInt64>(ref left) + Unsafe.As<T, UInt64>(ref right);
				return Unsafe.As<UInt64, T>(ref value);
			}

			if (typeof(T) == typeof(Single))
			{
				Single value = Unsafe.As<T, Single>(ref left) + Unsafe.As<T, Single>(ref right);
				return Unsafe.As<Single, T>(ref value);
			}

			if (typeof(T) == typeof(Double))
			{
				Double value = Unsafe.As<T, Double>(ref left) + Unsafe.As<T, Double>(ref right);
				return Unsafe.As<Double, T>(ref value);
			}

			if (typeof(T) == typeof(Decimal))
			{
				Decimal value = Unsafe.As<T, Decimal>(ref left) + Unsafe.As<T, Decimal>(ref right);
				return Unsafe.As<Decimal, T>(ref value);
			}

			throw new NotSupportedException($"Operator + is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Substract<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				Char value = (Char)(Unsafe.As<T, Char>(ref left) - Unsafe.As<T, Char>(ref right));
				return Unsafe.As<Char, T>(ref value);
			}

			if (typeof(T) == typeof(SByte))
			{
				SByte value = (SByte)(Unsafe.As<T, SByte>(ref left) - Unsafe.As<T, SByte>(ref right));
				return Unsafe.As<SByte, T>(ref value);
			}

			if (typeof(T) == typeof(Byte))
			{
				Byte value = (Byte)(Unsafe.As<T, Byte>(ref left) - Unsafe.As<T, Byte>(ref right));
				return Unsafe.As<Byte, T>(ref value);
			}

			if (typeof(T) == typeof(Int16))
			{
				Int16 value = (Int16)(Unsafe.As<T, Int16>(ref left) - Unsafe.As<T, Int16>(ref right));
				return Unsafe.As<Int16, T>(ref value);
			}

			if (typeof(T) == typeof(UInt16))
			{
				UInt16 value = (UInt16)(Unsafe.As<T, UInt16>(ref left) - Unsafe.As<T, UInt16>(ref right));
				return Unsafe.As<UInt16, T>(ref value);
			}

			if (typeof(T) == typeof(Int32))
			{
				Int32 value = Unsafe.As<T, Int32>(ref left) - Unsafe.As<T, Int32>(ref right);
				return Unsafe.As<Int32, T>(ref value);
			}

			if (typeof(T) == typeof(UInt32))
			{
				UInt32 value = Unsafe.As<T, UInt32>(ref left) - Unsafe.As<T, UInt32>(ref right);
				return Unsafe.As<UInt32, T>(ref value);
			}

			if (typeof(T) == typeof(Int64))
			{
				Int64 value = Unsafe.As<T, Int64>(ref left) - Unsafe.As<T, Int64>(ref right);
				return Unsafe.As<Int64, T>(ref value);
			}

			if (typeof(T) == typeof(UInt64))
			{
				UInt64 value = Unsafe.As<T, UInt64>(ref left) - Unsafe.As<T, UInt64>(ref right);
				return Unsafe.As<UInt64, T>(ref value);
			}

			if (typeof(T) == typeof(Single))
			{
				Single value = Unsafe.As<T, Single>(ref left) - Unsafe.As<T, Single>(ref right);
				return Unsafe.As<Single, T>(ref value);
			}

			if (typeof(T) == typeof(Double))
			{
				Double value = Unsafe.As<T, Double>(ref left) - Unsafe.As<T, Double>(ref right);
				return Unsafe.As<Double, T>(ref value);
			}

			if (typeof(T) == typeof(Decimal))
			{
				Decimal value = Unsafe.As<T, Decimal>(ref left) - Unsafe.As<T, Decimal>(ref right);
				return Unsafe.As<Decimal, T>(ref value);
			}

			throw new NotSupportedException($"Operator - is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Multiply<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				Char value = (Char)(Unsafe.As<T, Char>(ref left) * Unsafe.As<T, Char>(ref right));
				return Unsafe.As<Char, T>(ref value);
			}

			if (typeof(T) == typeof(SByte))
			{
				SByte value = (SByte)(Unsafe.As<T, SByte>(ref left) * Unsafe.As<T, SByte>(ref right));
				return Unsafe.As<SByte, T>(ref value);
			}

			if (typeof(T) == typeof(Byte))
			{
				Byte value = (Byte)(Unsafe.As<T, Byte>(ref left) * Unsafe.As<T, Byte>(ref right));
				return Unsafe.As<Byte, T>(ref value);
			}

			if (typeof(T) == typeof(Int16))
			{
				Int16 value = (Int16)(Unsafe.As<T, Int16>(ref left) * Unsafe.As<T, Int16>(ref right));
				return Unsafe.As<Int16, T>(ref value);
			}

			if (typeof(T) == typeof(UInt16))
			{
				UInt16 value = (UInt16)(Unsafe.As<T, UInt16>(ref left) * Unsafe.As<T, UInt16>(ref right));
				return Unsafe.As<UInt16, T>(ref value);
			}

			if (typeof(T) == typeof(Int32))
			{
				Int32 value = Unsafe.As<T, Int32>(ref left) * Unsafe.As<T, Int32>(ref right);
				return Unsafe.As<Int32, T>(ref value);
			}

			if (typeof(T) == typeof(UInt32))
			{
				UInt32 value = Unsafe.As<T, UInt32>(ref left) * Unsafe.As<T, UInt32>(ref right);
				return Unsafe.As<UInt32, T>(ref value);
			}

			if (typeof(T) == typeof(Int64))
			{
				Int64 value = Unsafe.As<T, Int64>(ref left) * Unsafe.As<T, Int64>(ref right);
				return Unsafe.As<Int64, T>(ref value);
			}

			if (typeof(T) == typeof(UInt64))
			{
				UInt64 value = Unsafe.As<T, UInt64>(ref left) * Unsafe.As<T, UInt64>(ref right);
				return Unsafe.As<UInt64, T>(ref value);
			}

			if (typeof(T) == typeof(Single))
			{
				Single value = Unsafe.As<T, Single>(ref left) * Unsafe.As<T, Single>(ref right);
				return Unsafe.As<Single, T>(ref value);
			}

			if (typeof(T) == typeof(Double))
			{
				Double value = Unsafe.As<T, Double>(ref left) * Unsafe.As<T, Double>(ref right);
				return Unsafe.As<Double, T>(ref value);
			}

			if (typeof(T) == typeof(Decimal))
			{
				Decimal value = Unsafe.As<T, Decimal>(ref left) * Unsafe.As<T, Decimal>(ref right);
				return Unsafe.As<Decimal, T>(ref value);
			}

			throw new NotSupportedException($"Operator * is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Divide<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				Char value = (Char)(Unsafe.As<T, Char>(ref left) / Unsafe.As<T, Char>(ref right));
				return Unsafe.As<Char, T>(ref value);
			}

			if (typeof(T) == typeof(SByte))
			{
				SByte value = (SByte)(Unsafe.As<T, SByte>(ref left) / Unsafe.As<T, SByte>(ref right));
				return Unsafe.As<SByte, T>(ref value);
			}

			if (typeof(T) == typeof(Byte))
			{
				Byte value = (Byte)(Unsafe.As<T, Byte>(ref left) / Unsafe.As<T, Byte>(ref right));
				return Unsafe.As<Byte, T>(ref value);
			}

			if (typeof(T) == typeof(Int16))
			{
				Int16 value = (Int16)(Unsafe.As<T, Int16>(ref left) / Unsafe.As<T, Int16>(ref right));
				return Unsafe.As<Int16, T>(ref value);
			}

			if (typeof(T) == typeof(UInt16))
			{
				UInt16 value = (UInt16)(Unsafe.As<T, UInt16>(ref left) / Unsafe.As<T, UInt16>(ref right));
				return Unsafe.As<UInt16, T>(ref value);
			}

			if (typeof(T) == typeof(Int32))
			{
				Int32 value = Unsafe.As<T, Int32>(ref left) / Unsafe.As<T, Int32>(ref right);
				return Unsafe.As<Int32, T>(ref value);
			}

			if (typeof(T) == typeof(UInt32))
			{
				UInt32 value = Unsafe.As<T, UInt32>(ref left) / Unsafe.As<T, UInt32>(ref right);
				return Unsafe.As<UInt32, T>(ref value);
			}

			if (typeof(T) == typeof(Int64))
			{
				Int64 value = Unsafe.As<T, Int64>(ref left) / Unsafe.As<T, Int64>(ref right);
				return Unsafe.As<Int64, T>(ref value);
			}

			if (typeof(T) == typeof(UInt64))
			{
				UInt64 value = Unsafe.As<T, UInt64>(ref left) / Unsafe.As<T, UInt64>(ref right);
				return Unsafe.As<UInt64, T>(ref value);
			}

			if (typeof(T) == typeof(Single))
			{
				Single value = Unsafe.As<T, Single>(ref left) / Unsafe.As<T, Single>(ref right);
				return Unsafe.As<Single, T>(ref value);
			}

			if (typeof(T) == typeof(Double))
			{
				Double value = Unsafe.As<T, Double>(ref left) / Unsafe.As<T, Double>(ref right);
				return Unsafe.As<Double, T>(ref value);
			}

			if (typeof(T) == typeof(Decimal))
			{
				Decimal value = Unsafe.As<T, Decimal>(ref left) / Unsafe.As<T, Decimal>(ref right);
				return Unsafe.As<Decimal, T>(ref value);
			}

			throw new NotSupportedException($"Operator / is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Modulo<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				Char value = (Char)(Unsafe.As<T, Char>(ref left) % Unsafe.As<T, Char>(ref right));
				return Unsafe.As<Char, T>(ref value);
			}

			if (typeof(T) == typeof(SByte))
			{
				SByte value = (SByte)(Unsafe.As<T, SByte>(ref left) % Unsafe.As<T, SByte>(ref right));
				return Unsafe.As<SByte, T>(ref value);
			}

			if (typeof(T) == typeof(Byte))
			{
				Byte value = (Byte)(Unsafe.As<T, Byte>(ref left) % Unsafe.As<T, Byte>(ref right));
				return Unsafe.As<Byte, T>(ref value);
			}

			if (typeof(T) == typeof(Int16))
			{
				Int16 value = (Int16)(Unsafe.As<T, Int16>(ref left) % Unsafe.As<T, Int16>(ref right));
				return Unsafe.As<Int16, T>(ref value);
			}

			if (typeof(T) == typeof(UInt16))
			{
				UInt16 value = (UInt16)(Unsafe.As<T, UInt16>(ref left) % Unsafe.As<T, UInt16>(ref right));
				return Unsafe.As<UInt16, T>(ref value);
			}

			if (typeof(T) == typeof(Int32))
			{
				Int32 value = Unsafe.As<T, Int32>(ref left) % Unsafe.As<T, Int32>(ref right);
				return Unsafe.As<Int32, T>(ref value);
			}

			if (typeof(T) == typeof(UInt32))
			{
				UInt32 value = Unsafe.As<T, UInt32>(ref left) % Unsafe.As<T, UInt32>(ref right);
				return Unsafe.As<UInt32, T>(ref value);
			}

			if (typeof(T) == typeof(Int64))
			{
				Int64 value = Unsafe.As<T, Int64>(ref left) % Unsafe.As<T, Int64>(ref right);
				return Unsafe.As<Int64, T>(ref value);
			}

			if (typeof(T) == typeof(UInt64))
			{
				UInt64 value = Unsafe.As<T, UInt64>(ref left) % Unsafe.As<T, UInt64>(ref right);
				return Unsafe.As<UInt64, T>(ref value);
			}

			if (typeof(T) == typeof(Single))
			{
				Single value = Unsafe.As<T, Single>(ref left) % Unsafe.As<T, Single>(ref right);
				return Unsafe.As<Single, T>(ref value);
			}

			if (typeof(T) == typeof(Double))
			{
				Double value = Unsafe.As<T, Double>(ref left) % Unsafe.As<T, Double>(ref right);
				return Unsafe.As<Double, T>(ref value);
			}

			if (typeof(T) == typeof(Decimal))
			{
				Decimal value = Unsafe.As<T, Decimal>(ref left) % Unsafe.As<T, Decimal>(ref right);
				return Unsafe.As<Decimal, T>(ref value);
			}

			throw new NotSupportedException($"Operator % is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T And<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				Char value = (Char)(Unsafe.As<T, Char>(ref left) & Unsafe.As<T, Char>(ref right));
				return Unsafe.As<Char, T>(ref value);
			}

			if (typeof(T) == typeof(SByte))
			{
				SByte value = (SByte)(Unsafe.As<T, SByte>(ref left) & Unsafe.As<T, SByte>(ref right));
				return Unsafe.As<SByte, T>(ref value);
			}

			if (typeof(T) == typeof(Byte))
			{
				Byte value = (Byte)(Unsafe.As<T, Byte>(ref left) & Unsafe.As<T, Byte>(ref right));
				return Unsafe.As<Byte, T>(ref value);
			}

			if (typeof(T) == typeof(Int16))
			{
				Int16 value = (Int16)(Unsafe.As<T, Int16>(ref left) & Unsafe.As<T, Int16>(ref right));
				return Unsafe.As<Int16, T>(ref value);
			}

			if (typeof(T) == typeof(UInt16))
			{
				UInt16 value = (UInt16)(Unsafe.As<T, UInt16>(ref left) & Unsafe.As<T, UInt16>(ref right));
				return Unsafe.As<UInt16, T>(ref value);
			}

			if (typeof(T) == typeof(Int32))
			{
				Int32 value = Unsafe.As<T, Int32>(ref left) & Unsafe.As<T, Int32>(ref right);
				return Unsafe.As<Int32, T>(ref value);
			}

			if (typeof(T) == typeof(UInt32))
			{
				UInt32 value = Unsafe.As<T, UInt32>(ref left) & Unsafe.As<T, UInt32>(ref right);
				return Unsafe.As<UInt32, T>(ref value);
			}

			if (typeof(T) == typeof(Int64))
			{
				Int64 value = Unsafe.As<T, Int64>(ref left) & Unsafe.As<T, Int64>(ref right);
				return Unsafe.As<Int64, T>(ref value);
			}

			if (typeof(T) == typeof(UInt64))
			{
				UInt64 value = Unsafe.As<T, UInt64>(ref left) & Unsafe.As<T, UInt64>(ref right);
				return Unsafe.As<UInt64, T>(ref value);
			}

			throw new NotSupportedException($"Operator & is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Or<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				Char value = (Char)(Unsafe.As<T, Char>(ref left) | Unsafe.As<T, Char>(ref right));
				return Unsafe.As<Char, T>(ref value);
			}

			if (typeof(T) == typeof(SByte))
			{
				SByte value = (SByte)(Unsafe.As<T, SByte>(ref left) | Unsafe.As<T, SByte>(ref right));
				return Unsafe.As<SByte, T>(ref value);
			}

			if (typeof(T) == typeof(Byte))
			{
				Byte value = (Byte)(Unsafe.As<T, Byte>(ref left) | Unsafe.As<T, Byte>(ref right));
				return Unsafe.As<Byte, T>(ref value);
			}

			if (typeof(T) == typeof(Int16))
			{
				Int16 value = (Int16)(Unsafe.As<T, Int16>(ref left) | Unsafe.As<T, Int16>(ref right));
				return Unsafe.As<Int16, T>(ref value);
			}

			if (typeof(T) == typeof(UInt16))
			{
				UInt16 value = (UInt16)(Unsafe.As<T, UInt16>(ref left) | Unsafe.As<T, UInt16>(ref right));
				return Unsafe.As<UInt16, T>(ref value);
			}

			if (typeof(T) == typeof(Int32))
			{
				Int32 value = Unsafe.As<T, Int32>(ref left) | Unsafe.As<T, Int32>(ref right);
				return Unsafe.As<Int32, T>(ref value);
			}

			if (typeof(T) == typeof(UInt32))
			{
				UInt32 value = Unsafe.As<T, UInt32>(ref left) | Unsafe.As<T, UInt32>(ref right);
				return Unsafe.As<UInt32, T>(ref value);
			}

			if (typeof(T) == typeof(Int64))
			{
				Int64 value = Unsafe.As<T, Int64>(ref left) | Unsafe.As<T, Int64>(ref right);
				return Unsafe.As<Int64, T>(ref value);
			}

			if (typeof(T) == typeof(UInt64))
			{
				UInt64 value = Unsafe.As<T, UInt64>(ref left) | Unsafe.As<T, UInt64>(ref right);
				return Unsafe.As<UInt64, T>(ref value);
			}

			throw new NotSupportedException($"Operator | is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Xor<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				Char value = (Char)(Unsafe.As<T, Char>(ref left) ^ Unsafe.As<T, Char>(ref right));
				return Unsafe.As<Char, T>(ref value);
			}

			if (typeof(T) == typeof(SByte))
			{
				SByte value = (SByte)(Unsafe.As<T, SByte>(ref left) ^ Unsafe.As<T, SByte>(ref right));
				return Unsafe.As<SByte, T>(ref value);
			}

			if (typeof(T) == typeof(Byte))
			{
				Byte value = (Byte)(Unsafe.As<T, Byte>(ref left) ^ Unsafe.As<T, Byte>(ref right));
				return Unsafe.As<Byte, T>(ref value);
			}

			if (typeof(T) == typeof(Int16))
			{
				Int16 value = (Int16)(Unsafe.As<T, Int16>(ref left) ^ Unsafe.As<T, Int16>(ref right));
				return Unsafe.As<Int16, T>(ref value);
			}

			if (typeof(T) == typeof(UInt16))
			{
				UInt16 value = (UInt16)(Unsafe.As<T, UInt16>(ref left) ^ Unsafe.As<T, UInt16>(ref right));
				return Unsafe.As<UInt16, T>(ref value);
			}

			if (typeof(T) == typeof(Int32))
			{
				Int32 value = Unsafe.As<T, Int32>(ref left) ^ Unsafe.As<T, Int32>(ref right);
				return Unsafe.As<Int32, T>(ref value);
			}

			if (typeof(T) == typeof(UInt32))
			{
				UInt32 value = Unsafe.As<T, UInt32>(ref left) ^ Unsafe.As<T, UInt32>(ref right);
				return Unsafe.As<UInt32, T>(ref value);
			}

			if (typeof(T) == typeof(Int64))
			{
				Int64 value = Unsafe.As<T, Int64>(ref left) ^ Unsafe.As<T, Int64>(ref right);
				return Unsafe.As<Int64, T>(ref value);
			}

			if (typeof(T) == typeof(UInt64))
			{
				UInt64 value = Unsafe.As<T, UInt64>(ref left) ^ Unsafe.As<T, UInt64>(ref right);
				return Unsafe.As<UInt64, T>(ref value);
			}

			throw new NotSupportedException($"Operator ^ is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Invert<T>(T value) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				Int32 i = ~Unsafe.As<T, Char>(ref value);
				Char val = Unsafe.As<Int32, Char>(ref i);
				return Unsafe.As<Char, T>(ref val);
			}

			if (typeof(T) == typeof(SByte))
			{
				Int32 i = ~Unsafe.As<T, SByte>(ref value);
				SByte val = Unsafe.As<Int32, SByte>(ref i);
				return Unsafe.As<SByte, T>(ref val);
			}

			if (typeof(T) == typeof(Byte))
			{
				Int32 i = ~Unsafe.As<T, Byte>(ref value);
				Byte val = Unsafe.As<Int32, Byte>(ref i);
				return Unsafe.As<Byte, T>(ref val);
			}

			if (typeof(T) == typeof(Int16))
			{
				Int32 i = ~Unsafe.As<T, Int16>(ref value);
				Int16 val = Unsafe.As<Int32, Int16>(ref i);
				return Unsafe.As<Int16, T>(ref val);
			}

			if (typeof(T) == typeof(UInt16))
			{
				Int32 i = ~Unsafe.As<T, UInt16>(ref value);
				UInt16 val = Unsafe.As<Int32, UInt16>(ref i);
				return Unsafe.As<UInt16, T>(ref val);
			}

			if (typeof(T) == typeof(Int32))
			{
				Int32 val = ~Unsafe.As<T, Int32>(ref value);
				return Unsafe.As<Int32, T>(ref val);
			}

			if (typeof(T) == typeof(UInt32))
			{
				UInt32 val = ~Unsafe.As<T, UInt32>(ref value);
				return Unsafe.As<UInt32, T>(ref val);
			}

			if (typeof(T) == typeof(Int64))
			{
				Int64 val = ~Unsafe.As<T, Int64>(ref value);
				return Unsafe.As<Int64, T>(ref val);
			}

			if (typeof(T) == typeof(UInt64))
			{
				UInt64 val = ~Unsafe.As<T, UInt64>(ref value);
				return Unsafe.As<UInt64, T>(ref val);
			}

			throw new NotSupportedException($"Operator ~ is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Abs<T>(T value) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(SByte))
			{
				SByte val = System.Math.Abs(Unsafe.As<T, SByte>(ref value));
				return Unsafe.As<SByte, T>(ref val);
			}

			if (typeof(T) == typeof(Int16))
			{
				Int16 val = System.Math.Abs(Unsafe.As<T, Int16>(ref value));
				return Unsafe.As<Int16, T>(ref val);
			}

			if (typeof(T) == typeof(Int32))
			{
				Int32 val = System.Math.Abs(Unsafe.As<T, Int32>(ref value));
				return Unsafe.As<Int32, T>(ref val);
			}

			if (typeof(T) == typeof(Int64))
			{
				Int64 val = System.Math.Abs(Unsafe.As<T, Int64>(ref value));
				return Unsafe.As<Int64, T>(ref val);
			}

			if (typeof(T) == typeof(Single))
			{
				Single val = System.Math.Abs(Unsafe.As<T, Single>(ref value));
				return Unsafe.As<Single, T>(ref val);
			}

			if (typeof(T) == typeof(Double))
			{
				Double val = System.Math.Abs(Unsafe.As<T, Double>(ref value));
				return Unsafe.As<Double, T>(ref val);
			}

			if (typeof(T) == typeof(Decimal))
			{
				Decimal val = System.Math.Abs(Unsafe.As<T, Decimal>(ref value));
				return Unsafe.As<Decimal, T>(ref val);
			}

			throw new NotSupportedException($"Operator | | is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Negative<T>(T value) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(SByte))
			{
				Int32 i = -System.Math.Abs(Unsafe.As<T, SByte>(ref value));
				SByte val = Unsafe.As<Int32, SByte>(ref i);
				return Unsafe.As<SByte, T>(ref val);
			}

			if (typeof(T) == typeof(Int16))
			{
				Int32 i = -System.Math.Abs(Unsafe.As<T, Int16>(ref value));
				Int16 val = Unsafe.As<Int32, Int16>(ref i);
				return Unsafe.As<Int16, T>(ref val);
			}

			if (typeof(T) == typeof(Int32))
			{
				Int32 val = -System.Math.Abs(Unsafe.As<T, Int32>(ref value));
				return Unsafe.As<Int32, T>(ref val);
			}

			if (typeof(T) == typeof(Int64))
			{
				Int64 val = -System.Math.Abs(Unsafe.As<T, Int64>(ref value));
				return Unsafe.As<Int64, T>(ref val);
			}

			if (typeof(T) == typeof(Single))
			{
				Single val = -System.Math.Abs(Unsafe.As<T, Single>(ref value));
				return Unsafe.As<Single, T>(ref val);
			}

			if (typeof(T) == typeof(Double))
			{
				Double val = -System.Math.Abs(Unsafe.As<T, Double>(ref value));
				return Unsafe.As<Double, T>(ref val);
			}

			if (typeof(T) == typeof(Decimal))
			{
				Decimal val = -System.Math.Abs(Unsafe.As<T, Decimal>(ref value));
				return Unsafe.As<Decimal, T>(ref val);
			}

			throw new NotSupportedException($"Operator -| | is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean Equal<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				return Unsafe.As<T, Char>(ref left) == Unsafe.As<T, Char>(ref right);
			}

			if (typeof(T) == typeof(SByte))
			{
				return Unsafe.As<T, SByte>(ref left) == Unsafe.As<T, SByte>(ref right);
			}

			if (typeof(T) == typeof(Byte))
			{
				return Unsafe.As<T, Byte>(ref left) == Unsafe.As<T, Byte>(ref right);
			}

			if (typeof(T) == typeof(Int16))
			{
				return Unsafe.As<T, Int16>(ref left) == Unsafe.As<T, Int16>(ref right);
			}

			if (typeof(T) == typeof(UInt16))
			{
				return Unsafe.As<T, UInt16>(ref left) == Unsafe.As<T, UInt16>(ref right);
			}

			if (typeof(T) == typeof(Int32))
			{
				return Unsafe.As<T, Int32>(ref left) == Unsafe.As<T, Int32>(ref right);
			}

			if (typeof(T) == typeof(UInt32))
			{
				return Unsafe.As<T, UInt32>(ref left) == Unsafe.As<T, UInt32>(ref right);
			}

			if (typeof(T) == typeof(Int64))
			{
				return Unsafe.As<T, Int64>(ref left) == Unsafe.As<T, Int64>(ref right);
			}

			if (typeof(T) == typeof(UInt64))
			{
				return Unsafe.As<T, UInt64>(ref left) == Unsafe.As<T, UInt64>(ref right);
			}

			if (typeof(T) == typeof(Single))
			{
				return Unsafe.As<T, Single>(ref left) - Unsafe.As<T, Single>(ref right) < Single.Epsilon;
			}

			if (typeof(T) == typeof(Double))
			{
				return Unsafe.As<T, Double>(ref left) - Unsafe.As<T, Double>(ref right) < Double.Epsilon;
			}

			if (typeof(T) == typeof(Decimal))
			{
				return Unsafe.As<T, Decimal>(ref left) == Unsafe.As<T, Decimal>(ref right);
			}

			throw new NotSupportedException($"Operator == is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean NotEqual<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				return Unsafe.As<T, Char>(ref left) != Unsafe.As<T, Char>(ref right);
			}

			if (typeof(T) == typeof(SByte))
			{
				return Unsafe.As<T, SByte>(ref left) != Unsafe.As<T, SByte>(ref right);
			}

			if (typeof(T) == typeof(Byte))
			{
				return Unsafe.As<T, Byte>(ref left) != Unsafe.As<T, Byte>(ref right);
			}

			if (typeof(T) == typeof(Int16))
			{
				return Unsafe.As<T, Int16>(ref left) != Unsafe.As<T, Int16>(ref right);
			}

			if (typeof(T) == typeof(UInt16))
			{
				return Unsafe.As<T, UInt16>(ref left) != Unsafe.As<T, UInt16>(ref right);
			}

			if (typeof(T) == typeof(Int32))
			{
				return Unsafe.As<T, Int32>(ref left) != Unsafe.As<T, Int32>(ref right);
			}

			if (typeof(T) == typeof(UInt32))
			{
				return Unsafe.As<T, UInt32>(ref left) != Unsafe.As<T, UInt32>(ref right);
			}

			if (typeof(T) == typeof(Int64))
			{
				return Unsafe.As<T, Int64>(ref left) != Unsafe.As<T, Int64>(ref right);
			}

			if (typeof(T) == typeof(UInt64))
			{
				return Unsafe.As<T, UInt64>(ref left) != Unsafe.As<T, UInt64>(ref right);
			}

			if (typeof(T) == typeof(Single))
			{
				return Unsafe.As<T, Single>(ref left) - Unsafe.As<T, Single>(ref right) > Single.Epsilon;
			}

			if (typeof(T) == typeof(Double))
			{
				return Unsafe.As<T, Double>(ref left) - Unsafe.As<T, Double>(ref right) > Double.Epsilon;
			}

			if (typeof(T) == typeof(Decimal))
			{
				return Unsafe.As<T, Decimal>(ref left) != Unsafe.As<T, Decimal>(ref right);
			}

			throw new NotSupportedException($"Operator != is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean Greater<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				return Unsafe.As<T, Char>(ref left) > Unsafe.As<T, Char>(ref right);
			}

			if (typeof(T) == typeof(SByte))
			{
				return Unsafe.As<T, SByte>(ref left) > Unsafe.As<T, SByte>(ref right);
			}

			if (typeof(T) == typeof(Byte))
			{
				return Unsafe.As<T, Byte>(ref left) > Unsafe.As<T, Byte>(ref right);
			}

			if (typeof(T) == typeof(Int16))
			{
				return Unsafe.As<T, Int16>(ref left) > Unsafe.As<T, Int16>(ref right);
			}

			if (typeof(T) == typeof(UInt16))
			{
				return Unsafe.As<T, UInt16>(ref left) > Unsafe.As<T, UInt16>(ref right);
			}

			if (typeof(T) == typeof(Int32))
			{
				return Unsafe.As<T, Int32>(ref left) > Unsafe.As<T, Int32>(ref right);
			}

			if (typeof(T) == typeof(UInt32))
			{
				return Unsafe.As<T, UInt32>(ref left) > Unsafe.As<T, UInt32>(ref right);
			}

			if (typeof(T) == typeof(Int64))
			{
				return Unsafe.As<T, Int64>(ref left) > Unsafe.As<T, Int64>(ref right);
			}

			if (typeof(T) == typeof(UInt64))
			{
				return Unsafe.As<T, UInt64>(ref left) > Unsafe.As<T, UInt64>(ref right);
			}

			if (typeof(T) == typeof(Single))
			{
				return Unsafe.As<T, Single>(ref left) > Unsafe.As<T, Single>(ref right);
			}

			if (typeof(T) == typeof(Double))
			{
				return Unsafe.As<T, Double>(ref left) > Unsafe.As<T, Double>(ref right);
			}

			if (typeof(T) == typeof(Decimal))
			{
				return Unsafe.As<T, Decimal>(ref left) > Unsafe.As<T, Decimal>(ref right);
			}

			throw new NotSupportedException($"Operator > is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean GreaterEqual<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				return Unsafe.As<T, Char>(ref left) >= Unsafe.As<T, Char>(ref right);
			}

			if (typeof(T) == typeof(SByte))
			{
				return Unsafe.As<T, SByte>(ref left) >= Unsafe.As<T, SByte>(ref right);
			}

			if (typeof(T) == typeof(Byte))
			{
				return Unsafe.As<T, Byte>(ref left) >= Unsafe.As<T, Byte>(ref right);
			}

			if (typeof(T) == typeof(Int16))
			{
				return Unsafe.As<T, Int16>(ref left) >= Unsafe.As<T, Int16>(ref right);
			}

			if (typeof(T) == typeof(UInt16))
			{
				return Unsafe.As<T, UInt16>(ref left) >= Unsafe.As<T, UInt16>(ref right);
			}

			if (typeof(T) == typeof(Int32))
			{
				return Unsafe.As<T, Int32>(ref left) >= Unsafe.As<T, Int32>(ref right);
			}

			if (typeof(T) == typeof(UInt32))
			{
				return Unsafe.As<T, UInt32>(ref left) >= Unsafe.As<T, UInt32>(ref right);
			}

			if (typeof(T) == typeof(Int64))
			{
				return Unsafe.As<T, Int64>(ref left) >= Unsafe.As<T, Int64>(ref right);
			}

			if (typeof(T) == typeof(UInt64))
			{
				return Unsafe.As<T, UInt64>(ref left) >= Unsafe.As<T, UInt64>(ref right);
			}

			if (typeof(T) == typeof(Single))
			{
				return Unsafe.As<T, Single>(ref left) >= Unsafe.As<T, Single>(ref right);
			}

			if (typeof(T) == typeof(Double))
			{
				return Unsafe.As<T, Double>(ref left) >= Unsafe.As<T, Double>(ref right);
			}

			if (typeof(T) == typeof(Decimal))
			{
				return Unsafe.As<T, Decimal>(ref left) >= Unsafe.As<T, Decimal>(ref right);
			}

			throw new NotSupportedException($"Operator >= is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean Less<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				return Unsafe.As<T, Char>(ref left) < Unsafe.As<T, Char>(ref right);
			}

			if (typeof(T) == typeof(SByte))
			{
				return Unsafe.As<T, SByte>(ref left) < Unsafe.As<T, SByte>(ref right);
			}

			if (typeof(T) == typeof(Byte))
			{
				return Unsafe.As<T, Byte>(ref left) < Unsafe.As<T, Byte>(ref right);
			}

			if (typeof(T) == typeof(Int16))
			{
				return Unsafe.As<T, Int16>(ref left) < Unsafe.As<T, Int16>(ref right);
			}

			if (typeof(T) == typeof(UInt16))
			{
				return Unsafe.As<T, UInt16>(ref left) < Unsafe.As<T, UInt16>(ref right);
			}

			if (typeof(T) == typeof(Int32))
			{
				return Unsafe.As<T, Int32>(ref left) < Unsafe.As<T, Int32>(ref right);
			}

			if (typeof(T) == typeof(UInt32))
			{
				return Unsafe.As<T, UInt32>(ref left) < Unsafe.As<T, UInt32>(ref right);
			}

			if (typeof(T) == typeof(Int64))
			{
				return Unsafe.As<T, Int64>(ref left) < Unsafe.As<T, Int64>(ref right);
			}

			if (typeof(T) == typeof(UInt64))
			{
				return Unsafe.As<T, UInt64>(ref left) < Unsafe.As<T, UInt64>(ref right);
			}

			if (typeof(T) == typeof(Single))
			{
				return Unsafe.As<T, Single>(ref left) < Unsafe.As<T, Single>(ref right);
			}

			if (typeof(T) == typeof(Double))
			{
				return Unsafe.As<T, Double>(ref left) < Unsafe.As<T, Double>(ref right);
			}

			if (typeof(T) == typeof(Decimal))
			{
				return Unsafe.As<T, Decimal>(ref left) < Unsafe.As<T, Decimal>(ref right);
			}

			throw new NotSupportedException($"Operator < is not supported for {typeof(T)} type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean LessEqual<T>(T left, T right) where T : unmanaged, IComparable
		{
			if (typeof(T) == typeof(Char))
			{
				return Unsafe.As<T, Char>(ref left) <= Unsafe.As<T, Char>(ref right);
			}

			if (typeof(T) == typeof(SByte))
			{
				return Unsafe.As<T, SByte>(ref left) <= Unsafe.As<T, SByte>(ref right);
			}

			if (typeof(T) == typeof(Byte))
			{
				return Unsafe.As<T, Byte>(ref left) <= Unsafe.As<T, Byte>(ref right);
			}

			if (typeof(T) == typeof(Int16))
			{
				return Unsafe.As<T, Int16>(ref left) <= Unsafe.As<T, Int16>(ref right);
			}

			if (typeof(T) == typeof(UInt16))
			{
				return Unsafe.As<T, UInt16>(ref left) <= Unsafe.As<T, UInt16>(ref right);
			}

			if (typeof(T) == typeof(Int32))
			{
				return Unsafe.As<T, Int32>(ref left) <= Unsafe.As<T, Int32>(ref right);
			}

			if (typeof(T) == typeof(UInt32))
			{
				return Unsafe.As<T, UInt32>(ref left) <= Unsafe.As<T, UInt32>(ref right);
			}

			if (typeof(T) == typeof(Int64))
			{
				return Unsafe.As<T, Int64>(ref left) <= Unsafe.As<T, Int64>(ref right);
			}

			if (typeof(T) == typeof(UInt64))
			{
				return Unsafe.As<T, UInt64>(ref left) <= Unsafe.As<T, UInt64>(ref right);
			}

			if (typeof(T) == typeof(Single))
			{
				return Unsafe.As<T, Single>(ref left) <= Unsafe.As<T, Single>(ref right);
			}

			if (typeof(T) == typeof(Double))
			{
				return Unsafe.As<T, Double>(ref left) <= Unsafe.As<T, Double>(ref right);
			}

			if (typeof(T) == typeof(Decimal))
			{
				return Unsafe.As<T, Decimal>(ref left) <= Unsafe.As<T, Decimal>(ref right);
			}

			throw new NotSupportedException($"Operator <= is not supported for {typeof(T)} type");
		}

    }
}