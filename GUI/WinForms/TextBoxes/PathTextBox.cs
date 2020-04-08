// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.IO;
using Common_Library.Utils;
using Common_Library.Utils.IO;

namespace Common_Library.GUI.WinForms.TextBoxes
{
    public class PathTextBox : HidenTextBox
    {
        public event Handlers.EmptyHandler CheckWellFormedChanged;

        private Boolean _checkWellFormed = true;

        public virtual Boolean CheckWellFormed
        {
            get
            {
                return _checkWellFormed;
            }
            set
            {
                if (_checkWellFormed == value)
                {
                    return;
                }

                _checkWellFormed = value;
                CheckWellFormedChanged?.Invoke();
            }
        }
        
        public event Handlers.EmptyHandler PathTypeChanged;

        private PathType _pathType = PathType.All;

        public PathType PathType
        {
            get
            {
                return _pathType;
            }
            set
            {
                if (_pathType == value)
                {
                    return;
                }

                _pathType = value;
                PathTypeChanged?.Invoke();
            }
        }
        
        public event Handlers.EmptyHandler PathStatusChanged;

        private PathStatus _pathStatus = PathStatus.All;

        public PathStatus PathStatus
        {
            get
            {
                return _pathStatus;
            }
            set
            {
                if (_pathStatus == value)
                {
                    return;
                }

                _pathStatus = value;
                PathStatusChanged?.Invoke();
            }
        }

        public PathTextBox()
        {
            ValidateFunc = obj => !CheckWellFormed || IsWellFormed();
            PasswdChar = '\0';
            CheckWellFormedChanged += CheckValidFormatColor;
            PathTypeChanged += CheckValidFormatColor;
        }

        protected override void CheckValidFormatColor()
        {
            Boolean check = CheckValidFormat();

            if (PathUtils.IsValidPath(Text, PathType, PathStatus) && check)
            {
                BackColor = Color.White;
            }
            else if (!check)
            {
                BackColor = Color.PaleVioletRed;
            }
            else
            {
                BackColor = Color.Coral;
            }
        }

        public Boolean IsValid()
        {
            return IsValidPath();
        }

        public Boolean IsValid(PathType type)
        {
            return IsValidPath(type);
        }
        
        public Boolean IsValid(PathStatus status)
        {
            return IsValidPath(status);
        }
        
        public virtual Boolean IsValid(PathType type, PathStatus status)
        {
            return IsValidPath(type, status);
        }

        public Boolean IsWellFormed()
        {
            return StringUtils.IsBracketsWellFormed(Text);
        }

        public Boolean IsValidPath()
        {
            return IsValidPath(PathType);
        }

        public Boolean IsValidPath(PathType type)
        {
            return PathUtils.IsValidPath(Text, type, PathStatus);
        }
        
        public Boolean IsValidPath(PathStatus status)
        {
            return PathUtils.IsValidPath(Text, PathType, status);
        }
        
        public Boolean IsValidPath(PathType type, PathStatus status)
        {
            return PathUtils.IsValidPath(Text, type, status);
        }

        public String GetAbsolutePath()
        {
            return PathUtils.GetFullPath(Text);
        }

        public String GetRelativePath()
        {
            String absolutePath = PathUtils.GetFullPath(Text);
            return absolutePath == null ? null : PathUtils.GetRelativePath(absolutePath, Directory.GetCurrentDirectory());
        }

        public String GetRelativePath(String relativePath)
        {
            String absolutePath = PathUtils.GetFullPath(relativePath);
            return absolutePath == null ? GetRelativePath() : PathUtils.GetRelativePath(PathUtils.GetFullPath(Text), absolutePath);
        }
    }
}