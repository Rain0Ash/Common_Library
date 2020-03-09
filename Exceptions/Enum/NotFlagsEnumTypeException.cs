// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Exceptions.Enum
{
    public class NotFlagsEnumTypeException : ArgumentException
    {
        public NotFlagsEnumTypeException(String message = null)
            : base(message)
        {
        }
    }
    
    public class NotFlagsEnumTypeException<T> : NotFlagsEnumTypeException
    {
        public NotFlagsEnumTypeException(String message = null)
            : base(message ?? $"{nameof(T)} must be an flags enum type.")
        {
        }
    }
}