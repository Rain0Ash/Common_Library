// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.IO;
using Common_Library.Exceptions;
using Common_Library.FileSystem.Interfaces;
using JetBrains.Annotations;

namespace Common_Library.FileSystem
{
    public class FileSystemWatcherExtended : FileSystemWatcher, IReadOnlyFileSystemWatcher
    {
        public FileSystemWatcherExtended()
        {
        }

        public FileSystemWatcherExtended([NotNull] String path)
            : base(path)
        {
        }

        public FileSystemWatcherExtended([NotNull] String path, [NotNull] String filter)
            : base(path, filter)
        {
        }

        public Boolean Recursive
        {
            get
            {
                return IncludeSubdirectories;
            }
            set
            {
                IncludeSubdirectories = value;
            }
        }

        public Boolean IsWatch
        {
            get
            {
                return EnableRaisingEvents;
            }
        }

        public void StartWatch()
        {
            if (EnableRaisingEvents)
            {
                throw new AlreadyInitializedException("Filesystem watcher already started");
            }
            
            EnableRaisingEvents = true;
        }

        public void StopWatch()
        {
           EnableRaisingEvents = false;
        }
    }
}