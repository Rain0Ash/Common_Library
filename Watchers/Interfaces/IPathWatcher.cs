// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using Common_Library.Utils.IO;

namespace Common_Library.Watchers.Interfaces
{
    public interface IPathWatcher : IDisposable
    {
        public String Path { get; }
        public PathStatus PathStatus { get; set; }
        
        public Image Icon { get; }

        public Boolean IsValid();
        
        public Boolean IsExist();

        public void StartWatch();
        
        public void StopWatch();
    }
}