// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Exceptions
{
    public class AlreadyInitializedException : InitializeException
    {
        public AlreadyInitializedException(String message = null)
            : base(message)
        {
        }
    }

    public class NotInitializedException : InitializeException
    {
        public NotInitializedException(String message = null)
            : base(message)
        {
        }
    }
    
    public class InitializeException : Exception
    {
        public InitializeException(String message = null)
            : base(message)
        {
        }
    }
}