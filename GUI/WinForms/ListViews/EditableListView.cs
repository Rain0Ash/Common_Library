// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Types.Map;
using Common_Library.Types.Other;
using Common_Library.Utils;

namespace Common_Library.GUI.WinForms.ListViews
{
    public class EditableListView : EventListView
    {
        public Boolean AllowContextMenu { get; set; } = true;
        public MouseButtons ContextMenuButton { get; set; } = MouseButtons.Right;

        private ActionType _actionType = ActionType.Basic;
        public ActionType ActionType 
        {
            get
            {
                return _actionType;
            }
            set
            {
                if (_actionType == value)
                {
                    return;
                }
                
                _actionType = value;

                UpdateMenuButtons();
            }
        }
        
        private readonly ContextMenuStrip _menu = new ContextMenuStrip();
        
        #region Text

        protected String GetText(ActionType action)
        {
            return Buttons.TryGetValue(action)?.Text;
        }
        
        protected void SetText(String value, ActionType action)
        {
            ToolStripItem item = Buttons.TryGetValue(action);

            if (item == null)
            {
                return;
            }

            if (value != null)
            {
                item.Text = value;
                return;
            }

            ActionType &= ~action;
        }
        
        public String SelectText
        {
            get
            {
                return GetText(ActionType.Select);
            }
            set
            {
                SetText(value, ActionType.Select);
            }
        }
        
        public String CopyText
        {
            get
            {
                return GetText(ActionType.Copy);
            }
            set
            {
                SetText(value, ActionType.Copy);
            }
        }
        
        public String PasteText
        {
            get
            {
                return GetText(ActionType.Paste);
            }
            set
            {
                SetText(value, ActionType.Paste);
            }
        }
        
        public String CutText
        {
            get
            {
                return GetText(ActionType.Cut);
            }
            set
            {
                SetText(value, ActionType.Cut);
            }
        }
        
        public String AddText
        {
            get
            {
                return GetText(ActionType.Add);
            }
            set
            {
                SetText(value, ActionType.Add);
            }
        }
        
        public String RemoveText
        {
            get
            {
                return GetText(ActionType.Remove);
            }
            set
            {
                SetText(value, ActionType.Remove);
            }
        }

        public String ChangeText
        {
            get
            {
                return GetText(ActionType.Change);
            }
            set
            {
                SetText(value, ActionType.Change);
            }
        }

        #endregion

        protected readonly IndexMap<ActionType, ToolStripItem> Buttons = new IndexMap<ActionType, ToolStripItem>
        {
            {ActionType.Select, new ToolStripMenuItem("Select")},
            {ActionType.Copy, new ToolStripMenuItem("Copy")},
            {ActionType.Paste, new ToolStripMenuItem("Paste")},
            {ActionType.Cut, new ToolStripMenuItem("Cut")},
            {ActionType.Add, new ToolStripMenuItem("Add")},
            {ActionType.Remove, new ToolStripMenuItem("Remove")},
            {ActionType.Change, new ToolStripMenuItem("Change")},
            {ActionType.ChangeStatus, new ToolStripMenuItem("ChangeStatus")},
            {ActionType.Replace, new ToolStripMenuItem("Replace")},
            {ActionType.Additional1, new ToolStripMenuItem("Additional1")},
            {ActionType.Additional2, new ToolStripMenuItem("Additional2")},
            {ActionType.Additional3, new ToolStripMenuItem("Additional3")},
        };

        public EditableListView()
        {
            UpdateMenuButtons();

            KeyDown += OnKeyDown;
            MouseDown += OpenContextMenu;
            _menu.ItemClicked += OnMenuActionClicked;
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (!ActionType.HasFlag(ActionType.Select))
            {
                SelectedIndices.Clear();
                return;
            }
            
            base.OnSelectedIndexChanged(e);
        }

        private void UpdateMenuButtons()
        {
            _menu.Items.Clear();

            foreach ((ActionType key, ToolStripItem button) in Buttons)
            {
                if (!ActionType.HasFlag(key))
                {
                    continue;
                }

                if (key == ActionType.Select && !MultiSelect)
                {
                    continue;
                }
                    
                _menu.Items.Add(button);
            }
        }
        
