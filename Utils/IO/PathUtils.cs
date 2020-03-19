// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Common_Library.LongPath;
using Path = Common_Library.LongPath.Path;
using Directory = Common_Library.LongPath.Directory;

namespace Common_Library.Utils.IO
{
    [Flags]
    public enum PathType : Byte
    {
        None = 0,
        LocalFolder = 1,
        LocalFile = 2,
        LocalPath = 3,
        NetworkFolder = 4,
        Folder = 5,
        NetworkFile = 8,
        File = 10,
        NetworkPath = 12,
        All = 15
    }

    public enum PathStatus : Byte
    {
        All,
        Exist,
        NotExist
    }

    public enum PathAction : Byte
    {
        None,
        Standart,
        Force
    }

    public static class PathUtils
    {
        public static readonly Char[] Separators = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};

        public static String GetRelativePath(String path, String pathReference = null)
        {
            if (String.IsNullOrEmpty(path))
            {
                return null;
            }

            if (String.IsNullOrEmpty(pathReference))
            {
                pathReference = Directory.GetCurrentDirectory();
            }

            try
            {
                Uri pathUri = new Uri(path);
                Uri referenceUri = new Uri(pathReference + Path.DirectorySeparatorChar);
                return Uri.UnescapeDataString(
                    referenceUri.MakeRelativeUri(pathUri).ToString()
                        .Replace('/', Path.DirectorySeparatorChar));
            }
            catch (UriFormatException)
            {
                return null;
            }
        }

        public static String RemoveIllegalChars(String path)
        {
            String regexSearch = new String(Path.GetInvalidFileNameChars()) + new String(Path.GetInvalidPathChars());
            return Regex.Replace(path, $"[{Regex.Escape(regexSearch)}]", String.Empty);
        }

        public static String GetFullPath(String path)
        {
            try
            {
                return Path.GetFullPath(String.IsNullOrEmpty(path) ? "." : path);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static String ConvertToFolder(String path)
        {
            return path.EndsWith(Separators) ? path : $"{path}{Path.DirectorySeparatorChar}";
        }

        public static Boolean IsAbsolutePath(String path)
        {
            return IsValidPath(path) && !IsNetworkPath(path) && Path.IsPathRooted(path) &&
                   !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
        }

        public static PathType GetPathType(String path)
        {
            if (IsValidNetworkPath(path))
            {
                return Separators.Any(chr => path.EndsWith(chr.ToString())) ? PathType.NetworkFolder : PathType.NetworkFile;
            }

            if (IsValidPath(path, false))
            {
                return Separators.Any(chr => path.EndsWith(chr.ToString())) ? PathType.LocalFolder : PathType.LocalFile;
            }

            return PathType.None;
        }

        private static Boolean CheckValidPath(String path, PathType type)
        {
            return type switch
            {
                PathType.None => false,
                PathType.LocalFolder => IsValidFolderPath(path, false),
                PathType.LocalFile => IsValidFilePath(path, false),
                PathType.LocalPath => IsValidPath(path, false),
                PathType.NetworkFolder => IsValidNetworkFolderPath(path),
                PathType.Folder => IsValidFolderPath(path),
                PathType.LocalFile | PathType.NetworkFolder => (IsValidFilePath(path, false) || IsValidNetworkFolderPath(path)),
                PathType.Folder | PathType.LocalFile => (IsValidPath(path, false) || IsValidNetworkFolderPath(path)),
                PathType.NetworkFile => IsValidNetworkFilePath(path),
                PathType.LocalFolder | PathType.NetworkFile => (IsValidFolderPath(path, false) || IsValidNetworkFilePath(path)),
                PathType.File => IsValidFilePath(path),
                PathType.LocalFolder | PathType.LocalFile | PathType.NetworkFile => (IsValidPath(path, false) ||
                                                                                     IsValidNetworkFilePath(path)),
                PathType.NetworkPath => IsValidNetworkPath(path),
                PathType.Folder | PathType.NetworkFile => (IsValidFolderPath(path) || IsValidNetworkPath(path)),
                PathType.File | PathType.NetworkFolder => (IsValidFilePath(path) || IsValidNetworkPath(path)),
                PathType.All => IsValidPath(path),
                _ => IsValidPath(path)
            };
        }

        public static Boolean IsValidPath(String path, PathType type, PathStatus status = PathStatus.All)
        {
            if (!CheckValidPath(path, type))
            {
                return false;
            }

            return status switch
            {
                PathStatus.All => true,
                PathStatus.Exist => CheckExist(path),
                PathStatus.NotExist => !CheckExist(path),
                _ => false
            };
        }

        public static Boolean IsValidPath(String path, Boolean allowNetworkPath = true)
        {
            return GetFullPath(path) != null && (!IsNetworkPath(path) || allowNetworkPath && IsValidNetworkPath(path));
        }
        
        public static Boolean IsValidFolderPath(String path, Boolean allowNetworkPath = true)
        {
            return IsValidPath(path, allowNetworkPath) && Separators.Any(chr => path.EndsWith(chr.ToString()));
        }

        public static Boolean IsValidFilePath(String path, Boolean allowNetworkPath = true)
        {
            return IsValidPath(path, allowNetworkPath) && !Separators.Any(chr => path.EndsWith(chr.ToString()));
        }

        public static Boolean IsNetworkPath(String path)
        {
            return path.StartsWith("\\");
        }

        public static Boolean IsValidNetworkPath(String path)
        {
            try
            {
                return IsNetworkPath(path) && new Uri(path).IsUnc &&
                       Regex.IsMatch(path, @"^\\{2}[\w-]+(\\{1}(([\w-][\w-\s]*[\w-]+[$$]?)|([\w-][$$]?$)))+");
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Boolean IsValidNetworkFolderPath(String path)
        {
            return IsValidNetworkPath(path) && Separators.Any(chr => path.EndsWith(chr.ToString()));
        }

        public static Boolean IsValidNetworkFilePath(String path)
        {
            return IsValidNetworkPath(path) && !Separators.Any(chr => path.EndsWith(chr.ToString()));
        }

        public static Boolean IsExistAsFolder(String path)
        {
            return IsValidFolderPath(path) && Directory.Exists(path);
        }

        public static Boolean IsExistAsFile(String path)
        {
            return IsValidFilePath(path) && File.Exists(path);
        }

        private static Boolean CheckExist(String path)
        {
            return IsExistAsFolder(path) || IsExistAsFile(path);
        }

        public static Boolean IsExist(String path, PathType type = PathType.All)
        {
            return IsValidPath(path, type) && CheckExist(path);
        }

        public static String TrimPath(String path)
        {
            return path.Trim(Separators);
        }

        public static String TrimPathStart(String path)
        {
            return path.TrimStart(Separators);
        }

        public static String TrimPathEnd(String path)
        {
            return path.TrimEnd(Separators);
        }

        public static Boolean IsValidWebPath(String path)
        {
            return Uri.TryCreate(path, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}