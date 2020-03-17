// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Common_Library.Utils.Math
{   
    public static partial class MathUtils
    {
        [SuppressMessage("ReSharper", "InvertIf")]
        public static class Unsafe
        {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T Add<T>(T left, T right) where T : unmanaged
			{
				if (typeof(T) == typeof(Char))
				{
					return (T)(Object)((Char)(Object)left + (Char)(Object)right);
				}

				if (typeof(T) == typeof(SByte))
				{
					return (T)(Object)((SByte)(Object)left + (SByte)(Object)right);
				}

				if (typeof(T) == typeof(Byte))
				{
					return (T)(Object)((Byte)(Object)left + (Byte)(Object)right);
				}

				if (typeof(T) == typeof(Int16))
				{
					return (T)(Object)((Int16)(Object)left + (Int16)(Object)right);
				}

				if (typeof(T) == typeof(UInt16))
				{
					return (T)(Object)((UInt16)(Object)left + (UInt16)(Object)right);
				}

				if (typeof(T) == typeof(Int32))
				{
					return (T)(Object)((Int32)(Object)left + (Int32)(Object)right);
				}

				if (typeof(T) == typeof(UInt32))
				{
					return (T)(Object)((UInt32)(Object)left + (UInt32)(Object)right);
				}

				if (typeof(T) == typeof(Int64))
				{
					return (T)(Object)((Int64)(Object)left + (Int64)(Object)right);
				}

				if (typeof(T) == typeof(UInt64))
				{
					return (T)(Object)((UInt64)(Object)left + (UInt64)(Object)right);
				}

				if (typeof(T) == typeof(Single))
				{
					return (T)(Object)((Single)(Object)left + (Single)(Object)right);
				}

				if (typeof(T) == typeof(Double))
				{
					return (T)(Object)((Double)(Object)left + (Double)(Object)right);
				}

				if (typeof(T) == typeof(Decimal))
				{
					return (T)(Object)((Decimal)(Object)left + (Decimal)(Object)right);
				}

				throw new NotSupportedException($"Operator + is not supported for {typeof(T)} type");
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T Substract<T>(T left, T right) where T : unmanaged
			{
				if (typeof(T) == typeof(Char))
				{
					return (T)(Object)((Char)(Object)left - (Char)(Object)right);
				}

				if (typeof(T) == typeof(SByte))
				{
					return (T)(Object)((SByte)(Object)left - (SByte)(Object)right);
				}

				if (typeof(T) == typeof(Byte))
				{
					return (T)(Object)((Byte)(Object)left - (Byte)(Object)right);
				}

				if (typeof(T) == typeof(Int16))
				{
					return (T)(Object)((Int16)(Object)left - (Int16)(Object)right);
				}

				if (typeof(T) == typeof(UInt16))
				{
					return (T)(Object)((UInt16)(Object)left - (UInt16)(Object)right);
				}

				if (typeof(T) == typeof(Int32))
				{
					return (T)(Object)((Int32)(Object)left - (Int32)(Object)right);
				}

				if (typeof(T) == typeof(UInt32))
				{
					return (T)(Object)((UInt32)(Object)left - (UInt32)(Object)right);
				}

				if (typeof(T) == typeof(Int64))
				{
					return (T)(Object)((Int64)(Object)left - (Int64)(Object)right);
				}

				if (typeof(T) == typeof(UInt64))
				{
					return (T)(Object)((UInt64)(Object)left - (UInt64)(Object)right);
				}

				if (typeof(T) == typeof(Single))
				{
					return (T)(Object)((Single)(Object)left - (Single)(Object)right);
				}

				if (typeof(T) == typeof(Double))
				{
					return (T)(Object)((Double)(Object)left - (Double)(Object)right);
				}

				if (typeof(T) == typeof(Decimal))
				{
					return (T)(Object)((Decimal)(Object)left - (Decimal)(Object)right);
				}

				throw new NotSupportedException($"Operator - is not supported for {typeof(T)} type");
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T Multiply<T>(T left, T right) where T : unmanaged
			{
				if (typeof(T) == typeof(Char))
				{
					return (T)(Object)((Char)(Object)left * (Char)(Object)right);
				}

				if (typeof(T) == typeof(SByte))
				{
					return (T)(Object)((SByte)(Object)left * (SByte)(Object)right);
				}

				if (typeof(T) == typeof(Byte))
				{
					return (T)(Object)((Byte)(Object)left * (Byte)(Object)right);
				}

				if (typeof(T) == typeof(Int16))
				{
					return (T)(Object)((Int16)(Object)left * (Int16)(Object)right);
				}

				if (typeof(T) == typeof(UInt16))
				{
					return (T)(Object)((UInt16)(Object)left * (UInt16)(Object)right);
				}

				if (typeof(T) == typeof(Int32))
				{
					return (T)(Object)((Int32)(Object)left * (Int32)(Object)right);
				}

				if (typeof(T) == typeof(UInt32))
				{
					return (T)(Object)((UInt32)(Object)left * (UInt32)(Object)right);
				}

				if (typeof(T) == typeof(Int64))
				{
					return (T)(Object)((Int64)(Object)left * (Int64)(Object)right);
				}

				if (typeof(T) == typeof(UInt64))
				{
					return (T)(Object)((UInt64)(Object)left * (UInt64)(Object)right);
				}

				if (typeof(T) == typeof(Single))
				{
					return (T)(Object)((Single)(Object)left * (Single)(Object)right);
				}

				if (typeof(T) == typeof(Double))
				{
					return (T)(Object)((Double)(Object)left * (Double)(Object)right);
				}

				if (typeof(T) == typeof(Decimal))
				{
					return (T)(Object)((Decimal)(Object)left * (Decimal)(Object)right);
				}

				throw new NotSupportedException($"Operator * is not supported for {typeof(T)} type");
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T Divide<T>(T left, T right) where T : unmanaged
			{
				if (typeof(T) == typeof(Char))
				{
					return (T)(Object)((Char)(Object)left / (Char)(Object)right);
				}

				if (typeof(T) == typeof(SByte))
				{
					return (T)(Object)((SByte)(Object)left / (SByte)(Object)right);
				}

				if (typeof(T) == typeof(Byte))
				{
					return (T)(Object)((Byte)(Object)left / (Byte)(Object)right);
				}

				if (typeof(T) == typeof(Int16))
				{
					return (T)(Object)((Int16)(Object)left / (Int16)(Object)right);
				}

				if (typeof(T) == typeof(UInt16))
				{
					return (T)(Object)((UInt16)(Object)left / (UInt16)(Object)right);
				}

				if (typeof(T) == typeof(Int32))
				{
					return (T)(Object)((Int32)(Object)left / (Int32)(Object)right);
				}

				if (typeof(T) == typeof(UInt32))
				{
					return (T)(Object)((UInt32)(Object)left / (UInt32)(Object)right);
				}

				if (typeof(T) == typeof(Int64))
				{
					return (T)(Object)((Int64)(Object)left / (Int64)(Object)right);
				}

				if (typeof(T) == typeof(UInt64))
				{
					return (T)(Object)((UInt64)(Object)left / (UInt64)(Object)right);
				}

				if (typeof(T) == typeof(Single))
				{
					return (T)(Object)((Single)(Object)left / (Single)(Object)right);
				}

				if (typeof(T) == typeof(Double))
				{
					return (T)(Object)((Double)(Object)left / (Double)(Object)right);
				}

				if (typeof(T) == typeof(Decimal))
				{
					return (T)(Object)((Decimal)(Object)left / (Decimal)(Object)right);
				}

				throw new NotSupportedException($"Operator / is not supported for {typeof(T)} type");
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T Modular<T>(T left, T right) where T : unmanaged
			{
				if (typeof(T) == typeof(Char))
				{
					return (T)(Object)((Char)(Object)left % (Char)(Object)right);
				}

				if (typeof(T) == typeof(SByte))
				{
					return (T)(Object)((SByte)(Object)left % (SByte)(Object)right);
				}

				if (typeof(T) == typeof(Byte))
				{
					return (T)(Object)((Byte)(Object)left % (Byte)(Object)right);
				}

				if (typeof(T) == typeof(Int16))
				{
					return (T)(Object)((Int16)(Object)left % (Int16)(Object)right);
				}

				if (typeof(T) == typeof(UInt16))
				{
					return (T)(Object)((UInt16)(Object)left % (UInt16)(Object)right);
				}

				if (typeof(T) == typeof(Int32))
				{
					return (T)(Object)((Int32)(Object)left % (Int32)(Object)right);
				}

				if (typeof(T) == typeof(UInt32))
				{
					return (T)(Object)((UInt32)(Object)left % (UInt32)(Object)right);
				}

				if (typeof(T) == typeof(Int64))
				{
					return (T)(Object)((Int64)(Object)left % (Int64)(Object)right);
				}

				if (typeof(T) == typeof(UInt64))
				{
					return (T)(Object)((UInt64)(Object)left % (UInt64)(Object)right);
				}

				if (typeof(T) == typeof(Single))
				{
					return (T)(Object)((Single)(Object)left % (Single)(Object)right);
				}

				if (typeof(T) == typeof(Double))
				{
					return (T)(Object)((Double)(Object)left % (Double)(Object)right);
				}

				if (typeof(T) == typeof(Decimal))
				{
					return (T)(Object)((Decimal)(Object)left % (Decimal)(Object)right);
				}

				throw new NotSupportedException($"Operator % is not supported for {typeof(T)} type");
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T And<T>(T left, T right) where T : unmanaged
			{
				if (typeof(T) == typeof(Char))
				{
					return (T)(Object)((Char)(Object)left & (Char)(Object)right);
				}

				if (typeof(T) == typeof(SByte))
				{
					return (T)(Object)((SByte)(Object)left & (SByte)(Object)right);
				}

				if (typeof(T) == typeof(Byte))
				{
					return (T)(Object)((Byte)(Object)left & (Byte)(Object)right);
				}

				if (typeof(T) == typeof(Int16))
				{
					return (T)(Object)((Int16)(Object)left & (Int16)(Object)right);
				}

				if (typeof(T) == typeof(UInt16))
				{
					return (T)(Object)((UInt16)(Object)left & (UInt16)(Object)right);
				}

				if (typeof(T) == typeof(Int32))
				{
					return (T)(Object)((Int32)(Object)left & (Int32)(Object)right);
				}

				if (typeof(T) == typeof(UInt32))
				{
					return (T)(Object)((UInt32)(Object)left & (UInt32)(Object)right);
				}

				if (typeof(T) == typeof(Int64))
				{
					return (T)(Object)((Int64)(Object)left & (Int64)(Object)right);
				}

				if (typeof(T) == typeof(UInt64))
				{
					return (T)(Object)((UInt64)(Object)left & (UInt64)(Object)right);
				}

				throw new NotSupportedException($"Operator & is not supported for {typeof(T)} type");
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T Or<T>(T left, T right) where T : unmanaged
			{
				if (typeof(T) == typeof(Char))
				{
					return (T)(Object)((Char)(Object)left | (Char)(Object)right);
				}

				if (typeof(T) == typeof(SByte))
				{
					return (T)(Object)((SByte)(Object)left | (SByte)(Object)right);
				}

				if (typeof(T) == typeof(Byte))
				{
					return (T)(Object)((Byte)(Object)left | (Byte)(Object)right);
				}

				if (typeof(T) == typeof(Int16))
				{
					return (T)(Object)((Int16)(Object)left | (Int16)(Object)right);
				}

				if (typeof(T) == typeof(UInt16))
				{
					return (T)(Object)((UInt16)(Object)left | (UInt16)(Object)right);
				}

				if (typeof(T) == typeof(Int32))
				{
					return (T)(Object)((Int32)(Object)left | (Int32)(Object)right);
				}

				if (typeof(T) == typeof(UInt32))
				{
					return (T)(Object)((UInt32)(Object)left | (UInt32)(Object)right);
				}

				if (typeof(T) == typeof(Int64))
				{
					return (T)(Object)((Int64)(Object)left | (Int64)(Object)right);
				}

				if (typeof(T) == typeof(UInt64))
				{
					return (T)(Object)((UInt64)(Object)left | (UInt64)(Object)right);
				}

				throw new NotSupportedException($"Operator | is not supported for {typeof(T)} type");
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T Xor<T>(T left, T right) where T : unmanaged
			{
				if (typeof(T) == typeof(Char))
				{
					return (T)(Object)((Char)(Object)left ^ (Char)(Object)right);
				}

				if (typeof(T) == typeof(SByte))
				{
					return (T)(Object)((SByte)(Object)left ^ (SByte)(Object)right);
				}

				if (typeof(T) == typeof(Byte))
				{
					return (T)(Object)((Byte)(Object)left ^ (Byte)(Object)right);
				}

				if (typeof(T) == typeof(Int16))
				{
					return (T)(Object)((Int16)(Object)left ^ (Int16)(Object)right);
				}

				if (typeof(T) == typeof(UInt16))
				{
					return (T)(Object)((UInt16)(Object)left ^ (UInt16)(Object)right);
				}

				if (typeof(T) == typeof(Int32))
				{
					return (T)(Object)((Int32)(Object)left ^ (Int32)(Object)right);
				}

				if (typeof(T) == typeof(UInt32))
				{
					return (T)(Object)((UInt32)(Object)left ^ (UInt32)(Object)right);
				}

				if (typeof(T) == typeof(Int64))
				{
					return (T)(Object)((Int64)(Object)left ^ (Int64)(Object)right);
				}

				if (typeof(T) == typeof(UInt64))
				{
					return (T)(Object)((UInt64)(Object)left ^ (UInt64)(Object)right);
				}

				throw new NotSupportedException($"Operator ^ is not supported for {typeof(T)} type");
			}

        }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Range(ref Char value, Char minimum = Char.MinValue, Char maximum = Char.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Char Range(Char value, Char minimum = Char.MinValue, Char maximum = Char.MaxValue, Boolean looped = false)
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
		public static void Range(ref SByte value, SByte minimum = 0, SByte maximum = SByte.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SByte Range(SByte value, SByte minimum = 0, SByte maximum = SByte.MaxValue, Boolean looped = false)
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
		public static void Range(ref Byte value, Byte minimum = 0, Byte maximum = Byte.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Byte Range(Byte value, Byte minimum = 0, Byte maximum = Byte.MaxValue, Boolean looped = false)
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
		public static void Range(ref Int16 value, Int16 minimum = 0, Int16 maximum = Int16.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int16 Range(Int16 value, Int16 minimum = 0, Int16 maximum = Int16.MaxValue, Boolean looped = false)
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
		public static void Range(ref UInt16 value, UInt16 minimum = 0, UInt16 maximum = UInt16.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UInt16 Range(UInt16 value, UInt16 minimum = 0, UInt16 maximum = UInt16.MaxValue, Boolean looped = false)
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
		public static void Range(ref Int32 value, Int32 minimum = 0, Int32 maximum = Int32.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Range(ref UInt32 value, UInt32 minimum = 0, UInt32 maximum = UInt32.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UInt32 Range(UInt32 value, UInt32 minimum = 0, UInt32 maximum = UInt32.MaxValue, Boolean looped = false)
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
		public static void Range(ref Int64 value, Int64 minimum = 0, Int64 maximum = Int64.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int64 Range(Int64 value, Int64 minimum = 0, Int64 maximum = Int64.MaxValue, Boolean looped = false)
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
		public static void Range(ref UInt64 value, UInt64 minimum = 0, UInt64 maximum = UInt64.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UInt64 Range(UInt64 value, UInt64 minimum = 0, UInt64 maximum = UInt64.MaxValue, Boolean looped = false)
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
		public static void Range(ref Single value, Single minimum = 0, Single maximum = Single.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Range(ref Double value, Double minimum = 0, Double maximum = Double.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Range(ref Decimal value, Decimal minimum = 0, Decimal maximum = Decimal.MaxValue, Boolean looped = false)
		{
			 value = Range(value, minimum, maximum, looped);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Decimal Range(Decimal value, Decimal minimum = 0, Decimal maximum = Decimal.MaxValue, Boolean looped = false)
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

    }
}