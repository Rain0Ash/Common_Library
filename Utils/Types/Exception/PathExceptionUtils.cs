// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.IO;

namespace Common_Library.Utils
{
    public static class PathExceptionUtils
    {
        /// <summary>
        /// Extracts the path of the directory in the DirectoryNotFoundException
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>The path of the directory</returns>
        public static String Path(this DirectoryNotFoundException ex)
        {
            return GetPathFromMessage(ex.Message);
        }

        /// <summary>
        /// Extracts the path of the directory in the UnauthorizedAccessException
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>The path of the directory</returns>
        public static String Path(this UnauthorizedAccessException ex)
        {
            return GetPathFromMessage(ex.Message);
        }

        private static String GetPathFromMessage(String exMessage)
        {
            Int32 startIndex = exMessage.IndexOf('\'') + 1;
            Int32 endIndex = exMessage.LastIndexOf('\'');
            Int32 length = endIndex - startIndex;

            // Here I assert that atleast 2 apostrophe exist in the message
            return length < 0 ? String.Empty : exMessage.Substring(startIndex, length);
        }
    }
}