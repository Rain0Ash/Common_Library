// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Encapsulates information relating to exceptions thrown while making calls to the console via the Win32 API.
    /// </summary>
    public sealed class ConsoleAccessException : Exception
    {
        /// <summary>
        /// Encapsulates information relating to exceptions thrown while making calls to the console via the Win32 API.
        /// </summary>
        public ConsoleAccessException()
            : base("Color conversion failed because a handle to the actual windows console was not found.")
        {
        }
    }
}
