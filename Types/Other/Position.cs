// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Runtime.CompilerServices;
using Common_Library.Utils.Math;

namespace Common_Library.Types.Other
{
    [Flags]
    public enum PositionOffset
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
    
    public struct Position<T> where T : unmanaged, IComparable
    {
        public static Position<T> operator +(Position<T> pos1, Position<T> pos2)
        {
            return new Position<T>(MathUnsafe.Add(pos1.X, pos2.X), MathUnsafe.Add(pos1.Y, pos2.Y));
        }
        
        public static Position<T> operator -(Position<T> pos1, Position<T> pos2)
        {
            return new Position<T>(MathUnsafe.Substract(pos1.X, pos2.X), MathUnsafe.Substract(pos1.Y, pos2.Y));
        }
        
        public static Position<T> operator *(Position<T> pos1, Position<T> pos2)
        {
            return new Position<T>(MathUnsafe.Multiply(pos1.X, pos2.X), MathUnsafe.Multiply(pos1.Y, pos2.Y));
        }
        
        public static Position<T> operator /(Position<T> pos1, Position<T> pos2)
        {
            return new Position<T>(MathUnsafe.Divide(pos1.X, pos2.X), MathUnsafe.Divide(pos1.Y, pos2.Y));
        }

        public static Position<T> operator %(Position<T> pos1, Position<T> pos2)
        {
            return new Position<T>(MathUnsafe.Modulo(pos1.X, pos2.X), MathUnsafe.Modulo(pos1.Y, pos2.Y));
        }
        
        public readonly T X;
        public readonly T Y;
        
        public static readonly Position<T> Zero = new Position<T>(0, 0);

        public Position(T x, T y)
        {
            X = x;
            Y = y;
        }
        
        public Position(T x, Int32 y)
        {
            X = x;
            Y = (T)(Object)y;
        }
        
        public Position(Int32 x, T y)
        {
            X = (T)(Object)x;
            Y = y;
        }

        public Position(Int32 x, Int32 y)
        {
            X = (T)(Object)x;
            Y = (T)(Object)y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Position<T> Offset(Position<T> position)
        {
            return this + position;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Position<T> Offset(T x, T y)
        {
            return this + new Position<T>(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Position<T> Offset(PositionOffset offset)
        {
            return Offset(offset, (T)(Object)1);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Position<T> Offset(PositionOffset offset, T count)
        {
            return offset switch
            {
                PositionOffset.None => this,
                PositionOffset.Up => (this - new Position<T>(0, count)),
                PositionOffset.Down => (this + new Position<T>(0, count)),
                PositionOffset.Left => (this - new Position<T>(count, 0)),
                PositionOffset.Right => (this + new Position<T>(count, 0)),
                PositionOffset.UpLeft => (this - new Position<T>(count, count)),
                PositionOffset.DownLeft => (this + new Position<T>(MathUnsafe.Negative(count), count)),
                PositionOffset.UpRight => (this - new Position<T>(MathUnsafe.Negative(count), count)),
                PositionOffset.DownRight => (this + new Position<T>(count, count)),
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