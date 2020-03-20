// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Runtime.CompilerServices;
using Common_Library.Utils.Math;

namespace Common_Library.Types.Other
{
    [Flags]
    public enum PointOffset
    {
        None = 0,
        Up = 1,
        Down = 3,
        Left = 4,
        Right = 12,
        
        UpLeft = 5,
        DownLeft = 7,
        UpRight = 13,
        DownRight = 15
    }

	public struct CharPoint
	{
		public static CharPoint operator +(CharPoint point1, CharPoint point2)
		{
			Int32 x = point1.X + point2.X;
			Int32 y = point1.Y + point2.Y;
			return new CharPoint(Unsafe.As<Int32, Char>(ref x), Unsafe.As<Int32, Char>(ref y));
		}
		public static CharPoint operator -(CharPoint point1, CharPoint point2)
		{
			Int32 x = point1.X - point2.X;
			Int32 y = point1.Y - point2.Y;
			return new CharPoint(Unsafe.As<Int32, Char>(ref x), Unsafe.As<Int32, Char>(ref y));
		}
		public static CharPoint operator *(CharPoint point1, CharPoint point2)
		{
			Int32 x = point1.X * point2.X;
			Int32 y = point1.Y * point2.Y;
			return new CharPoint(Unsafe.As<Int32, Char>(ref x), Unsafe.As<Int32, Char>(ref y));
		}
		public static CharPoint operator /(CharPoint point1, CharPoint point2)
		{
			Int32 x = point1.X / point2.X;
			Int32 y = point1.Y / point2.Y;
			return new CharPoint(Unsafe.As<Int32, Char>(ref x), Unsafe.As<Int32, Char>(ref y));
		}
		public static CharPoint operator %(CharPoint point1, CharPoint point2)
		{
			Int32 x = point1.X % point2.X;
			Int32 y = point1.Y % point2.Y;
			return new CharPoint(Unsafe.As<Int32, Char>(ref x), Unsafe.As<Int32, Char>(ref y));
		}

		public readonly Char X;
		public readonly Char Y;

		public CharPoint(Char x, Char y)
		{
			X = x;
			Y = y;
		}

