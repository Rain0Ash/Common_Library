// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Utils;
using Common_Library.Utils.IO;
using Common_Library.Watchers;

namespace Common_Library.GUI.WinForms.ListViews
{
    public class PathListView : EditableListView
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
                Refresh();
            }
        }
        
        public String RecursiveText
        {
            get
            {
                return GetText(ActionType.ChangeStatus);
            }
            set
            {
                SetText(value, ActionType.ChangeStatus);
            }
        }

        public PathListView()
        {
            ValidateFunc = obj => CheckValidFormat();
            RecursiveText = "Recursive";
            ActionType |= ActionType.ChangeStatus;
        }

        public override void Insert(Int32 index, Object item)
        {
            if (!(item is ListViewItem lvitem))
            {
                lvitem = ItemsDictionary.GetOrAdd(item, new ListViewItem(item.ToString()));

                if (item is FSWatcher watcher)
                {
                    lvitem.Tag = watcher;
                }
            }

            if (!(lvitem.Tag is FSWatcher))
            {
                lvitem.Tag = new FSWatcher(lvitem.Text);
            }
            
            base.Insert(index, lvitem);
        }

        protected override Boolean CheckValidFormatItem(ListViewItem item)
        {
            Object path = item.Tag ?? item.Text;
            
            if (path is FSWatcher watcher)
            {
                return watcher.IsValid();
            }

            return PathUtils.IsValidPath(path.ToString(), PathType);
        }

        protected override void OnKeyDown(Object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.R when ActionType.HasFlag(ActionType.ChangeStatus) && e.Control:
                    e.Handled = true;
                    RecursiveAction();
                    break;
                default:
                    base.OnKeyDown(sender, e);
                    break;
            }
        }

        protected virtual void RecursiveAction()
        {
            foreach (ListViewItem item in SelectedItems)
            {
                if (item.Tag is FSWatcher watcher)
                {
                    watcher.Recursive = true;
                }
            }
            
            Refresh();
        }

        protected override void OnDrawItem(Object sender, DrawListViewItemEventArgs e)
        {
            base.OnDrawItem(sender, e);
            
            Graphics graphics = e.Graphics;
            ListViewItem item = e.Item;
            Object watcher = item.Tag;
            
            if (watcher is FSWatcher path && path.Recursive)
            {
                graphics.DrawString("R", item.Font, Brushes.Red,
                    new Rectangle(item.Bounds.Width - TextRenderer.MeasureText("R", item.Font).Width, item.Bounds.Y, item.Bounds.Width, item.Bounds.Height));
            }
        }
    }
}