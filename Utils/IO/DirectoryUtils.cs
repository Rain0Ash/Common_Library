// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace Common_Library.Utils.IO
{
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
                    //ignored
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
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
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

        public static IEnumerable<String> GetDirectories([NotNull] String path, Boolean recursive)
        {
            return GetDirectories(path, AnySearchPattern, recursive);
        }

        public static IEnumerable<String> GetDirectories([NotNull] String path, [NotNull][JetBrains.Annotations.RegexPattern] String searchPattern = AnySearchPattern, Boolean recursive = false)
        {
            return GetEntries(path, searchPattern, recursive, PathType.Folder);
        }

        public static IEnumerable<String> GetFiles([NotNull] String path, Boolean recursive)
        {
            return GetFiles(path, AnySearchPattern, recursive);
        }

        public static IEnumerable<String> GetFiles([NotNull] String path, [NotNull][JetBrains.Annotations.RegexPattern] String searchPattern = AnySearchPattern, Boolean recursive = false)
        {
            return GetEntries(path, searchPattern, recursive, PathType.File);
        }
        
        public static IEnumerable<String> GetEntries([NotNull] String path, Boolean recursive, PathType type = PathType.All)
        {
            return GetEntries(path, AnySearchPattern, recursive, type);
        }

        public static IEnumerable<String> GetEntries([NotNull] String path, [NotNull][JetBrains.Annotations.RegexPattern]
            String searchPattern = AnySearchPattern, Boolean recursive = false, PathType type = PathType.All)
        {
            return GetEntries(path, new Regex(String.IsNullOrEmpty(searchPattern) ? ".*" : searchPattern), recursive, type);
        }

        public static IEnumerable<String> GetEntries([NotNull] String path, Regex regex, Boolean recursive = false, PathType type = PathType.All)
        {
            if (type == PathType.None)
            {
                yield break;
            }

            if (path.Last() != '\\')
            {
                path += "\\";
            }

            if (!PathUtils.IsValidPath(path, type, PathStatus.Exist))
            {
                yield break;
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

                    String entryname = path + data.cFileName;

                    if ((data.dwFileAttributes & (UInt32) FileAttributes.Directory) == (UInt32) FileAttributes.Directory)
                    {
                        if (recursive)
                        {
                            foreach (String entry in GetEntries(entryname, regex, true, type))
                            {
                                yield return entry;
                            }
                        }
                        
                        if (type.HasFlag(PathType.Folder) && regex.IsMatch(entryname))
                        {
                            yield return entryname;
                        }
                    }
                    else
                    {
                        if (type.HasFlag(PathType.File) && regex.IsMatch(entryname))
                        {
                            yield return entryname;
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