		public static readonly CharPoint Zero = new CharPoint((Char)0, (Char)0);
		public static readonly CharPoint One = new CharPoint((Char)1, (Char)1);
		public static readonly CharPoint Minimum = new CharPoint(Char.MinValue, Char.MinValue);
		public static readonly CharPoint Maximum = new CharPoint(Char.MaxValue, Char.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CharPoint Offset(CharPoint point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CharPoint Offset(Char x, Char y)
		{
			return this + new CharPoint(x, y);
		}

		public CharPoint Offset(PointOffset offset, Char count = (Char)1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new CharPoint((Char)0, count)),
				PointOffset.Down => (this + new CharPoint((Char)0, count)),
				PointOffset.Left => (this - new CharPoint(count, (Char)0)),
				PointOffset.Right => (this + new CharPoint(count, (Char)0)),
				PointOffset.UpLeft => (this - new CharPoint(count, count)),
				PointOffset.DownLeft => (this + new CharPoint((Char)0, count) - new CharPoint(count, (Char)0)),
				PointOffset.UpRight => (this - new CharPoint((Char)0, count) + new CharPoint(count, (Char)0)),
				PointOffset.DownRight => (this + new CharPoint(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CharPoint Delta(CharPoint point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CharPoint Delta(Char count)
		{
			return this - new CharPoint(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CharPoint Delta(Char x, Char y)
		{
			return this - new CharPoint(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return true;
		}
	}

	public struct SBytePoint
	{
		public static SBytePoint operator +(SBytePoint point1, SBytePoint point2)
		{
			Int32 x = point1.X + point2.X;
			Int32 y = point1.Y + point2.Y;
			return new SBytePoint(Unsafe.As<Int32, SByte>(ref x), Unsafe.As<Int32, SByte>(ref y));
		}
		public static SBytePoint operator -(SBytePoint point1, SBytePoint point2)
		{
			Int32 x = point1.X - point2.X;
			Int32 y = point1.Y - point2.Y;
			return new SBytePoint(Unsafe.As<Int32, SByte>(ref x), Unsafe.As<Int32, SByte>(ref y));
		}
		public static SBytePoint operator *(SBytePoint point1, SBytePoint point2)
		{
			Int32 x = point1.X * point2.X;
			Int32 y = point1.Y * point2.Y;
			return new SBytePoint(Unsafe.As<Int32, SByte>(ref x), Unsafe.As<Int32, SByte>(ref y));
		}
		public static SBytePoint operator /(SBytePoint point1, SBytePoint point2)
		{
			Int32 x = point1.X / point2.X;
			Int32 y = point1.Y / point2.Y;
			return new SBytePoint(Unsafe.As<Int32, SByte>(ref x), Unsafe.As<Int32, SByte>(ref y));
		}
		public static SBytePoint operator %(SBytePoint point1, SBytePoint point2)
		{
			Int32 x = point1.X % point2.X;
			Int32 y = point1.Y % point2.Y;
			return new SBytePoint(Unsafe.As<Int32, SByte>(ref x), Unsafe.As<Int32, SByte>(ref y));
		}

		public readonly SByte X;
		public readonly SByte Y;

		public SBytePoint(SByte x, SByte y)
		{
			X = x;
			Y = y;
		}

		public static readonly SBytePoint Zero = new SBytePoint(0, 0);
		public static readonly SBytePoint One = new SBytePoint(1, 1);
		public static readonly SBytePoint Minimum = new SBytePoint(SByte.MinValue, SByte.MinValue);
		public static readonly SBytePoint Maximum = new SBytePoint(SByte.MaxValue, SByte.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SBytePoint Offset(SBytePoint point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SBytePoint Offset(SByte x, SByte y)
		{
			return this + new SBytePoint(x, y);
		}

		public SBytePoint Offset(PointOffset offset, SByte count = 1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new SBytePoint(0, count)),
				PointOffset.Down => (this + new SBytePoint(0, count)),
				PointOffset.Left => (this - new SBytePoint(count, 0)),
				PointOffset.Right => (this + new SBytePoint(count, 0)),
				PointOffset.UpLeft => (this - new SBytePoint(count, count)),
				PointOffset.DownLeft => (this + new SBytePoint(0, count) - new SBytePoint(count, 0)),
				PointOffset.UpRight => (this - new SBytePoint(0, count) + new SBytePoint(count, 0)),
				PointOffset.DownRight => (this + new SBytePoint(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SBytePoint Delta(SBytePoint point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SBytePoint Delta(SByte count)
		{
			return this - new SBytePoint(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SBytePoint Delta(SByte x, SByte y)
		{
			return this - new SBytePoint(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return X >= 0 && Y >= 0;
		}
	}

	public struct BytePoint
	{
		public static BytePoint operator +(BytePoint point1, BytePoint point2)
		{
			Int32 x = point1.X + point2.X;
			Int32 y = point1.Y + point2.Y;
			return new BytePoint(Unsafe.As<Int32, Byte>(ref x), Unsafe.As<Int32, Byte>(ref y));
		}
		public static BytePoint operator -(BytePoint point1, BytePoint point2)
		{
			Int32 x = point1.X - point2.X;
			Int32 y = point1.Y - point2.Y;
			return new BytePoint(Unsafe.As<Int32, Byte>(ref x), Unsafe.As<Int32, Byte>(ref y));
		}
		public static BytePoint operator *(BytePoint point1, BytePoint point2)
		{
			Int32 x = point1.X * point2.X;
			Int32 y = point1.Y * point2.Y;
			return new BytePoint(Unsafe.As<Int32, Byte>(ref x), Unsafe.As<Int32, Byte>(ref y));
		}
		public static BytePoint operator /(BytePoint point1, BytePoint point2)
		{
			Int32 x = point1.X / point2.X;
			Int32 y = point1.Y / point2.Y;
			return new BytePoint(Unsafe.As<Int32, Byte>(ref x), Unsafe.As<Int32, Byte>(ref y));
		}
		public static BytePoint operator %(BytePoint point1, BytePoint point2)
		{
			Int32 x = point1.X % point2.X;
			Int32 y = point1.Y % point2.Y;
			return new BytePoint(Unsafe.As<Int32, Byte>(ref x), Unsafe.As<Int32, Byte>(ref y));
		}

		public readonly Byte X;
		public readonly Byte Y;

		public BytePoint(Byte x, Byte y)
		{
			X = x;
			Y = y;
		}

		public static readonly BytePoint Zero = new BytePoint(0, 0);
		public static readonly BytePoint One = new BytePoint(1, 1);
		public static readonly BytePoint Minimum = new BytePoint(Byte.MinValue, Byte.MinValue);
		public static readonly BytePoint Maximum = new BytePoint(Byte.MaxValue, Byte.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BytePoint Offset(BytePoint point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BytePoint Offset(Byte x, Byte y)
		{
			return this + new BytePoint(x, y);
		}

		public BytePoint Offset(PointOffset offset, Byte count = 1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new BytePoint(0, count)),
				PointOffset.Down => (this + new BytePoint(0, count)),
				PointOffset.Left => (this - new BytePoint(count, 0)),
				PointOffset.Right => (this + new BytePoint(count, 0)),
				PointOffset.UpLeft => (this - new BytePoint(count, count)),
				PointOffset.DownLeft => (this + new BytePoint(0, count) - new BytePoint(count, 0)),
				PointOffset.UpRight => (this - new BytePoint(0, count) + new BytePoint(count, 0)),
				PointOffset.DownRight => (this + new BytePoint(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BytePoint Delta(BytePoint point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BytePoint Delta(Byte count)
		{
			return this - new BytePoint(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BytePoint Delta(Byte x, Byte y)
		{
			return this - new BytePoint(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return true;
		}
	}

	public struct Int16Point
	{
		public static Int16Point operator +(Int16Point point1, Int16Point point2)
		{
			Int32 x = point1.X + point2.X;
			Int32 y = point1.Y + point2.Y;
			return new Int16Point(Unsafe.As<Int32, Int16>(ref x), Unsafe.As<Int32, Int16>(ref y));
		}
		public static Int16Point operator -(Int16Point point1, Int16Point point2)
		{
			Int32 x = point1.X - point2.X;
			Int32 y = point1.Y - point2.Y;
			return new Int16Point(Unsafe.As<Int32, Int16>(ref x), Unsafe.As<Int32, Int16>(ref y));
		}
		public static Int16Point operator *(Int16Point point1, Int16Point point2)
		{
			Int32 x = point1.X * point2.X;
			Int32 y = point1.Y * point2.Y;
			return new Int16Point(Unsafe.As<Int32, Int16>(ref x), Unsafe.As<Int32, Int16>(ref y));
		}
		public static Int16Point operator /(Int16Point point1, Int16Point point2)
		{
			Int32 x = point1.X / point2.X;
			Int32 y = point1.Y / point2.Y;
			return new Int16Point(Unsafe.As<Int32, Int16>(ref x), Unsafe.As<Int32, Int16>(ref y));
		}
		public static Int16Point operator %(Int16Point point1, Int16Point point2)
		{
			Int32 x = point1.X % point2.X;
			Int32 y = point1.Y % point2.Y;
			return new Int16Point(Unsafe.As<Int32, Int16>(ref x), Unsafe.As<Int32, Int16>(ref y));
		}

		public readonly Int16 X;
		public readonly Int16 Y;

		public Int16Point(Int16 x, Int16 y)
		{
			X = x;
			Y = y;
		}

		public static readonly Int16Point Zero = new Int16Point(0, 0);
		public static readonly Int16Point One = new Int16Point(1, 1);
		public static readonly Int16Point Minimum = new Int16Point(Int16.MinValue, Int16.MinValue);
		public static readonly Int16Point Maximum = new Int16Point(Int16.MaxValue, Int16.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int16Point Offset(Int16Point point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int16Point Offset(Int16 x, Int16 y)
		{
			return this + new Int16Point(x, y);
		}

		public Int16Point Offset(PointOffset offset, Int16 count = 1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new Int16Point(0, count)),
				PointOffset.Down => (this + new Int16Point(0, count)),
				PointOffset.Left => (this - new Int16Point(count, 0)),
				PointOffset.Right => (this + new Int16Point(count, 0)),
				PointOffset.UpLeft => (this - new Int16Point(count, count)),
				PointOffset.DownLeft => (this + new Int16Point(0, count) - new Int16Point(count, 0)),
				PointOffset.UpRight => (this - new Int16Point(0, count) + new Int16Point(count, 0)),
				PointOffset.DownRight => (this + new Int16Point(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int16Point Delta(Int16Point point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int16Point Delta(Int16 count)
		{
			return this - new Int16Point(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int16Point Delta(Int16 x, Int16 y)
		{
			return this - new Int16Point(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return X >= 0 && Y >= 0;
		}
	}

	public struct UInt16Point
	{
		public static UInt16Point operator +(UInt16Point point1, UInt16Point point2)
		{
			Int32 x = point1.X + point2.X;
			Int32 y = point1.Y + point2.Y;
			return new UInt16Point(Unsafe.As<Int32, UInt16>(ref x), Unsafe.As<Int32, UInt16>(ref y));
		}
		public static UInt16Point operator -(UInt16Point point1, UInt16Point point2)
		{
			Int32 x = point1.X - point2.X;
			Int32 y = point1.Y - point2.Y;
			return new UInt16Point(Unsafe.As<Int32, UInt16>(ref x), Unsafe.As<Int32, UInt16>(ref y));
		}
		public static UInt16Point operator *(UInt16Point point1, UInt16Point point2)
		{
			Int32 x = point1.X * point2.X;
			Int32 y = point1.Y * point2.Y;
			return new UInt16Point(Unsafe.As<Int32, UInt16>(ref x), Unsafe.As<Int32, UInt16>(ref y));
		}
		public static UInt16Point operator /(UInt16Point point1, UInt16Point point2)
		{
			Int32 x = point1.X / point2.X;
			Int32 y = point1.Y / point2.Y;
			return new UInt16Point(Unsafe.As<Int32, UInt16>(ref x), Unsafe.As<Int32, UInt16>(ref y));
		}
		public static UInt16Point operator %(UInt16Point point1, UInt16Point point2)
		{
			Int32 x = point1.X % point2.X;
			Int32 y = point1.Y % point2.Y;
			return new UInt16Point(Unsafe.As<Int32, UInt16>(ref x), Unsafe.As<Int32, UInt16>(ref y));
		}

		public readonly UInt16 X;
		public readonly UInt16 Y;

		public UInt16Point(UInt16 x, UInt16 y)
		{
			X = x;
			Y = y;
		}

		public static readonly UInt16Point Zero = new UInt16Point(0, 0);
		public static readonly UInt16Point One = new UInt16Point(1, 1);
		public static readonly UInt16Point Minimum = new UInt16Point(UInt16.MinValue, UInt16.MinValue);
		public static readonly UInt16Point Maximum = new UInt16Point(UInt16.MaxValue, UInt16.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt16Point Offset(UInt16Point point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt16Point Offset(UInt16 x, UInt16 y)
		{
			return this + new UInt16Point(x, y);
		}

		public UInt16Point Offset(PointOffset offset, UInt16 count = 1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new UInt16Point(0, count)),
				PointOffset.Down => (this + new UInt16Point(0, count)),
				PointOffset.Left => (this - new UInt16Point(count, 0)),
				PointOffset.Right => (this + new UInt16Point(count, 0)),
				PointOffset.UpLeft => (this - new UInt16Point(count, count)),
				PointOffset.DownLeft => (this + new UInt16Point(0, count) - new UInt16Point(count, 0)),
				PointOffset.UpRight => (this - new UInt16Point(0, count) + new UInt16Point(count, 0)),
				PointOffset.DownRight => (this + new UInt16Point(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt16Point Delta(UInt16Point point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt16Point Delta(UInt16 count)
		{
			return this - new UInt16Point(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt16Point Delta(UInt16 x, UInt16 y)
		{
			return this - new UInt16Point(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return true;
		}
	}

	public struct Int32Point
	{
		public static Int32Point operator +(Int32Point point1, Int32Point point2)
		{
			return new Int32Point(point1.X + point2.X, point1.Y + point2.Y);
		}
		public static Int32Point operator -(Int32Point point1, Int32Point point2)
		{
			return new Int32Point(point1.X - point2.X, point1.Y - point2.Y);
		}
		public static Int32Point operator *(Int32Point point1, Int32Point point2)
		{
			return new Int32Point(point1.X * point2.X, point1.Y * point2.Y);
		}
		public static Int32Point operator /(Int32Point point1, Int32Point point2)
		{
			return new Int32Point(point1.X / point2.X, point1.Y / point2.Y);
		}
		public static Int32Point operator %(Int32Point point1, Int32Point point2)
		{
			return new Int32Point(point1.X % point2.X, point1.Y % point2.Y);
		}

		public readonly Int32 X;
		public readonly Int32 Y;

		public Int32Point(Int32 x, Int32 y)
		{
			X = x;
			Y = y;
		}

		public static readonly Int32Point Zero = new Int32Point(0, 0);
		public static readonly Int32Point One = new Int32Point(1, 1);
		public static readonly Int32Point Minimum = new Int32Point(Int32.MinValue, Int32.MinValue);
		public static readonly Int32Point Maximum = new Int32Point(Int32.MaxValue, Int32.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int32Point Offset(Int32Point point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int32Point Offset(Int32 x, Int32 y)
		{
			return this + new Int32Point(x, y);
		}

		public Int32Point Offset(PointOffset offset, Int32 count = 1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new Int32Point(0, count)),
				PointOffset.Down => (this + new Int32Point(0, count)),
				PointOffset.Left => (this - new Int32Point(count, 0)),
				PointOffset.Right => (this + new Int32Point(count, 0)),
				PointOffset.UpLeft => (this - new Int32Point(count, count)),
				PointOffset.DownLeft => (this + new Int32Point(0, count) - new Int32Point(count, 0)),
				PointOffset.UpRight => (this - new Int32Point(0, count) + new Int32Point(count, 0)),
				PointOffset.DownRight => (this + new Int32Point(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int32Point Delta(Int32Point point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int32Point Delta(Int32 count)
		{
			return this - new Int32Point(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int32Point Delta(Int32 x, Int32 y)
		{
			return this - new Int32Point(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return X >= 0 && Y >= 0;
		}
	}

	public struct UInt32Point
	{
		public static UInt32Point operator +(UInt32Point point1, UInt32Point point2)
		{
			return new UInt32Point(point1.X + point2.X, point1.Y + point2.Y);
		}
		public static UInt32Point operator -(UInt32Point point1, UInt32Point point2)
		{
			return new UInt32Point(point1.X - point2.X, point1.Y - point2.Y);
		}
		public static UInt32Point operator *(UInt32Point point1, UInt32Point point2)
		{
			return new UInt32Point(point1.X * point2.X, point1.Y * point2.Y);
		}
		public static UInt32Point operator /(UInt32Point point1, UInt32Point point2)
		{
			return new UInt32Point(point1.X / point2.X, point1.Y / point2.Y);
		}
		public static UInt32Point operator %(UInt32Point point1, UInt32Point point2)
		{
			return new UInt32Point(point1.X % point2.X, point1.Y % point2.Y);
		}

		public readonly UInt32 X;
		public readonly UInt32 Y;

		public UInt32Point(UInt32 x, UInt32 y)
		{
			X = x;
			Y = y;
		}

		public static readonly UInt32Point Zero = new UInt32Point(0, 0);
		public static readonly UInt32Point One = new UInt32Point(1, 1);
		public static readonly UInt32Point Minimum = new UInt32Point(UInt32.MinValue, UInt32.MinValue);
		public static readonly UInt32Point Maximum = new UInt32Point(UInt32.MaxValue, UInt32.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt32Point Offset(UInt32Point point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt32Point Offset(UInt32 x, UInt32 y)
		{
			return this + new UInt32Point(x, y);
		}

		public UInt32Point Offset(PointOffset offset, UInt32 count = 1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new UInt32Point(0, count)),
				PointOffset.Down => (this + new UInt32Point(0, count)),
				PointOffset.Left => (this - new UInt32Point(count, 0)),
				PointOffset.Right => (this + new UInt32Point(count, 0)),
				PointOffset.UpLeft => (this - new UInt32Point(count, count)),
				PointOffset.DownLeft => (this + new UInt32Point(0, count) - new UInt32Point(count, 0)),
				PointOffset.UpRight => (this - new UInt32Point(0, count) + new UInt32Point(count, 0)),
				PointOffset.DownRight => (this + new UInt32Point(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt32Point Delta(UInt32Point point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt32Point Delta(UInt32 count)
		{
			return this - new UInt32Point(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt32Point Delta(UInt32 x, UInt32 y)
		{
			return this - new UInt32Point(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return true;
		}
	}

	public struct Int64Point
	{
		public static Int64Point operator +(Int64Point point1, Int64Point point2)
		{
			return new Int64Point(point1.X + point2.X, point1.Y + point2.Y);
		}
		public static Int64Point operator -(Int64Point point1, Int64Point point2)
		{
			return new Int64Point(point1.X - point2.X, point1.Y - point2.Y);
		}
		public static Int64Point operator *(Int64Point point1, Int64Point point2)
		{
			return new Int64Point(point1.X * point2.X, point1.Y * point2.Y);
		}
		public static Int64Point operator /(Int64Point point1, Int64Point point2)
		{
			return new Int64Point(point1.X / point2.X, point1.Y / point2.Y);
		}
		public static Int64Point operator %(Int64Point point1, Int64Point point2)
		{
			return new Int64Point(point1.X % point2.X, point1.Y % point2.Y);
		}

		public readonly Int64 X;
		public readonly Int64 Y;

		public Int64Point(Int64 x, Int64 y)
		{
			X = x;
			Y = y;
		}

		public static readonly Int64Point Zero = new Int64Point(0, 0);
		public static readonly Int64Point One = new Int64Point(1, 1);
		public static readonly Int64Point Minimum = new Int64Point(Int64.MinValue, Int64.MinValue);
		public static readonly Int64Point Maximum = new Int64Point(Int64.MaxValue, Int64.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int64Point Offset(Int64Point point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int64Point Offset(Int64 x, Int64 y)
		{
			return this + new Int64Point(x, y);
		}

		public Int64Point Offset(PointOffset offset, Int64 count = 1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new Int64Point(0, count)),
				PointOffset.Down => (this + new Int64Point(0, count)),
				PointOffset.Left => (this - new Int64Point(count, 0)),
				PointOffset.Right => (this + new Int64Point(count, 0)),
				PointOffset.UpLeft => (this - new Int64Point(count, count)),
				PointOffset.DownLeft => (this + new Int64Point(0, count) - new Int64Point(count, 0)),
				PointOffset.UpRight => (this - new Int64Point(0, count) + new Int64Point(count, 0)),
				PointOffset.DownRight => (this + new Int64Point(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int64Point Delta(Int64Point point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int64Point Delta(Int64 count)
		{
			return this - new Int64Point(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int64Point Delta(Int64 x, Int64 y)
		{
			return this - new Int64Point(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return X >= 0 && Y >= 0;
		}
	}

	public struct UInt64Point
	{
		public static UInt64Point operator +(UInt64Point point1, UInt64Point point2)
		{
			return new UInt64Point(point1.X + point2.X, point1.Y + point2.Y);
		}
		public static UInt64Point operator -(UInt64Point point1, UInt64Point point2)
		{
			return new UInt64Point(point1.X - point2.X, point1.Y - point2.Y);
		}
		public static UInt64Point operator *(UInt64Point point1, UInt64Point point2)
		{
			return new UInt64Point(point1.X * point2.X, point1.Y * point2.Y);
		}
		public static UInt64Point operator /(UInt64Point point1, UInt64Point point2)
		{
			return new UInt64Point(point1.X / point2.X, point1.Y / point2.Y);
		}
		public static UInt64Point operator %(UInt64Point point1, UInt64Point point2)
		{
			return new UInt64Point(point1.X % point2.X, point1.Y % point2.Y);
		}

		public readonly UInt64 X;
		public readonly UInt64 Y;

		public UInt64Point(UInt64 x, UInt64 y)
		{
			X = x;
			Y = y;
		}

		public static readonly UInt64Point Zero = new UInt64Point(0, 0);
		public static readonly UInt64Point One = new UInt64Point(1, 1);
		public static readonly UInt64Point Minimum = new UInt64Point(UInt64.MinValue, UInt64.MinValue);
		public static readonly UInt64Point Maximum = new UInt64Point(UInt64.MaxValue, UInt64.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt64Point Offset(UInt64Point point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt64Point Offset(UInt64 x, UInt64 y)
		{
			return this + new UInt64Point(x, y);
		}

		public UInt64Point Offset(PointOffset offset, UInt64 count = 1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new UInt64Point(0, count)),
				PointOffset.Down => (this + new UInt64Point(0, count)),
				PointOffset.Left => (this - new UInt64Point(count, 0)),
				PointOffset.Right => (this + new UInt64Point(count, 0)),
				PointOffset.UpLeft => (this - new UInt64Point(count, count)),
				PointOffset.DownLeft => (this + new UInt64Point(0, count) - new UInt64Point(count, 0)),
				PointOffset.UpRight => (this - new UInt64Point(0, count) + new UInt64Point(count, 0)),
				PointOffset.DownRight => (this + new UInt64Point(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt64Point Delta(UInt64Point point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt64Point Delta(UInt64 count)
		{
			return this - new UInt64Point(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt64Point Delta(UInt64 x, UInt64 y)
		{
			return this - new UInt64Point(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return true;
		}
	}

	public struct SinglePoint
	{
		public static SinglePoint operator +(SinglePoint point1, SinglePoint point2)
		{
			return new SinglePoint(point1.X + point2.X, point1.Y + point2.Y);
		}
		public static SinglePoint operator -(SinglePoint point1, SinglePoint point2)
		{
			return new SinglePoint(point1.X - point2.X, point1.Y - point2.Y);
		}
		public static SinglePoint operator *(SinglePoint point1, SinglePoint point2)
		{
			return new SinglePoint(point1.X * point2.X, point1.Y * point2.Y);
		}
		public static SinglePoint operator /(SinglePoint point1, SinglePoint point2)
		{
			return new SinglePoint(point1.X / point2.X, point1.Y / point2.Y);
		}
		public static SinglePoint operator %(SinglePoint point1, SinglePoint point2)
		{
			return new SinglePoint(point1.X % point2.X, point1.Y % point2.Y);
		}

		public readonly Single X;
		public readonly Single Y;

		public SinglePoint(Single x, Single y)
		{
			X = x;
			Y = y;
		}

		public static readonly SinglePoint Zero = new SinglePoint(0, 0);
		public static readonly SinglePoint One = new SinglePoint(1, 1);
		public static readonly SinglePoint Minimum = new SinglePoint(Single.MinValue, Single.MinValue);
		public static readonly SinglePoint Maximum = new SinglePoint(Single.MaxValue, Single.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SinglePoint Offset(SinglePoint point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SinglePoint Offset(Single x, Single y)
		{
			return this + new SinglePoint(x, y);
		}

		public SinglePoint Offset(PointOffset offset, Single count = 1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new SinglePoint(0, count)),
				PointOffset.Down => (this + new SinglePoint(0, count)),
				PointOffset.Left => (this - new SinglePoint(count, 0)),
				PointOffset.Right => (this + new SinglePoint(count, 0)),
				PointOffset.UpLeft => (this - new SinglePoint(count, count)),
				PointOffset.DownLeft => (this + new SinglePoint(0, count) - new SinglePoint(count, 0)),
				PointOffset.UpRight => (this - new SinglePoint(0, count) + new SinglePoint(count, 0)),
				PointOffset.DownRight => (this + new SinglePoint(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SinglePoint Delta(SinglePoint point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SinglePoint Delta(Single count)
		{
			return this - new SinglePoint(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SinglePoint Delta(Single x, Single y)
		{
			return this - new SinglePoint(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return X >= 0 && Y >= 0;
		}
	}

	public struct DoublePoint
	{
		public static DoublePoint operator +(DoublePoint point1, DoublePoint point2)
		{
			return new DoublePoint(point1.X + point2.X, point1.Y + point2.Y);
		}
		public static DoublePoint operator -(DoublePoint point1, DoublePoint point2)
		{
			return new DoublePoint(point1.X - point2.X, point1.Y - point2.Y);
		}
		public static DoublePoint operator *(DoublePoint point1, DoublePoint point2)
		{
			return new DoublePoint(point1.X * point2.X, point1.Y * point2.Y);
		}
		public static DoublePoint operator /(DoublePoint point1, DoublePoint point2)
		{
			return new DoublePoint(point1.X / point2.X, point1.Y / point2.Y);
		}
		public static DoublePoint operator %(DoublePoint point1, DoublePoint point2)
		{
			return new DoublePoint(point1.X % point2.X, point1.Y % point2.Y);
		}

		public readonly Double X;
		public readonly Double Y;

		public DoublePoint(Double x, Double y)
		{
			X = x;
			Y = y;
		}

		public static readonly DoublePoint Zero = new DoublePoint(0, 0);
		public static readonly DoublePoint One = new DoublePoint(1, 1);
		public static readonly DoublePoint Minimum = new DoublePoint(Double.MinValue, Double.MinValue);
		public static readonly DoublePoint Maximum = new DoublePoint(Double.MaxValue, Double.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DoublePoint Offset(DoublePoint point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DoublePoint Offset(Double x, Double y)
		{
			return this + new DoublePoint(x, y);
		}

		public DoublePoint Offset(PointOffset offset, Double count = 1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new DoublePoint(0, count)),
				PointOffset.Down => (this + new DoublePoint(0, count)),
				PointOffset.Left => (this - new DoublePoint(count, 0)),
				PointOffset.Right => (this + new DoublePoint(count, 0)),
				PointOffset.UpLeft => (this - new DoublePoint(count, count)),
				PointOffset.DownLeft => (this + new DoublePoint(0, count) - new DoublePoint(count, 0)),
				PointOffset.UpRight => (this - new DoublePoint(0, count) + new DoublePoint(count, 0)),
				PointOffset.DownRight => (this + new DoublePoint(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DoublePoint Delta(DoublePoint point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DoublePoint Delta(Double count)
		{
			return this - new DoublePoint(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DoublePoint Delta(Double x, Double y)
		{
			return this - new DoublePoint(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return X >= 0 && Y >= 0;
		}
	}

	public struct DecimalPoint
	{
		public static DecimalPoint operator +(DecimalPoint point1, DecimalPoint point2)
		{
			return new DecimalPoint(point1.X + point2.X, point1.Y + point2.Y);
		}
		public static DecimalPoint operator -(DecimalPoint point1, DecimalPoint point2)
		{
			return new DecimalPoint(point1.X - point2.X, point1.Y - point2.Y);
		}
		public static DecimalPoint operator *(DecimalPoint point1, DecimalPoint point2)
		{
			return new DecimalPoint(point1.X * point2.X, point1.Y * point2.Y);
		}
		public static DecimalPoint operator /(DecimalPoint point1, DecimalPoint point2)
		{
			return new DecimalPoint(point1.X / point2.X, point1.Y / point2.Y);
		}
		public static DecimalPoint operator %(DecimalPoint point1, DecimalPoint point2)
		{
			return new DecimalPoint(point1.X % point2.X, point1.Y % point2.Y);
		}

		public readonly Decimal X;
		public readonly Decimal Y;

		public DecimalPoint(Decimal x, Decimal y)
		{
			X = x;
			Y = y;
		}

		public static readonly DecimalPoint Zero = new DecimalPoint(0, 0);
		public static readonly DecimalPoint One = new DecimalPoint(1, 1);
		public static readonly DecimalPoint Minimum = new DecimalPoint(Decimal.MinValue, Decimal.MinValue);
		public static readonly DecimalPoint Maximum = new DecimalPoint(Decimal.MaxValue, Decimal.MaxValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DecimalPoint Offset(DecimalPoint point)
		{
			return this + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DecimalPoint Offset(Decimal x, Decimal y)
		{
			return this + new DecimalPoint(x, y);
		}

		public DecimalPoint Offset(PointOffset offset, Decimal count = 1)
		{
			return offset switch
			{
				PointOffset.None => this,
				PointOffset.Up => (this - new DecimalPoint(0, count)),
				PointOffset.Down => (this + new DecimalPoint(0, count)),
				PointOffset.Left => (this - new DecimalPoint(count, 0)),
				PointOffset.Right => (this + new DecimalPoint(count, 0)),
				PointOffset.UpLeft => (this - new DecimalPoint(count, count)),
				PointOffset.DownLeft => (this + new DecimalPoint(0, count) - new DecimalPoint(count, 0)),
				PointOffset.UpRight => (this - new DecimalPoint(0, count) + new DecimalPoint(count, 0)),
				PointOffset.DownRight => (this + new DecimalPoint(count, count)),
				_ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DecimalPoint Delta(DecimalPoint point)
		{
			return this - point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DecimalPoint Delta(Decimal count)
		{
			return this - new DecimalPoint(count, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DecimalPoint Delta(Decimal x, Decimal y)
		{
			return this - new DecimalPoint(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsPositive()
		{
			return X >= 0 && Y >= 0;
		}
	}

}