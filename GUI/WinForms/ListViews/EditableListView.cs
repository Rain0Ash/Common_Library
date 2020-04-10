// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Common_Library.GUI.WinForms.Forms;
using Common_Library.GUI.WinForms.ToolStrips;
using Common_Library.GUI.WinForms.ToolStrips.Items;
using Common_Library.Types.Map;
using Common_Library.Types.Other;
using Common_Library.Utils;

namespace Common_Library.GUI.WinForms.ListViews
{
    public class EditableListView : EventListView
    {
        public Boolean AllowContextMenu { get; set; } = true;
        public MouseButtons ContextMenuButton { get; } = MouseButtons.Right;

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
        
        private readonly FixedContextMenuStrip _menu = new FixedContextMenuStrip();

        public Boolean DoubleClickForChange { get; set; } = true;
        
        public Boolean DoubleEmptyClickForAdd { get; set; } = true;

        #region Text
        
        public TextBoxForm ItemForm { get; set; } = new TextBoxForm();

        public String ItemFormTitle
        {
            get
            {
                return ItemForm.Text;
            }
            set
            {
                ItemForm.Text = value;
            }
        }
        
        public String ItemFormApplyButtonText
        {
            get
            {
                return ItemForm.ApplyButton.Text;
            }
            set
            {
                ItemForm.ApplyButton.Text = value;
            }
        }
        
        public String ItemFormCancelButtonText
        {
            get
            {
                return ItemForm.DenyButton.Text;
            }
            set
            {
                ItemForm.DenyButton.Text = value;
            }
        }

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

        protected readonly IndexMap<ActionType, FixedToolStripMenuItem> Buttons = new IndexMap<ActionType, FixedToolStripMenuItem>
        {
            {ActionType.Select, new FixedToolStripMenuItem("Select")},
            {ActionType.Copy, new FixedToolStripMenuItem("Copy"){AllowOnMinItems = 1}},
            {ActionType.View, new FixedToolStripMenuItem("View"){AllowOnMinItems = 1, AllowOnMaxItems = 1}},
            {ActionType.Paste, new FixedToolStripMenuItem("Paste")},
            {ActionType.Cut, new FixedToolStripMenuItem("Cut"){AllowOnMinItems = 1}},
            {ActionType.Add, new FixedToolStripMenuItem("Add")},
            {ActionType.Remove, new FixedToolStripMenuItem("Remove"){AllowOnMinItems = 1}},
            {ActionType.Change, new FixedToolStripMenuItem("Change"){AllowOnMinItems = 1, AllowOnMaxItems = 1}},
            {ActionType.ChangeStatus, new FixedToolStripMenuItem("ChangeStatus"){AllowOnMinItems = 1}},
            {ActionType.Replace, new FixedToolStripMenuItem("Replace")},
            {ActionType.Reset, new FixedToolStripMenuItem("Reset")},
            {ActionType.Additional1, new FixedToolStripMenuItem("Additional1")},
            {ActionType.Additional2, new FixedToolStripMenuItem("Additional2")},
            {ActionType.Additional3, new FixedToolStripMenuItem("Additional3")},
        };
        
        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg >= 0x204 && msg.Msg <= 0x206)
            {
                Point pointMousePos = new Point(msg.LParam.ToInt32() & 0xffff, msg.LParam.ToInt32() >> 16);
                ListViewHitTestInfo lvhti = HitTest(pointMousePos);

                if (lvhti.Item == null)
                {
                    SelectedIndices.Clear();
                }
                else if (SelectedIndices.Count == 0 && lvhti.Item != null)
                {
                    lvhti.Item.Selected = true;
                    lvhti.Item.Focused = true;
                }
            }
            
            base.WndProc(ref msg);
        }

        public EditableListView()
        {
            ActionType = ActionType.Basic;
            ValidateItemChanged += () => ItemForm.TextBox.Validate = () => IsValidItem(ItemForm.TextBox.Text);

            KeyDown += OnKeyDown;
            MouseDown += OpenContextMenu;
            _menu.ItemClicked += OnMenuActionClicked;
            ItemDoubleClick += DoubleClickChange;
            EmptyDoubleClick += DoubleEmptyClickAdd;
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

        private void UpdateMenuButtons(Int32 count = -1)
        {
            _menu.Items.Clear();

            foreach ((ActionType key, FixedToolStripMenuItem item) in Buttons)
            {
                if (!ActionType.HasFlag(key))
                {
                    continue;
                }

                if (key == ActionType.Select && !MultiSelect)
                {
                    continue;
                }

                if (count >= 0 && !item.SelectedCountInRange(count))
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
                case Keys.E when ActionType.HasFlag(ActionType.View) && e.Control:
                    ViewAction();
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

        public virtual void ViewAction()
        {
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
                if (IsValidItem(text))
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

        private void DoubleEmptyClickAdd(MouseEventArgs e)
        {
            if (DoubleEmptyClickForAdd && ActionType.HasFlag(ActionType.Add))
            {
                AddAction();
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
            if (ItemForm.ShowDialog() == DialogResult.OK && IsValidItem(ItemForm.TextBox.Text))
            {
                item.Text = ItemForm.TextBox.Text;
            }
        }
        
        protected virtual void OpenContextMenu(Object sender, MouseEventArgs e)
        {
            if (!AllowContextMenu || e.Button != ContextMenuButton)
            {
                return;
            }

            UpdateMenuButtons(SelectedItems.Count);
            _menu.Show(Cursor.Position);
        }

        protected virtual void OnMenuActionClicked(Object sender, ToolStripItemClickedEventArgs e)
        {
            if (!(e.ClickedItem is FixedToolStripMenuItem item))
            {
                return;
            }
            
            ActionType action = Buttons.TryGetValue(item);

            switch (action)
            {
                case ActionType.Select:
                    SelectAction();
                    break;
                case ActionType.Copy:
                    CopyAction();
                    break;
                case ActionType.View:
                    ViewAction();
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