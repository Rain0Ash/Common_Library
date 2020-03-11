// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Windows.Forms;
using Common_Library.GUI.WinForms.Controls;
using Common_Library.GUI.WinForms.ToolTips;

namespace Common_Library.GUI.WinForms.ListBoxes
{
    public class AdvancedListBox : LocalizationControl
    {
        public readonly EditableListBox ListBox = new EditableListBox();

        public event Handlers.EmptyHandler ImageChanged;

        private Image _addButtonImage = Images.Images.Lineal.Plus;

        public Image AddButtonImage
        {
            get
            {
                return _addButtonImage;
            }
            set
            {
                _addButtonImage = value;
                ImageChanged?.Invoke();
            }
        }

        public readonly Button AddButton = new Button();

        private Image _removeButtonImage = Images.Images.Lineal.Minus;

        public Image RemoveButtonImage
        {
            get
            {
                return _removeButtonImage;
            }
            set
            {
                _removeButtonImage = value;
                ImageChanged?.Invoke();
            }
        }

        public readonly Button RemoveButton = new Button();

        public virtual String AddButtonToolTip
        {
            get
            {
                return HelpToolTip.GetToolTip(AddButton);
            }
            set
            {
                HelpToolTip.SetToolTip(AddButton, value);
            }
        }

        public virtual String RemoveButtonToolTip
        {
            get
            {
                return HelpToolTip.GetToolTip(RemoveButton);
            }
            set
            {
                HelpToolTip.SetToolTip(RemoveButton, value);
            }
        }

        public virtual String UpButtonToolTip
        {
            get
            {
                return HelpToolTip.GetToolTip(UpButton);
            }
            set
            {
                HelpToolTip.SetToolTip(UpButton, value);
            }
        }

        public virtual String DownButtonToolTip
        {
            get
            {
                return HelpToolTip.GetToolTip(DownButton);
            }
            set
            {
                HelpToolTip.SetToolTip(DownButton, value);
            }
        }

        public readonly Button UpButton = new Button();

        public readonly Button DownButton = new Button();

        public readonly HelpToolTip HelpToolTip = new HelpToolTip();

        public AdvancedListBox()
        {
            ListBox.SelectionMode = SelectionMode.MultiExtended;
            SizeChanged += (sender, args) => UpdatePosition();
            ImageChanged += UpdatePosition;


            if (GetType() != typeof(AdvancedListBox))
            {
                return;
            }

            RemoveButton.Click += (sender, args) => ListBox.RemoveAt(ListBox.SelectedIndices);
            Controls.Add(ListBox);
            Controls.Add(AddButton);
            Controls.Add(RemoveButton);
        }

        protected const Int32 ButtonSize = 19;

        protected virtual void UpdatePosition()
        {
            ListBox.Size = new Size(Size.Width - ButtonSize, Size.Height + 8);
            ListBox.Location = new Point(0, 0);
            AddButton.Size = new Size(ButtonSize, ButtonSize);
            AddButton.Location = new Point(ListBox.Size.Width, -1);
            AddButton.Image = AddButtonImage != null ? new Bitmap(AddButtonImage, AddButton.Size) : null;
            RemoveButton.Size = new Size(ButtonSize, ButtonSize);
            RemoveButton.Location = new Point(ListBox.Size.Width, AddButton.Size.Height - 1);
            RemoveButton.Image = RemoveButtonImage != null ? new Bitmap(RemoveButtonImage, RemoveButton.Size) : null;
        }

        protected override void Dispose(Boolean disposing)
        {
            HelpToolTip.Dispose();
        }
    }
}