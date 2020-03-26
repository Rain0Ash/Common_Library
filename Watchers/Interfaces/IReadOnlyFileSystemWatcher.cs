// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Common_Library.FileSystem.Interfaces
{
    public interface IReadOnlyFileSystemWatcher : IDisposable
    {
        public event FileSystemEventHandler Changed;
        public event FileSystemEventHandler Created;
        public event FileSystemEventHandler Deleted;
        public event ErrorEventHandler Error;
        public event RenamedEventHandler Renamed;
        
        public String Path { get; }
        public Boolean Recursive { get; }

        public String Filter { get; }
        public Collection<String> Filters { get; }

        public NotifyFilters NotifyFilter { get; }

        public Boolean IsWatch { get; }
    }
}