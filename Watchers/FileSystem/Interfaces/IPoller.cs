using System;
using System.IO;
using System.Threading.Tasks;

namespace Common_Library.Watchers.FileSystem.Interfaces
{
    public interface IPoller : IDisposable, ICloneable
    {
        Boolean EnableRaisingEvents { get; set; }
        String Filter { get; set; }
        Boolean IncludeSubdirectories { get; set; }
        String Path { get; set; }
        TimeSpan Polling { get; set; }
        PollingType PollingType { get; set; }
        
        event FileSystemEventHandler Created;
        event FileSystemEventHandler Deleted;
        event ErrorEventHandler Error;

        Task ForcePollAsync(Boolean returnWhenPolled);
        void ForcePoll();
    }
}