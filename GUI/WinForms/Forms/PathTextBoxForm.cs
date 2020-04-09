// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Windows.Forms;
using Common_Library.GUI.WinForms.TextBoxes;
using Common_Library.Utils;

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
            
            if (TextBox is FormatPathTextBox textBox)
            {
                textBox.CheckWellFormed = false;
            }

            PathTextBox.PathDialogButton.PathBeenSelected += path => TextBox.Text = path;

            Shown += OnShown;
            
            Controls.Add(PathTextBox);
        }

        protected override void OnTextBoxTextChanged(Object sender, EventArgs e)
        {
            ReturnValue = TextBox.Text;
            ApplyButton.Enabled = !TextBox.Text.IsNullOrEmpty() && TextBox.ValidateFunc?.Invoke(TextBox.Text) != false;
        }

        protected virtual void OnShown(Object sender, EventArgs e)
        {
            OnTextBoxTextChanged(null, EventArgs.Empty);
            
            if (OpenDialogOnEmpty && TextBox.Text.IsNullOrEmpty())
            {
                PathTextBox.PathDialogButton.PerformClick();
            }
        }
    }
}