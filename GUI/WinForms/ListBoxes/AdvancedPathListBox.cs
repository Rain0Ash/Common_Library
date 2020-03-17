// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common_Library.GUI.WinForms.Buttons;
using Common_Library.Types.Other;
using Common_Library.Utils.IO;

namespace Common_Library.GUI.WinForms.ListBoxes
{
    public class AdvancedPathListBox : AdvancedListBox
    {
        public new readonly AdditionalPathListBox ListBox = new AdditionalPathListBox();

        public PathType PathType
        {
            get
            {
                return ListBox.PathType;
            }
            set
            {
                ListBox.PathType = value;
            }
        }

        public readonly Button RecursiveButton = new Button
        {
            Text = @"R",
        };

        public new readonly DialogButton AddButton = new DialogButton();

        public override String AddButtonToolTip
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

        public override String RemoveButtonToolTip
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

        public String RecursiveButtonToolTip
        {
            get
            {
                return HelpToolTip.GetToolTip(RecursiveButton);
            }
            set
            {
                HelpToolTip.SetToolTip(RecursiveButton, value);
            }
        }

        public AdvancedPathListBox()
        {
            AddButton.OpenFileDialog.Multiselect = true;
            RecursiveButton.Click += (sender, args) =>
            {
                foreach (Int32 index in ListBox.SelectedIndices)
                {
                    Object item = ListBox.Items[index];
                    if (item is PathObject path)
                    {
                        path.Recursive = !path.Recursive;
                    }
                }

                ListBox.Update();
            };
            AddButton.PathBeenSelected += path => ListBox.Add(new PathObject(path, PathType, PathStatus.Exist) {Recursive = true});
            AddButton.PathsBeenSelected += paths =>
                ListBox.AddRange(paths.Select(path => new PathObject(path.ToString(), PathType, PathStatus.Exist) {Recursive = true})
                    .ToArray());
            RemoveButton.Click += (sender, args) => ListBox.RemoveAt(ListBox.SelectedIndices);
            Controls.Add(AddButton);
            Controls.Add(RemoveButton);
            Controls.Add(RecursiveButton);
            Controls.Add(ListBox);
        }

        protected override void UpdatePosition()
        {
            ListBox.Size = new Size(Size.Width - ButtonSize, Size.Height + 8);
            ListBox.Location = new Point(0, 0);
            RecursiveButton.Size = new Size(ButtonSize, ButtonSize);
            RecursiveButton.Location = new Point(ListBox.Size.Width, -1);
            AddButton.Size = new Size(ButtonSize, ButtonSize);
            AddButton.Location = new Point(ListBox.Size.Width, RecursiveButton.Size.Height - 1);
            AddButton.Image = AddButtonImage != null ? new Bitmap(AddButtonImage, AddButton.Size) : null;
            RemoveButton.Size = new Size(ButtonSize, ButtonSize);
            RemoveButton.Location = new Point(ListBox.Size.Width, RecursiveButton.Size.Height + AddButton.Size.Height - 1);
            RemoveButton.Image = RemoveButtonImage != null ? new Bitmap(RemoveButtonImage, RemoveButton.Size) : null;
        }
    }
}