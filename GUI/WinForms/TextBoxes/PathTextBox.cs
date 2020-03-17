// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using Common_Library.Utils;
using Common_Library.Utils.IO;
using Directory = Common_Library.LongPath.Directory;

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

        public PathTextBox()
        {
            PasswdChar = '\0';
            CheckWellFormedChanged += CheckValidFormatColor;
            PathTypeChanged += CheckValidFormatColor;
        }

        public override Boolean CheckValidFormat()
        {
            return !CheckWellFormed || IsWellFormed();
        }

        protected override void CheckValidFormatColor()
        {
            Boolean check = CheckValidFormat();

            if (PathUtils.IsValidPath(Text, PathType) && check)
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
            return IsValid(PathType);
        }

        public virtual Boolean IsValid(PathType type)
        {
            return IsValidPath(type);
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
            return PathUtils.IsValidPath(Text, type);
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