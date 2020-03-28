using System;
using System.IO;
using Common_Library.Watchers.FileSystem.Interfaces;

namespace Common_Library.Watchers.FileSystem
{
    /// <summary>
    /// A fake object which implements the IWatcher interface.
    /// </summary>
    public class FakeWatcher : IWatcher
    {
        public Boolean EnableRaisingEvents { get; set; }
        public String Filter { get; set; }
        public Boolean IncludeSubdirectories { get; set; }
        public Int32 InternalBufferSize { get; set; }
        public NotifyFilters NotifyFilter { get; set; }
        public String Path { get; set; }
        
        public event FileSystemEventHandler Changed
        {
            add { }
            remove { }
        }

        public event FileSystemEventHandler Created
        {
            add { }
            remove { }
        }

        public event FileSystemEventHandler Deleted
        {
            add { }
            remove { }
        }

        public event RenamedEventHandler Renamed
        {
            add { }
            remove { }
        }

        public event ErrorEventHandler Error
        {
            add { }
            remove { }
        }

        public FakeWatcher()
        {
        }
        
        public FakeWatcher(String path)
        {
            Path = path;
        }
        
        public FakeWatcher(String path, String filter)
            : this(path)
        {
            Filter = filter;
        }
        
        public void StartWatch()
        {
            EnableRaisingEvents = true;
        }
        
        public void StopWatch()
        {
            EnableRaisingEvents = false;
        }

        public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
        {
            return new WaitForChangedResult();
        }

        public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, Int32 timeout)
        {
            return new WaitForChangedResult();
        }
        
        public void Dispose()
        {
        }

        public Object Clone()
        {
            return this;
        }
    }
}