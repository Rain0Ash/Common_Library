// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Common_Library.Utils.IO;
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

        private Image _folderImage;

        public Image FolderImage
        {
            get
            {
                return _folderImage ?? Images.Images.Lineal.Folder;
            }
            set
            {
                _folderImage = value;
            }
        }

        private Image _notFolderImage;

        public Image NotFolderImage
        {
            get
            {
                return _notFolderImage ?? Images.Images.Lineal.NotFolder;
            }
            set
            {
                _notFolderImage = value;
            }
        }

        private Image _fileImage;

        public Image FileImage
        {
            get
            {
                return _fileImage ?? Images.Images.Lineal.File;
            }
            set
            {
                _fileImage = value;
            }
        }

        private Image _notFileImage;

        public Image NotFileImage
        {
            get
            {
                return _notFileImage ?? Images.Images.Lineal.NotFile;
            }
            set
            {
                _notFileImage = value;
            }
        }

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

        public IEnumerable<String> GetFolders()
        {
            return GetFolders(new Regex(".*"), Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public IEnumerable<String> GetFolders(SearchOption searchOption)
        {
            return GetFolders(new Regex(".*"), searchOption);
        }

        public IEnumerable<String> GetFolders([RegexPattern] String searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return GetFolders(new Regex(String.IsNullOrEmpty(searchPattern) ? ".*" : searchPattern), searchOption);
        }

        public IEnumerable<String> GetFolders(Regex regex, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            regex ??= new Regex(".*");
            try
            {
                return DirectoryUtils.EnumerateDirectories(Path, "*", searchOption)
                    .Where(name => regex.IsMatch(name));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<String> GetFiles()
        {
            return GetFiles(new Regex(".*"), Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public IEnumerable<String> GetFiles(SearchOption searchOption)
        {
            return GetFiles(new Regex(".*"), searchOption);
        }

        public IEnumerable<String> GetFiles([RegexPattern] String searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return GetFiles(new Regex(String.IsNullOrEmpty(searchPattern) ? ".*" : searchPattern), searchOption);
        }

        public IEnumerable<String> GetFiles(Regex regex, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            regex ??= new Regex(".*");
            try
            {
                return DirectoryUtils.EnumerateFiles(Path, "*", searchOption)
                    .Where(name => regex.IsMatch(name));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<String> GetFoldersAndFiles()
        {
            SearchOption searchOption = Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return GetFoldersAndFiles(searchOption, searchOption);
        }

        public IEnumerable<String> GetFoldersAndFiles(SearchOption searchOption)
        {
            return GetFoldersAndFiles(searchOption, searchOption);
        }

        public IEnumerable<String> GetFoldersAndFiles(SearchOption foldersSearchOption, SearchOption filesSearchOption)
        {
            return GetFolders(foldersSearchOption).Concat(GetFiles(filesSearchOption));
        }

        public IEnumerable<String> GetFoldersAndFiles([RegexPattern] String foldersSearchPattern, [RegexPattern] String filesSearchPattern, SearchOption foldersSearchOption,
            SearchOption filesSearchOption)
        {
            return GetFolders(foldersSearchPattern, foldersSearchOption).Concat(GetFiles(filesSearchPattern, filesSearchOption));
        }

        public IEnumerable<String> GetFoldersAndFiles(Regex foldersSearchPattern, Regex filesSearchPattern, SearchOption foldersSearchOption,
            SearchOption filesSearchOption)
        {
            return GetFolders(foldersSearchPattern, foldersSearchOption).Concat(GetFiles(filesSearchPattern, filesSearchOption));
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

        public override Boolean Equals(Object? obj)
        {
            return obj != null && obj.ToString().Equals(Path, StringComparison.InvariantCultureIgnoreCase);
        }

        public override Int32 GetHashCode()
        {
            return Path.GetHashCode();
        }
    }
}