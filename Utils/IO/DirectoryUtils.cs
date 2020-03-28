// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Common_Library.Workstation;

namespace Common_Library.Utils.IO
{
    [Flags]
    public enum FileType
    {
        None = 0,
        Directories = 1,
        Files = 2,
        All = 3,
    };
    
    public static class DirectoryUtils
    {
        public static void CreateDirectory(String path)
        {
            CreateDirectory(path, out _);
        }

        public static void CreateDirectory(String path, out LongPath.DirectoryInfo directoryInfo)
        {
            if (LongPath.Directory.Exists(path))
            {
                directoryInfo = new LongPath.DirectoryInfo(path);
                return;
            }

            directoryInfo = LongPath.Directory.CreateDirectory(path);
        }

        public static Boolean TryCreateDirectory(String path, PathAction remove = PathAction.Standart)
        {
            return TryCreateDirectory(path, remove, out _);
        }

        public static Boolean TryCreateDirectory(String path, out LongPath.DirectoryInfo directoryInfo)
        {
            return TryCreateDirectory(path, PathAction.Standart, out directoryInfo);
        }

        public static Boolean TryCreateDirectory(String path, PathAction remove, out LongPath.DirectoryInfo directoryInfo)
        {
            directoryInfo = null;
            try
            {
                if (LongPath.Directory.Exists(path))
                {
                    return true;
                }

                CreateDirectory(path, out directoryInfo);

                return LongPath.Directory.Exists(path);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                try
                {
                    switch (remove)
                    {
                        case PathAction.Standart:
                            if (LongPath.Directory.GetFiles(path).Length == 0 &&
                                LongPath.Directory.GetDirectories(path).Length == 0)
                            {
                                LongPath.Directory.Delete(path, false);
                            }

                            break;
                        case PathAction.Force:
                            LongPath.Directory.Delete(path, true);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
        private struct WIN32_FIND_DATA
        {
            public UInt32 dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public UInt32 nFileSizeHigh;
            public UInt32 nFileSizeLow;
            public UInt32 dwReserved0;
            public UInt32 dwReserved1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public String cFileName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public String cAlternateFileName;

            public UInt32 dwFileType;
            public UInt32 dwCreatorType;
            public UInt16 wFinderFlags;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern Boolean FindClose(IntPtr hFindFile);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr FindFirstFileW(String lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern Boolean FindNextFileW(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

        public const String AnySearchPattern = ".*";

        public static IEnumerable<String> GetDirectories(String path, Boolean recursive)
        {
            return GetDirectories(path, AnySearchPattern, recursive);
        }

        public static IEnumerable<String> GetDirectories(String path, String searchPattern = AnySearchPattern, Boolean recursive = false)
        {
            return GetEntries(path, searchPattern, recursive, FileType.Directories);
        }

        public static IEnumerable<String> GetFiles(String path, Boolean recursive)
        {
            return GetFiles(path, AnySearchPattern, recursive);
        }
        
        public static IEnumerable<String> GetFiles(String path, String searchPattern = AnySearchPattern, Boolean recursive = false)
        {
            return GetEntries(path, searchPattern, recursive, FileType.Files);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void CheckErrors(String path)
        {
            if (path.Trim() == String.Empty)
            {
                throw new ArgumentException();
            }

            if (!LongPath.Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }

            try
            {
                LongPath.FileInfo fi = new LongPath.FileInfo(path);
                if (fi.Length > 0)
                {
                    throw new IOException();
                }
            }
            catch
            {
                // ignored
            }
        }

        public static IEnumerable<String> GetEntries([NotNull] String path, Boolean recursive, FileType type = FileType.All)
        {
            return GetEntries(path, AnySearchPattern, recursive, type);
        }

        public static IEnumerable<String> GetEntries([NotNull] String path, [NotNull][JetBrains.Annotations.RegexPattern] String searchPattern = AnySearchPattern, Boolean recursive = false, FileType type = FileType.All)
        {
            return GetEntries(path, new Regex(String.IsNullOrEmpty(searchPattern) ? ".*" : searchPattern), recursive, type);
        }

        public static IEnumerable<String> GetEntries([NotNull] String path, Regex regex, Boolean recursive = false, FileType type = FileType.All)
        {
            if (type == FileType.None)
            {
                yield break;
            }
            
            CheckErrors(path);
            if (path.Last() != '\\')
            {
                path += "\\";
            }

            IntPtr handle = FindFirstFileW(path + "*", out WIN32_FIND_DATA data);

            try
            {
                if ((Environment.Is64BitProcess ? handle.ToInt64() : handle.ToInt32()) == -1)
                {
                    yield break;
                }
                
                do
                {
                    if (data.cFileName == "." || data.cFileName == "..")
                    {
                        continue;
                    }
                    
                    if ((data.dwFileAttributes & (UInt32) FileAttributes.Directory) == (UInt32) FileAttributes.Directory)
                    {
                        if (recursive)
                        {
                            foreach (String entry in GetEntries(path + data.cFileName, regex, true, type))
                            {
                                yield return entry;
                            }
                        }

                        if (type.HasFlag(FileType.Directories) && regex.IsMatch(path + data.cFileName))
                        {
                            yield return path + data.cFileName;
                        }
                    }
                    else
                    {
                        if (type.HasFlag(FileType.Files) && regex.IsMatch(path + data.cFileName))
                        {
                            yield return path + data.cFileName;
                        }
                    }
                    
                    
                } while (FindNextFileW(handle, out data));
            }
            finally
            {
                FindClose(handle);
            }
        }
    }
}