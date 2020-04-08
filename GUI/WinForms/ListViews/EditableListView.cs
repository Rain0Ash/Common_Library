// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Common_Library.GUI.WinForms.Forms;
using Common_Library.Types.Map;
using Common_Library.Types.Other;
using Common_Library.Utils;

namespace Common_Library.GUI.WinForms.ListViews
{
    public class EditableListView : EventListView
    {
        public Boolean AllowContextMenu { get; set; } = true;
        public MouseButtons ContextMenuButton { get; set; } = MouseButtons.Right;

        private ActionType _actionType;
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

        public Boolean DoubleClickForChange { get; set; } = true;
        
        #region Text
        
        public TextBoxForm ItemForm { get; set; } = new TextBoxForm();

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
            {ActionType.Reset, new ToolStripMenuItem("Reset")},
            {ActionType.Additional1, new ToolStripMenuItem("Additional1")},
            {ActionType.Additional2, new ToolStripMenuItem("Additional2")},
            {ActionType.Additional3, new ToolStripMenuItem("Additional3")},
        };

        public EditableListView()
        {
            ActionType = ActionType.Basic;
            ValidateFuncChanged += function => ItemForm.TextBox.ValidateFunc = function;

            KeyDown += OnKeyDown;
            MouseDown += OpenContextMenu;
            _menu.ItemClicked += OnMenuActionClicked;
            ItemDoubleClick += DoubleClickChange;
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

            foreach ((ActionType key, ToolStripItem item) in Buttons)
            {
                if (!ActionType.HasFlag(key))
                {
                    continue;
                }

                if (key == ActionType.Select && !MultiSelect)
                {
                    continue;
                }
                    
                _menu.Items.Add(item);
            }
        }
        
        protected virtual void OnKeyDown(Object sender, KeyEventArgs e)
        {
            e.Handled = true;
            
            switch (e.KeyCode)
            {
                case Keys.Enter when SelectedItems.Count > 0:
                    DoubleClickChange(SelectedItems[0], null);
                    break;
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

        public virtual void SelectAction()
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
        
        public virtual void CopyAction()
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

        public virtual void PasteAction()
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
        
        public virtual void CutAction()
        {
            CopyAction();
            RemoveAction();
        }

        public virtual void SwapAction(PointOffset offset)
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
        
        public virtual void AddAction()
        {
            ItemForm.TextBox.Clear();

            if (ItemForm.ShowDialog() == DialogResult.OK)
            {
                AddAction(ItemForm.TextBox.Text);
            }
        }

        public virtual void AddAction(Object item)
        {
            switch (item)
            {
                case ListViewItem lvitem:
                    AddAction(lvitem);
                    break;
                case Stream stream:
                    AddAction(stream);
                    break;
                case Image image:
                    AddAction(image);
                    break;
                case StringCollection collection:
                    AddAction(collection);
                    break;
                default:
                    AddAction(item.ToString());
                    break;
            }
        }
        
        public virtual void AddAction(ListViewItem item)
        {
            Insert(SelectedIndices.OfType<Int32>().FirstOr(SelectedIndices.Count), item);
        }
        
        public virtual void AddAction(Stream stream)
        {
            
        }
        
        public virtual void AddAction(Image image)
        {
            
        }

        public virtual void AddAction(String text)
        {
            while (true)
            {
                if (ValidateFunc?.Invoke(text) != false)
                {
                    Add(text);
                }
                else
                {
                    ItemForm.TextBox.Clear();
                    ItemForm.TextBox.Text = text;

                    if (ItemForm.ShowDialog() == DialogResult.OK)
                    {
                        text = ItemForm.TextBox.Text;
                        continue;
                    }
                }

                break;
            }
        }

        public virtual void AddAction(StringCollection collection)
        {
            
        }
        
        public virtual void RemoveAction()
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

        private void DoubleClickChange(ListViewItem item, MouseEventArgs e)
        {
            if (DoubleClickForChange && ActionType.HasFlag(ActionType.Change))
            {
                ChangeAction(item);
            }
        }
        
        public virtual void ChangeAction()
        {
            if (SelectedItems.Count <= 0)
            {
                return;
            }

            ChangeAction(SelectedItems[0]);
        }

        public virtual void ResetAction()
        {
        }
        
        public virtual void ChangeAction(ListViewItem item)
        {
            ItemForm.TextBox.Clear();
            ItemForm.TextBox.Text = item.Text;
            if (ItemForm.ShowDialog() == DialogResult.OK && CheckValidItem(ItemForm.TextBox.Text))
            {
                item.Text = ItemForm.TextBox.Text;
            }
        }

        private void OpenContextMenu(Object sender, MouseEventArgs e)
        {
            if (!AllowContextMenu || e.Button != ContextMenuButton)
            {
                return;
            }


            if (FocusedItem == null || !FocusedItem.Bounds.Contains(e.Location))
            {
                
            }

            _menu.Show(Cursor.Position);
        }

        protected virtual void OnMenuActionClicked(Object sender, ToolStripItemClickedEventArgs e)
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
                case ActionType.Reset:
                    ResetAction();
                    break;
                default:
                    break;
            }
        }
    }
}