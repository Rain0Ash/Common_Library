// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using Common_Library.Utils.IO;
using Common_Library.Watchers.Interfaces;

namespace Common_Library.Watchers
{
    public abstract class WatcherBase : IPathWatcher
    {
        public static implicit operator String(WatcherBase watcher)
        {
            return watcher.ToString();
        }
        
        public event Handlers.EmptyHandler PathChanged;
        
        private String _path;
        public String Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (_path == value)
                {
                    return;
                }

                _path = value;
                
                PathChanged?.Invoke();
            }
        }
        public PathStatus PathStatus { get; set; }

        public virtual Image Icon
        {
            get
            {
                return Images.Images.Lineal.WWW;
            }
        }

        public WatcherBase()
        {
            PathChanged += OnPathChanged;
        }
        
        protected virtual void OnPathChanged()
        {
        }
        
        public virtual Boolean IsValid()
        {
            throw new NotImplementedException();
        }

        public virtual Boolean IsExist()
        {
            throw new NotImplementedException();
        }

        public virtual void StartWatch()
        {
            throw new NotImplementedException();
        }

        public virtual void StopWatch()
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            return Path;
        }
        
        public virtual void Dispose()
        {
        }
    }
}