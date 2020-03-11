// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Exceptions.Enum
{
    public class NotEnumTypeException : NotFlagsEnumTypeException
    {
        public NotEnumTypeException(String message = null)
            : base(message)
        {
        }
    }

    public class NotEnumTypeException<T> : NotFlagsEnumTypeException<T>
    {
        public NotEnumTypeException(String message = null)
            : base(message ?? $"{nameof(T)} must be an enum type.")
        {
        }
    }
}