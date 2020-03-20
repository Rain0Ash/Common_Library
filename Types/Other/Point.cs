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
    
    public struct Point<T> where T : unmanaged, IComparable
    {
        public static Point<T> operator +(Point<T> pos1, Point<T> pos2)
        {
            return new Point<T>(MathUnsafe.Add(pos1.X, pos2.X), MathUnsafe.Add(pos1.Y, pos2.Y));
        }
        
        public static Point<T> operator -(Point<T> pos1, Point<T> pos2)
        {
            return new Point<T>(MathUnsafe.Substract(pos1.X, pos2.X), MathUnsafe.Substract(pos1.Y, pos2.Y));
        }
        
        public static Point<T> operator *(Point<T> pos1, Point<T> pos2)
        {
            return new Point<T>(MathUnsafe.Multiply(pos1.X, pos2.X), MathUnsafe.Multiply(pos1.Y, pos2.Y));
        }
        
        public static Point<T> operator /(Point<T> pos1, Point<T> pos2)
        {
            return new Point<T>(MathUnsafe.Divide(pos1.X, pos2.X), MathUnsafe.Divide(pos1.Y, pos2.Y));
        }

        public static Point<T> operator %(Point<T> pos1, Point<T> pos2)
        {
            return new Point<T>(MathUnsafe.Modulo(pos1.X, pos2.X), MathUnsafe.Modulo(pos1.Y, pos2.Y));
        }
        
        public readonly T X;
        public readonly T Y;
        
        public static readonly Point<T> Zero = new Point<T>(0, 0);
        public static readonly Point<T> One = new Point<T>(1, 1);

        public Point(T x, T y)
        {
            X = x;
            Y = y;
        }
        
        public Point(T x, Int32 y)
        {
            X = x;
            Y = (T)(Object)y;
        }
        
        public Point(Int32 x, T y)
        {
            X = (T)(Object)x;
            Y = y;
        }

        public Point(Int32 x, Int32 y)
        {
            X = (T)(Object)x;
            Y = (T)(Object)y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point<T> Offset(Point<T> point)
        {
            return this + point;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point<T> Offset(T x, T y)
        {
            return this + new Point<T>(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point<T> Offset(PointOffset offset)
        {
            return Offset(offset, (T)(Object)1);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point<T> Offset(PointOffset offset, T count)
        {
            return offset switch
            {
                PointOffset.None => this,
                PointOffset.Up => (this - new Point<T>(0, count)),
                PointOffset.Down => (this + new Point<T>(0, count)),
                PointOffset.Left => (this - new Point<T>(count, 0)),
                PointOffset.Right => (this + new Point<T>(count, 0)),
                PointOffset.UpLeft => (this - new Point<T>(count, count)),
                PointOffset.DownLeft => (this + new Point<T>(MathUnsafe.Negative(count), count)),
                PointOffset.UpRight => (this - new Point<T>(MathUnsafe.Negative(count), count)),
                PointOffset.DownRight => (this + new Point<T>(count, count)),
                _ => throw new ArgumentOutOfRangeException(nameof(offset), offset, null)
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Boolean IsPositive()
        {
            return MathUnsafe.GreaterEqual(X, default) && MathUnsafe.GreaterEqual(Y, default);
        }

        public override String ToString()
        {
            return $"X:{X}, Y:{Y}";
        }
    }
}