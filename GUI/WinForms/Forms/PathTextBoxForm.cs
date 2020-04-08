// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Windows.Forms;
using Common_Library.GUI.WinForms.ListBoxes;
using Common_Library.GUI.WinForms.TextBoxes;
using Common_Library.Utils;
using Common_Library.Utils.IO;

namespace Common_Library.GUI.WinForms.Forms
{
    public class PathTextBoxForm : TextBoxForm
    {
        public AdvancedPathTextBox PathTextBox
        {
            get
            {
                return Control as AdvancedPathTextBox;
            }
            set
            {
                Control = value;
            }
        }

        public override ValidableTextBox TextBox
        {
            get
            {
                return PathTextBox?.TextBox;
            }
            // ReSharper disable once ValueParameterNotUsed
            set
            {
                return;
            }
        }

        public Boolean OpenDialogOnEmpty { get; set; } = false;

        public PathTextBoxForm()
        {
            PathTextBox = new AdvancedPathTextBox
            {
                Location = new Point(0, 0),
                Size = new Size(ClientSize.Width, 24),
                PathFormatHelpButtonEnabled = false
            };

            TextBox.TextChanged += OnTextBoxTextChanged;
            
            if (TextBox is FormatPathTextBox format)
            {
                format.CheckWellFormed = false;
                
                TextBox.TextChanged += (sender, args) =>
                {
                    ApplyButton.Enabled = format.ValidateFunc?.Invoke(format.Text) != false;
                };
            }
            PathTextBox.PathDialogButton.PathBeenSelected += path => TextBox.Text = path;

            Shown += OnShown;
            
            Controls.Add(PathTextBox);
        }

        public override void OnTextBoxTextChanged(Object sender, EventArgs e)
        {
            ReturnValue = TextBox.Text;
        }

        protected virtual void OnShown(Object sender, EventArgs e)
        {
            if (OpenDialogOnEmpty && TextBox.Text.IsNullOrEmpty())
            {
                PathTextBox.PathDialogButton.PerformClick();
            }
        }
    }
}