        protected virtual void OnKeyDown(Object sender, KeyEventArgs e)
        {
            e.Handled = true;
            
            switch (e.KeyCode)
            {
                case Keys.A when ActionType.HasFlag(ActionType.Select) && e.Control:
                    SelectAction();
                    break;
                case Keys.C when ActionType.HasFlag(ActionType.Copy) && e.Control:
                    CopyAction();
                    break;
                case Keys.V when ActionType.HasFlag(ActionType.Paste) && e.Control:
                    PasteAction();
                    break;
                case Keys.X when ActionType.HasFlag(ActionType.Cut) && e.Control:
                    CutAction();
                    break;
                case Keys.Up when ActionType.HasFlag(ActionType.Swap) && e.Shift:
                    SwapAction(PointOffset.Up);
                    break;
                case Keys.Down when ActionType.HasFlag(ActionType.Swap) && e.Shift:
                    SwapAction(PointOffset.Down);
                    break;
                case Keys.Delete when ActionType.HasFlag(ActionType.Remove):
                    RemoveAction();
                    return;
                default:
                    e.Handled = false;
                    break;
            }
        }

        protected virtual void SelectAction()
        {
            if (!MultiSelect)
            {
                return;
            }

            foreach (ListViewItem item in Items)
            {
                item.Selected = true;
            }
        }
        
        protected virtual void CopyAction()
        {
            try
            {
                if (SelectedItems.Count > 0)
                {
                    Clipboard.SetText(SelectedItems.OfType<ListViewItem>().Select(item => item.Text).Join("; "));
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected virtual void PasteAction()
        {
            try
            {
                if (Clipboard.ContainsAudio())
                {
                    AddAction(Clipboard.GetAudioStream());
                }
                else if (Clipboard.ContainsImage())
                {
                    AddAction(Clipboard.GetImage());
                }
                else if (Clipboard.ContainsText())
                {
                    AddAction(Clipboard.GetText());
                }
                else if (Clipboard.ContainsFileDropList())
                {
                    AddAction(Clipboard.GetFileDropList());
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        
        protected virtual void CutAction()
        {
            CopyAction();
            RemoveAction();
        }

        protected virtual void SwapAction(PointOffset offset)
        {
            if (SelectedIndices.Count != 1)
            {
                return;
            }

            Int32 index = SelectedIndices[0];

            switch (offset)
            {
                case PointOffset.Up:
                    if (!IndexInItems(index - 1))
                    {
                        return;
                    }
                    
                    SwapItems(index, index - 1);

                    break;
                case PointOffset.Down:
                    if (!IndexInItems(index + 1))
                    {
                        return;
                    }
                    
                    SwapItems(index, index + 1);
                    
                    break;
                default:
                    break;
            }
        }
        
        protected virtual void AddAction()
        {
            
        }

        protected virtual void AddAction(Object item)
        {
            
        }
        
        protected virtual void AddAction(ListViewItem item)
        {
            Insert(SelectedIndices.OfType<Int32>().FirstOr(SelectedIndices.Count), item);
        }
        
        protected virtual void AddAction(Stream stream)
        {
            
        }
        
        protected virtual void AddAction(Image image)
        {
            
        }
        
        protected virtual void AddAction(String text)
        {
            
        }
        
        protected virtual void AddAction(StringCollection collection)
        {
            
        }
        
        protected virtual void RemoveAction()
        {
            try
            {
                foreach (ListViewItem item in SelectedItems)
                {
                    Remove(item);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected virtual void ChangeAction()
        {
        }

        private void OpenContextMenu(Object sender, MouseEventArgs e)
        {
            if (!AllowContextMenu || e.Button != ContextMenuButton)
            {
                return;
            }
            
            _menu.Show(Cursor.Position);

            if (FocusedItem != null && FocusedItem.Bounds.Contains(e.Location))
            {
                
            }
        }

        private void OnMenuActionClicked(Object sender, ToolStripItemClickedEventArgs e)
        {
            ActionType action = Buttons.TryGetValue(e.ClickedItem);

            switch (action)
            {
                case ActionType.Select:
                    SelectAction();
                    break;
                case ActionType.Copy:
                    CopyAction();
                    break;
                case ActionType.Paste:
                    PasteAction();
                    break;
                case ActionType.Cut:
                    CutAction();
                    break;
                case ActionType.Add:
                    AddAction();
                    break;
                case ActionType.Remove:
                    RemoveAction();
                    break;
                case ActionType.Change:
                    ChangeAction();
                    break;
                default:
                    break;
            }
        }
    }
}