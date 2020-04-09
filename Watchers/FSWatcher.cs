// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Common_Library.Exceptions;
using Common_Library.Utils.IO;
using Common_Library.Watchers.FileSystem;
using Common_Library.Watchers.FileSystem.Interfaces;
using JetBrains.Annotations;

namespace Common_Library.Watchers
{
    public class FSWatcher : WatcherBase
    {
        public PathType PathType { get; set; }

        private readonly IWatcher _watcher;

        public IReadOnlyWatcher Watcher
        {
            get
            {
                return _watcher;
            }
        }

        public TimeSpan? Polling
        {
            get
            {
                CheckWatcher();

                if (_watcher is IPoller poller)
                {
                    return poller.Polling;
                }
                
                return null;
            }
            set
            {
                CheckWatcher();

                if (value != null && _watcher is IPoller poller)
                {
                    poller.Polling = value.Value;
                }
            }
        }

        public Boolean WatcherEnabled
        {
            get
            {
                CheckWatcher();

                return _watcher.EnableRaisingEvents;
            }
            set
            {
                CheckWatcher();

                _watcher.EnableRaisingEvents = value;
            }
        }

        public event Handlers.EmptyHandler RecursiveChanged;
        private Boolean _isRecursive;
        public Boolean IsRecursive
        {
            get
            {
                return _isRecursive;
            }
            set
            {
                if (_isRecursive == value)
                {
                    return;
                }

                _isRecursive = value;
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

        public override Image Icon
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
        
        public FSWatcher(String path, PathType type = PathType.All, PathStatus status = PathStatus.All, WatcherType watcherType = WatcherType.None)
        {
            if (!PathUtils.IsValidPath(path))
            {
                throw new ArgumentException($"Path: \"{path}\" is invalid");
            }

            Path = path;
            PathType = type;
            PathStatus = status;

            _watcher = FileSystem.Watcher.Factory(watcherType, Path);

            if (_watcher != null)
            {
                WatcherEnabled = false;
            }
        }

        protected override void OnPathChanged()
        {
            if (_watcher != null)
            {
                _watcher.Path = Path;
            }
        }
        
        public FSWatcher(String path, IWatcher watcher, PathType type = PathType.All, PathStatus status = PathStatus.All)
            : this(path, type, status)
        {
            if (_watcher.Path != path)
            {
                throw new ArgumentException("Watcher path need to equals path");
            }
            
            _watcher = watcher;
        }

        private void CheckWatcher()
        {
            if (_watcher == null)
            {
                throw new NotInitializedException("Watcher is not initialized");
            }
        }
        
        private void OnRecursive_Changed()
        {
            if (_watcher == null)
            {
                return;
            }
            
            _watcher.IncludeSubdirectories = IsRecursive;
        }

        public override void StartWatch()
        {
            CheckWatcher();
            
            _watcher.StartWatch();
        }

        public override void StopWatch()
        {
            _watcher?.StopWatch();
        }

        public override Boolean IsValid()
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

        public override Boolean IsExist()
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

        public IEnumerable<String> GetEntries()
        {
            return GetEntries(PathType);
        }
        
        public IEnumerable<String> GetEntries(PathType type)
        {
            return GetEntries(type, IsRecursive);
        }
        
        public IEnumerable<String> GetEntries(Boolean recursive)
        {
            return GetEntries(PathType, recursive);
        }
        
        public IEnumerable<String> GetEntries(PathType type, Boolean recursive)
        {
            return GetEntries(new Regex(".*"), recursive, type);
        }

        public IEnumerable<String> GetEntries([NotNull][RegexPattern] String searchPattern, Boolean recursive = false, PathType type = PathType.All)
        {
            return GetEntries(new Regex(String.IsNullOrEmpty(searchPattern) ? ".*" : searchPattern), recursive, type);
        }

        public IEnumerable<String> GetEntries(Regex regex, Boolean recursive = false, PathType type = PathType.All)
        {
            regex ??= new Regex(".*");
            try
            {
                return DirectoryUtils.GetEntries(Path, recursive, type).Where(name => regex.IsMatch(name));
            }
            catch (Exception)
            {
                return null;
            }
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
        
        public override void Dispose()
        {
            Watcher?.Dispose();
        }
    }
}