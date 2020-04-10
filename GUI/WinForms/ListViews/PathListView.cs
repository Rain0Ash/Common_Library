// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Common_Library.GUI.WinForms.Forms;
using Common_Library.GUI.WinForms.ToolStrips.Items;
using Common_Library.Types.Drawing;
using Common_Library.Utils;
using Common_Library.Utils.GUI.ListView;
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
            OverlapAllowed = false;
            ValidateItem = IsValidItem;
            RecursiveText = "Recursive";
            ActionType |= ActionType.ChangeStatus;
            ItemForm = new PathTextBoxForm();
            ItemForm.TextBox.Validate = () => IsValidItem(ItemForm.TextBox.Text);
        }

        public override void Insert(Int32 index, Object item)
        {
            if (!(item is ListViewItem lvitem))
            {
                lvitem = ItemsMap.TryGetValue(item, new ListViewItem(item.ToString()));

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

        public override Boolean IsValidItem(Object item)
        {
            return item switch
            {
                FSWatcher watcher => watcher.IsValid(),
                ListViewItem lvitem => IsValidItem(lvitem),
                _ => PathUtils.IsValidPath(item.ToString(), PathType, PathStatus)
            };
        }
        
        protected Boolean IsValidItem(ListViewItem item)
        {
            Object path = item.Tag ?? item.Text;
            
            if (path is FSWatcher watcher)
            {
                return watcher.IsValid();
            }

            return PathUtils.IsValidPath(path.ToString(), PathType, PathStatus);
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

        protected override void OnMenuActionClicked(Object sender, ToolStripItemClickedEventArgs e)
        {
            if (!(e.ClickedItem is FixedToolStripMenuItem item))
            {
                return;
            }
            
            ActionType action = Buttons.TryGetValue(item);

            switch (action)
            {
                case ActionType.ChangeStatus:
                    RecursiveAction();
                    break;
                default:
                    base.OnMenuActionClicked(sender, e);
                    break;
            }
        }

        public override void ViewAction()
        {
            if (SelectedItems.Count <= 0)
            {
                return;
            }
            
            String path = PathUtils.GetFullPath(SelectedItems[0].Text);

            if (!PathUtils.IsValidPath(path, PathType.Folder, PathStatus.Exist))
            {
                return;
            }

            try
            {
                Process.Start(@$"explorer.exe", $"\"{$@"{path}"}\"");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public override void ChangeAction(ListViewItem item)
        {
            base.ChangeAction(item);

            if (item.Tag is FSWatcher watcher)
            {
                watcher.Path = item.Text;
            }
        }
        
        public virtual void RecursiveAction()
        {
            foreach (ListViewItem item in SelectedItems)
            {
                if (item.Tag is FSWatcher watcher)
                {
                    watcher.IsRecursive = !watcher.IsRecursive;
                }
            }
        }

        protected override String GetItemImageKey(ListViewItem lvitem)
        {
            if (lvitem.Tag is FSWatcher watcher)
            {
                return lvitem.ImageList.GetOrSetImageKey(watcher.Icon);
            }

            return base.GetItemImageKey(lvitem);
        }
        
        protected override Color GetItemForeColor(ListViewItem lvitem, DrawingData data)
        {
            if (lvitem.Tag is FSWatcher watcher && watcher.IsRecursive)
            {
                return Color.Red;
            }
            
            return base.GetItemForeColor(lvitem, data);
        }
    }
}