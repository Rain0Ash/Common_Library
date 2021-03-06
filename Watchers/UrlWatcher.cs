// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using Common_Library.Utils.IO;

namespace Common_Library.Watchers
{
    public class UrlWatcher : WatcherBase
    {
        private Image _networkActiveImage;
        public Image NetworkActiveImage
        {
            get
            {
                return _networkActiveImage ?? Images.Images.Lineal.WWW;
            }
            set
            {
                _networkActiveImage = value;
            }
        }
        
        public override Image Icon
        {
            get
            {
                return NetworkActiveImage;
            }
        }

        public UrlWatcher(String url, PathStatus status = PathStatus.All)
        {
            if (!PathUtils.IsValidUrl(url))
            {
                throw new ArgumentException("Url is invalid");
            }

            Path = url;
            PathStatus = status;
        }

        public override Boolean IsValid()
        {
            return IsValid(PathStatus);
        }

        public Boolean IsValid(PathStatus status)
        {
            return status switch
            {
                PathStatus.All => PathUtils.IsValidUrl(Path),
                PathStatus.Exist => PathUtils.IsUrlContainData(Path),
                PathStatus.NotExist => !PathUtils.IsUrlContainData(Path),
                _ => throw new NotSupportedException($"{nameof(PathStatus)} {status} is not supported")
            };
        }

        public override Boolean IsExist()
        {
            return IsValid(PathStatus.Exist);
        }

        public override void Dispose()
        {
        }
    }
}