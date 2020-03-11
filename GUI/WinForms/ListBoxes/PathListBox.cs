// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Windows.Forms;
using Common_Library.Types.Other;
using Common_Library.Utils;

namespace Common_Library.GUI.WinForms.ListBoxes
{
    public class PathListBox : EditableListBox
    {
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

        public PathListBox()
        {
            PathTypeChanged += CheckValidFormatColor;
        }

        protected override Boolean CheckValidFormatItem(Object item)
        {
            if (item is PathObject path)
            {
                return path.IsValid();
            }

            return PathUtils.IsValidPath(item.ToString(), PathType);
        }

        protected override void OnDrawItem(DrawItemEventArgs e, Image image, (Brush, Brush) color)
        {
            base.OnDrawItem(e, image, color);

            Int32 index = e.Index;
            if (!IndexInItems(index))
            {
                return;
            }

            Object item = Items[index];
            Graphics graphics = e.Graphics;
            Rectangle bounds = e.Bounds;

            if (item is PathObject path && path.Recursive)
            {
                graphics.DrawString("R", e.Font, Brushes.Red,
                    new Rectangle(bounds.Width - TextRenderer.MeasureText("R", e.Font).Width, bounds.Y, bounds.Width, bounds.Height));
            }
        }

        protected override void Draw(DrawItemEventArgs e, Image image, (Brush, Brush)? colors)
        {
            if (!IndexInItems(e.Index))
            {
                return;
            }

            base.Draw(e, Items[e.Index] is PathObject path ? path.GetPathTypeIcon : null, null);
        }
    }
}