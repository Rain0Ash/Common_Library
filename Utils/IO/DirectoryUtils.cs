// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        private enum EntryType
        {
            All = 0,
            Directories = 1,
            Files = 2
        };

        public static IEnumerable<String> EnumerateDirectories(String path, String searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            CheckErrors(path, searchPattern);
            List<String> retValue = new List<String>();
            Enumerate(path, searchPattern, searchOption, ref retValue, EntryType.Directories);
            return retValue;
        }

        public static IEnumerable<String> EnumerateFiles(String path, String searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            CheckErrors(path, searchPattern);
            List<String> retValue = new List<String>();
            Enumerate(path, searchPattern, searchOption, ref retValue, EntryType.Files);
            return retValue;
        }


        public static IEnumerable<String> EnumerateFileSystemEntries(String path, String searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            CheckErrors(path, searchPattern);
            List<String> retValue = new List<String>();
            Enumerate(path, searchPattern, searchOption, ref retValue, EntryType.All);
            return retValue;
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void CheckErrors(String path, String searchPattern)
        {
            if (path.Trim() == "")
            {
                throw new ArgumentException();
            }

            if (searchPattern == null)
            {
                throw new ArgumentNullException();
            }

            if (!Directory.Exists(path))
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

        private static void Enumerate(String path, String searchPattern, SearchOption searchOption, ref List<String> retValue, EntryType entryType)
        {
            if (path.Last() != '\\')
            {
                path += "\\";
            }

            AdjustSearchPattern(ref path, ref searchPattern);
            searchPattern = searchPattern.Replace("*.*", "*");
            Regex rx = new Regex(
                "^" + Regex.Escape(path) + Regex.Escape(searchPattern)
                    .Replace("\\*", ".*")
                    .Replace("\\?", ".") + "$",
                RegexOptions.IgnoreCase);
            IntPtr hFile = FindFirstFileW(path + "*", out WIN32_FIND_DATA findData);
            List<String> subDirs = new List<String>();
            if ((IntPtr.Size == 4 ? hFile.ToInt32() : hFile.ToInt64()) != -1)
            {
                do
                {
                    if (findData.cFileName == "." || findData.cFileName == "..")
                    {
                        continue;
                    }

                    if ((findData.dwFileAttributes & (UInt32) FileAttributes.Directory) == (UInt32) FileAttributes.Directory)
                    {
                        subDirs.Add(path + findData.cFileName);
                        if ((entryType == EntryType.Directories || entryType == EntryType.All) && rx.IsMatch(path + findData.cFileName))
                        {
                            retValue.Add(path + findData.cFileName);
                        }
                    }
                    else
                    {
                        if ((entryType == EntryType.Files || entryType == EntryType.All) && rx.IsMatch(path + findData.cFileName))
                        {
                            retValue.Add(path + findData.cFileName);
                        }
                    }
                } while (FindNextFileW(hFile, out findData));

                if (searchOption == SearchOption.AllDirectories)
                {
                    foreach (String subdir in subDirs)
                    {
                        Enumerate(subdir, searchPattern, searchOption, ref retValue, entryType);
                    }
                }
            }

            FindClose(hFile);
        }

        private static void AdjustSearchPattern(ref String path, ref String searchPattern)
        {
            if (path.Last() != '\\')
            {
                path += "\\";
            }

            if (searchPattern.Contains("\\"))
            {
                path = (path + searchPattern).Substring(0, (path + searchPattern).LastIndexOf("\\", StringComparison.Ordinal) + 1);
                searchPattern = searchPattern.Substring(searchPattern.IndexOf("\\", StringComparison.Ordinal) + 1);
            }

            if (searchPattern == "*.*")
            {
                searchPattern = "*";
            }
        }
    }
}