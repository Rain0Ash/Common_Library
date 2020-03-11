// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Linq;

// ReSharper disable UnusedMember.Global

namespace Common_Library.LongPath
{
    using SearchOption = System.IO.SearchOption;
    using IOException = System.IO.IOException;

    public class DirectoryInfo : FileSystemInfo
    {
        public override System.IO.FileSystemInfo SystemInfo
        {
            get
            {
                return SysDirectoryInfo;
            }
        }

        private System.IO.DirectoryInfo SysDirectoryInfo
        {
            get
            {
                return new System.IO.DirectoryInfo(FullPath);
            }
        }

        public override Boolean Exists
        {
            get
            {
                if (StateType == State.Uninitialized)
                {
                    Refresh();
                }

                return StateType == State.Initialized &&
                       (Data.FileAttributes & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory;
            }
        }

        public override String Name { get; }

        public DirectoryInfo Parent
        {
            get
            {
                String fullPath = FullPath;
                if (fullPath.Length > 3 && fullPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                {
                    fullPath = FullPath.Substring(0, FullPath.Length - 1);
                }

                String directoryName = Path.GetDirectoryName(fullPath);
                return directoryName == null ? null : new DirectoryInfo(directoryName);
            }
        }

        public DirectoryInfo Root
        {
            get
            {
                Int32 rootLength = Path.GetRootLength(FullPath);
                String str = FullPath.Substring(0, rootLength - (Common.IsPathUnc(FullPath) ? 1 : 0));
                return new DirectoryInfo(str);
            }
        }

        public DirectoryInfo(String path)
        {
            OriginalPath = path ?? throw new ArgumentNullException(nameof(path));
            FullPath = Path.GetFullPath(path);
            Name = OriginalPath.Length != 2 || OriginalPath[1] != ':' ? GetDirName(FullPath) : ".";
        }

        public void Create()
        {
            Directory.CreateDirectory(FullPath);
        }

        public DirectoryInfo CreateSubdirectory(String path)
        {
            String newDir = Path.Combine(FullPath, path);
            String newFullPath = Path.GetFullPath(newDir);
            if (String.Compare(FullPath, 0, newFullPath, 0, FullPath.Length, StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new ArgumentException(@"Invalid subpath", path);
            }

            Directory.CreateDirectory(newDir);
            return new DirectoryInfo(newDir);
        }

        public override void Delete()
        {
            Directory.Delete(FullPath);
        }

        public void Delete(Boolean recursive)
        {
            Directory.Delete(FullPath, recursive);
        }

        public IEnumerable<DirectoryInfo> EnumerateDirectories(String searchPattern)
        {
            if (Common.IsRunningOnMono())
            {
                return SysDirectoryInfo.EnumerateDirectories(searchPattern).Select(s => new DirectoryInfo(s.FullName));
            }

            return Directory.EnumerateFileSystemEntries(FullPath, searchPattern, true, false, SearchOption.TopDirectoryOnly)
                .Select(directory => new DirectoryInfo(directory));
        }

        public IEnumerable<DirectoryInfo> EnumerateDirectories(String searchPattern, SearchOption searchOption)
        {
            if (Common.IsRunningOnMono())
            {
                return SysDirectoryInfo.EnumerateDirectories(searchPattern, searchOption).Select(s => new DirectoryInfo(s.FullName));
            }

            return Directory.EnumerateFileSystemEntries(FullPath, searchPattern, true, false, searchOption)
                .Select(directory => new DirectoryInfo(directory));
        }

        public IEnumerable<FileInfo> EnumerateFiles()
        {
            return Directory.EnumerateFiles(FullPath).Select(e => new FileInfo(e));
        }

        public IEnumerable<FileInfo> EnumerateFiles(String searchPattern)
        {
            return Common.IsRunningOnMono()
                ? SysDirectoryInfo.EnumerateFiles(searchPattern).Select(s => new FileInfo(s.FullName))
                : Directory.EnumerateFileSystemEntries(FullPath, searchPattern, false, true, SearchOption.TopDirectoryOnly)
                    .Select(e => new FileInfo(e));
        }

        public IEnumerable<FileInfo> EnumerateFiles(String searchPattern, SearchOption searchOption)
        {
            return Common.IsRunningOnMono()
                ? SysDirectoryInfo.EnumerateFiles(searchPattern, searchOption).Select(s => new FileInfo(s.FullName))
                : Directory.EnumerateFileSystemEntries(FullPath, searchPattern, false, true, searchOption).Select(e => new FileInfo(e));
        }

        public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos()
        {
            return
                Directory.EnumerateFileSystemEntries(FullPath)
                    .Select(e => Directory.Exists(e) ? (FileSystemInfo) new DirectoryInfo(e) : (FileSystemInfo) new FileInfo(e));
        }

        public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(String searchPattern)
        {
            if (Common.IsRunningOnMono())
            {
                return SysDirectoryInfo.EnumerateFileSystemInfos(searchPattern)
                    .Select(e => System.IO.Directory.Exists(e.FullName)
                        ? (FileSystemInfo) new DirectoryInfo(e.FullName)
                        : (FileSystemInfo) new FileInfo(e.FullName));
            }

            return Directory.EnumerateFileSystemEntries(FullPath, searchPattern, true, true, SearchOption.TopDirectoryOnly)
                .Select(e => Directory.Exists(e) ? (FileSystemInfo) new DirectoryInfo(e) : (FileSystemInfo) new FileInfo(e));
        }

        public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(String searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFileSystemEntries(FullPath, searchPattern, searchOption)
                .Select(e => Directory.Exists(e) ? (FileSystemInfo) new DirectoryInfo(e) : (FileSystemInfo) new FileInfo(e));
        }

        private static String GetDirName(String fullPath)
        {
            if (fullPath.Length <= 3)
            {
                return fullPath;
            }

            String s = fullPath;
            if (s.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
                s = s.Substring(0, s.Length - 1);
            }

            return Path.GetFileName(s);
        }

        public void MoveTo(String destDirName)
        {
            if (Common.IsRunningOnMono())
            {
                SysDirectoryInfo.MoveTo(destDirName);
                return;
            }

            if (destDirName == null)
            {
                throw new ArgumentNullException(nameof(destDirName));
            }

            if (String.IsNullOrWhiteSpace(destDirName))
            {
                throw new ArgumentException(@"Empty filename", nameof(destDirName));
            }

            String fullDestDirName = Path.GetFullPath(destDirName);
            if (!fullDestDirName.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
                fullDestDirName += Path.DirectorySeparatorChar;
            }

            String fullSourcePath;
            if (FullPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
                fullSourcePath = FullPath;
            }
            else
            {
                fullSourcePath = FullPath + Path.DirectorySeparatorChar;
            }

            if (String.Compare(fullSourcePath, fullDestDirName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                throw new IOException("source and destination directories must be different");
            }

            String sourceRoot = Path.GetPathRoot(fullSourcePath);
            String destinationRoot = Path.GetPathRoot(fullDestDirName);

            if (String.Compare(sourceRoot, destinationRoot, StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new IOException("Source and destination directories must have same root");
            }

            File.Move(fullSourcePath, fullDestDirName);
        }

        public void Create(DirectorySecurity directorySecurity)
        {
            Directory.CreateDirectory(FullPath, directorySecurity);
        }

        public DirectoryInfo CreateSubdirectory(String path, DirectorySecurity directorySecurity)
        {
            String newDir = Path.Combine(FullPath, path);
            String newFullPath = Path.GetFullPath(newDir);
            if (String.Compare(FullPath, 0, newFullPath, 0, FullPath.Length, StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new ArgumentException(@"Invalid subpath", path);
            }

            Directory.CreateDirectory(newDir, directorySecurity);
            return new DirectoryInfo(newDir);
        }

        public IEnumerable<DirectoryInfo> EnumerateDirectories()
        {
            return Common.IsRunningOnMono()
                ? SysDirectoryInfo.EnumerateDirectories().Select(s => new DirectoryInfo(s.FullName))
                : Directory.EnumerateFileSystemEntries(FullPath, "*", true, false, SearchOption.TopDirectoryOnly)
                    .Select(directory => new DirectoryInfo(directory));
        }

        public DirectorySecurity GetAccessControl()
        {
            return Directory.GetAccessControl(FullPath);
        }

        public DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            return Directory.GetAccessControl(FullPath, includeSections);
        }

        public DirectoryInfo[] GetDirectories()
        {
            return Directory.GetDirectories(FullPath).Select(path => new DirectoryInfo(path)).ToArray();
        }

        public DirectoryInfo[] GetDirectories(String searchPattern)
        {
            return Directory.GetDirectories(FullPath, searchPattern).Select(path => new DirectoryInfo(path)).ToArray();
        }

        public DirectoryInfo[] GetDirectories(String searchPattern, SearchOption searchOption)
        {
            return Directory.GetDirectories(FullPath, searchPattern, searchOption).Select(path => new DirectoryInfo(path)).ToArray();
        }

        public FileInfo[] GetFiles(String searchPattern)
        {
            return Directory.GetFiles(FullPath, searchPattern).Select(path => new FileInfo(path)).ToArray();
        }

        public FileInfo[] GetFiles(String searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(FullPath, searchPattern, searchOption).Select(path => new FileInfo(path)).ToArray();
        }

        public FileInfo[] GetFiles()
        {
            if (!Common.IsRunningOnMono())
            {
                return Directory.EnumerateFileSystemEntries(FullPath, "*", false, true, SearchOption.TopDirectoryOnly)
                    .Select(path => new FileInfo(path)).ToArray();
            }

            System.IO.FileInfo[] files = SysDirectoryInfo.GetFiles();
            FileInfo[] ret = new FileInfo[files.Length];
            for (Int32 index = 0; index < files.Length; index++)
            {
                ret[index] = new FileInfo(files[index].FullName);
            }

            return ret;
        }

        public FileSystemInfo[] GetFileSystemInfos(String searchPattern)
        {
            if (!Common.IsRunningOnMono())
            {
                return Directory
                    .EnumerateFileSystemEntries(FullPath, searchPattern, true, true, SearchOption.TopDirectoryOnly)
                    .Select(e =>
                        Directory.Exists(e) ? (FileSystemInfo) new DirectoryInfo(e) : (FileSystemInfo) new FileInfo(e))
                    .ToArray();
            }

            {
                System.IO.FileSystemInfo[] sysInfos = SysDirectoryInfo.GetFileSystemInfos(searchPattern);
                FileSystemInfo[] fsis = new FileSystemInfo[sysInfos.Length];
                for (Int32 i = 0; i < sysInfos.Length; i++)
                {
                    String e = sysInfos[i].FullName;
                    fsis[i] = Directory.Exists(e)
                        ? new DirectoryInfo(e)
                        : (FileSystemInfo) new FileInfo(e);
                }

                return fsis;
            }
        }

        public FileSystemInfo[] GetFileSystemInfos(String searchPattern, SearchOption searchOption)
        {
            if (Common.IsRunningOnMono())
            {
                return SysDirectoryInfo.GetFileSystemInfos(searchPattern, searchOption).Select(s => s.FullName).Select(e =>
                    Directory.Exists(e) ? (FileSystemInfo) new DirectoryInfo(e) : (FileSystemInfo) new FileInfo(e)).ToArray();
            }

            return Directory.EnumerateFileSystemEntries(FullPath, searchPattern, true, true, searchOption)
                .Select(e => Directory.Exists(e) ? (FileSystemInfo) new DirectoryInfo(e) : (FileSystemInfo) new FileInfo(e)).ToArray();
        }

        public FileSystemInfo[] GetFileSystemInfos()
        {
            if (!Common.IsRunningOnMono())
            {
                return Directory.EnumerateFileSystemEntries(FullPath, "*", true, true, SearchOption.TopDirectoryOnly)
                    .Select(e =>
                        Directory.Exists(e) ? (FileSystemInfo) new DirectoryInfo(e) : (FileSystemInfo) new FileInfo(e))
                    .ToArray();
            }


            if (!Common.IsRunningOnMono())
            {
                return Directory
                    .EnumerateFileSystemEntries(FullPath, "*", true, true, SearchOption.TopDirectoryOnly)
                    .Select(e =>
                        Directory.Exists(e)
                            ? (FileSystemInfo) new DirectoryInfo(e)
                            : (FileSystemInfo) new FileInfo(e)).ToArray();
            }

            System.IO.FileSystemInfo[] sysInfos = SysDirectoryInfo.GetFileSystemInfos();
            FileSystemInfo[] fsis = new FileSystemInfo[sysInfos.Length];
            for (Int32 i = 0; i < sysInfos.Length; i++)
            {
                String e = sysInfos[i].FullName;
                fsis[i] = Directory.Exists(e)
                    ? new DirectoryInfo(e)
                    : (FileSystemInfo) new FileInfo(e);
            }

            return fsis;
        }

        public void SetAccessControl(DirectorySecurity directorySecurity)
        {
            Directory.SetAccessControl(FullPath, directorySecurity);
        }

        public override String ToString()
        {
            return DisplayPath;
        }
    }

    public static class StringExtensions
    {
        public static Boolean EndsWith(this String text, Char value)
        {
            return EndsWith(text, new[] {value});
        }

        public static Boolean EndsWith(this String text, IEnumerable<Char> value)
        {
            value = value as Char[] ?? value?.ToArray();
            if (String.IsNullOrEmpty(text) || value?.Any() != true)
            {
                return false;
            }

            return value.Contains(text[text.Length - 1]);
        }
    }
}