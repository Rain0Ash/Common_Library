// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Common_Library.Utils.IO
{
    public static class DirectoryUtils
    { public static Boolean TryCreateDirectory(String path, PathAction remove = PathAction.Standart)
        {
            return TryCreateDirectory(path, remove, out _);
        }

        public static Boolean TryCreateDirectory(String path, out DirectoryInfo directoryInfo)
        {
            return TryCreateDirectory(path, PathAction.Standart, out directoryInfo);
        }

        public static Boolean TryCreateDirectory(String path, PathAction remove, out DirectoryInfo directoryInfo)
        {
            directoryInfo = null;
            
            try
            {
                if (Directory.Exists(path))
                {
                    return true;
                }

                directoryInfo = Directory.CreateDirectory(path);

                return Directory.Exists(path);
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
                            if (GetFiles(path).All(file => file.Equals("desktop.ini", StringComparison.OrdinalIgnoreCase)) &&
                                !GetDirectories(path).Any())
                            {
                                Directory.Delete(path, true);
                                $"{path}\nExist: {Directory.Exists(path)}".ToMessageBox();
                            }

                            break;
                        case PathAction.Force:
                            Directory.Delete(path, true);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public static DirectoryInfo LatestExist(String path)
        {
            return LatestExist(new DirectoryInfo(path));
        }

        public static DirectoryInfo LatestExist(this FileSystemInfo info)
        {
            return info switch
            {
                DirectoryInfo directory => LatestExist(directory),
                FileInfo file => LatestExist(file),
                _ => throw new NotSupportedException($"{nameof(info)} not supported {info.GetType()}")
            };
        }
        
        public static DirectoryInfo LatestExist(this FileInfo info)
        {
            return LatestExist(info.Directory);
        }
        
        public static DirectoryInfo LatestExist(this DirectoryInfo info)
        {
            while (!info.Exists)
            {
                DirectoryInfo parent = info.Parent;
                if (parent == null)
                {
                    break;
                }
                
                info = parent;
            }

            return info.Exists ? info : null;
        }

        public static Boolean CheckPermissions(String path, FileSystemRights access, Boolean? error = false)
        {
            return CheckPermissions(new DirectoryInfo(path), access, error);
        }

        public static Boolean CheckPermissions(this DirectoryInfo info, FileSystemRights access, Boolean? error = false)
        {
            try
            {
                return info.LatestExist()
                    .GetAccessControl()
                    .GetAccessRules(true, true, typeof(NTAccount))
                    .Cast<FileSystemAccessRule>()
                    .Any(rule => (rule.FileSystemRights & access) > 0);
            }           
            catch (Exception)
            {
                if (error == null)
                {
                    throw;
                }
                
                return error.Value;
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

        public static IEnumerable<String> GetDirectories([NotNull] String path, Boolean recursive = false)
        {
            return GetEntries(path, recursive, PathType.Folder);
        }

        public static IEnumerable<String> GetDirectories([NotNull] String path, [NotNull][JetBrains.Annotations.RegexPattern] String searchPattern, Boolean recursive = false)
        {
            return GetEntries(path, searchPattern, recursive, PathType.Folder);
        }
        
        public static IEnumerable<String> GetDirectories([NotNull] String path, Regex regex, Boolean recursive = false)
        {
            return GetEntries(path, regex, recursive, PathType.Folder);
        }

        public static IEnumerable<String> GetFiles([NotNull] String path, Boolean recursive = false)
        {
            return GetEntries(path, recursive, PathType.File);
        }

        public static IEnumerable<String> GetFiles([NotNull] String path, [NotNull][JetBrains.Annotations.RegexPattern] String searchPattern, Boolean recursive = false)
        {
            return GetEntries(path, searchPattern, recursive, PathType.File);
        }
        
        public static IEnumerable<String> GetFiles([NotNull] String path, Regex regex, Boolean recursive = false)
        {
            return GetEntries(path, regex, recursive, PathType.File);
        }

        public static IEnumerable<String> GetEntries([NotNull] String path, [NotNull][JetBrains.Annotations.RegexPattern]
            String searchPattern, Boolean recursive = false, PathType type = PathType.All)
        {
            return GetEntries(path, new Regex(String.IsNullOrEmpty(searchPattern) ? AnySearchPattern : searchPattern), recursive, type);
        }

        public static IEnumerable<String> GetEntries([NotNull] String path, Boolean recursive = false, PathType type = PathType.All)
        {
            if (type == PathType.None)
            {
                yield break;
            }

            if (path.Last() != '\\')
            {
                path += "\\";
            }

            if (!PathUtils.IsValidPath(path, PathType.Folder, PathStatus.Exist))
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
                            foreach (String entry in GetEntries(entryname, true, type))
                            {
                                yield return entry;
                            }
                        }
                        
                        if (type.HasFlag(PathType.Folder))
                        {
                            yield return entryname;
                        }
                    }
                    else
                    {
                        if (type.HasFlag(PathType.File))
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

        public static IEnumerable<String> GetEntries([NotNull] String path, Regex regex, Boolean recursive = false, PathType type = PathType.All)
        {
            return GetEntries(path, recursive, type).Where(file => regex.IsMatch(file));
        }
    }
}