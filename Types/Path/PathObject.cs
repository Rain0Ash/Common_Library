// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Common_Library.Utils;
using JetBrains.Annotations;

namespace Common_Library.Types.Other
{
    public class PathObject
    {
        public static implicit operator String(PathObject path)
        {
            return path.ToString();
        }
        
        public String Path { get; }
        public PathType PathType { get; set; }
        
        public PathStatus PathStatus { get; set; }

        public event Handlers.EmptyHandler RecursiveChanged;
        private Boolean _recursive;

        public Boolean Recursive
        {
            get
            {
                return _recursive;
            }
            set
            {
                if (_recursive == value)
                {
                    return;
                }

                _recursive = value;
                RecursiveChanged?.Invoke();
            }
        }

        public event Handlers.EmptyHandler IconExistCheckChanged;
        private Boolean _iconExistCheck = true;

        public Boolean IconExistCheck
        {
            get
            {
                return _iconExistCheck;
            }
            set
            {
                if (_iconExistCheck == value)
                {
                    return;
                }
                
                _iconExistCheck = value;
                IconExistCheckChanged?.Invoke();
            }
        }

        public Image FolderImage { get; set; } = Images.Images.Lineal.Folder;
        public Image NotFolderImage { get; set; } = Images.Images.Lineal.NotFolder;
        public Image FileImage { get; set; } = Images.Images.Lineal.File;
        public Image NotFileImage { get; set; } = Images.Images.Lineal.NotFile;

        public Image GetPathTypeIcon
        {
            get
            {
                return PathUtils.GetPathType(Path) switch
                {
                    PathType.Folder => (!IconExistCheck || IsExistAsFolder() ? FolderImage : NotFolderImage),
                    PathType.LocalFolder => (!IconExistCheck || IsExistAsFolder() ? FolderImage : NotFolderImage),
                    PathType.NetworkFolder => (!IconExistCheck || IsExistAsFolder() ? FolderImage : NotFolderImage),
                    PathType.File => (!IconExistCheck || IsExistAsFile() ? FileImage : NotFileImage),
                    PathType.LocalFile => (!IconExistCheck || IsExistAsFile() ? FileImage : NotFileImage),
                    PathType.NetworkFile => (!IconExistCheck || IsExistAsFile() ? FileImage : NotFileImage),
                    _ => Images.Images.Basic.Null
                };
            }
        }

        public PathObject(String path, PathType type = PathType.All, PathStatus status = PathStatus.All)
        {
            if (!PathUtils.IsValidPath(path))
            {
                throw new ArgumentException("Path is invalid");
            }

            Path = path;
            PathType = type;
            PathStatus = status;
        }

        public Boolean IsValid()
        {
            return IsValid(PathType, PathStatus);
        }
        
        public Boolean IsValid(PathType type)
        {
            return IsValid(type, PathStatus);
        }
        
        public Boolean IsValid(PathStatus status)
        {
            return IsValid(PathType, status);
        }
        
        public Boolean IsValid(PathType type, PathStatus status)
        {
            return PathUtils.IsValidPath(Path, type, status);
        }

        public Boolean IsExistAsFolder()
        {
            return PathUtils.IsExistAsFolder(Path);
        }
        
        public Boolean IsExistAsFile()
        {
            return PathUtils.IsExistAsFile(Path);
        }

        public Boolean IsExist()
        {
            return PathUtils.IsExist(Path, PathType);
        }
        
        public Boolean IsExist(PathType type)
        {
            return PathUtils.IsExist(Path, type);
        }
        
        public String GetAbsolutePath()
        {
            return PathUtils.GetFullPath(Path);
        }

        public String[] GetFolders()
        {
            return GetFolders(new Regex(".*"), Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }
        
        public String[] GetFolders(SearchOption searchOption)
        {
            return GetFolders(new Regex(".*"), searchOption);
        }

        public String[] GetFolders([RegexPattern] String searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return GetFolders(new Regex(String.IsNullOrEmpty(searchPattern) ? ".*" : searchPattern), searchOption);
        }
        
        public String[] GetFolders(Regex regex, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            regex ??= new Regex(".*");
            try
            {
                return LongPath.Directory.GetDirectories(Path, "*", searchOption)
                    .Where(name => regex.IsMatch(name)).ToArray();
            }
            catch (Exception)
            {
                return new String[0];
            }
        }
        
        public String[] GetFiles()
        {
            return GetFiles(new Regex(".*"), Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }
        
        public String[] GetFiles(SearchOption searchOption)
        {
            return GetFiles(new Regex(".*"), searchOption);
        }
        
        public String[] GetFiles(String searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return GetFiles(new Regex(String.IsNullOrEmpty(searchPattern) ? ".*" : searchPattern), searchOption);
        }
        
        public String[] GetFiles(Regex regex, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            regex ??= new Regex(".*");
            try
            {
                return LongPath.Directory.GetFiles(Path, "*", searchOption)
                    .Where(name => regex.IsMatch(name)).ToArray();
            }
            catch (Exception)
            {
                return new String[0];
            }
        }

        public String[] GetFoldersAndFiles()
        {
            SearchOption searchOption = Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return GetFoldersAndFiles(searchOption, searchOption);
        }
        
        public String[] GetFoldersAndFiles(SearchOption searchOption)
        {
            return GetFoldersAndFiles(searchOption, searchOption);
        }

        public String[] GetFoldersAndFiles(SearchOption foldersSearchOption, SearchOption filesSearchOption)
        {
            return GetFolders(foldersSearchOption).Concat(GetFiles(filesSearchOption)).ToArray();
        }
        
        public String[] GetFoldersAndFiles(String foldersSearchPattern, String filesSearchPattern, SearchOption foldersSearchOption, SearchOption filesSearchOption)
        {
            return GetFolders(foldersSearchPattern, foldersSearchOption).Concat(GetFiles(filesSearchPattern, filesSearchOption)).ToArray();
        }
        
        public String[] GetFoldersAndFiles(Regex foldersSearchPattern, Regex filesSearchPattern, SearchOption foldersSearchOption, SearchOption filesSearchOption)
        {
            return GetFolders(foldersSearchPattern, foldersSearchOption).Concat(GetFiles(filesSearchPattern, filesSearchOption)).ToArray();
        }
        
        public Byte[] ReadFileBytes(Boolean isThrow = false)
        {
            return FileUtils.ReadFileBytes(Path, isThrow);
        }
        
        public String ReadFileText(Boolean isThrow = false)
        {
            return FileUtils.ReadFileText(Path, isThrow);
        }
        
        public String[] ReadFileLines(Boolean isThrow = false)
        {
            return FileUtils.ReadFileLines(Path, isThrow);
        }

        public override String ToString()
        {
            return Path;
        }

        public override Boolean Equals(Object obj)
        {
            return obj != null && obj.ToString().Equals(Path, StringComparison.InvariantCultureIgnoreCase);
        }

        public override Int32 GetHashCode()
        {
            return Path.GetHashCode();
        }
    }
}