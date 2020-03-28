﻿using System;
using System.IO;
using Common_Library.Watchers.Interfaces;

namespace Common_Library.Watchers.FileSystem.Interfaces
{
    /// <summary>
    /// Defines properties, events and methods for a FileSystemWatcher-like class
    /// </summary>
    public interface IWatcher : IReadOnlyWatcher
    {
        public new Boolean EnableRaisingEvents { get; set; }
        public new String Filter { get; set; }
        public new Boolean IncludeSubdirectories { get; set; }
        public new Int32 InternalBufferSize { get; set; }
        public new NotifyFilters NotifyFilter { get; set; }
        public new String Path { get; set; }
        
        public void StartWatch();
        public void StopWatch();
    }
}