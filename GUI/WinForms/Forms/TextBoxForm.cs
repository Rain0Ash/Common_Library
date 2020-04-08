// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Windows.Forms;
using Common_Library.GUI.WinForms.TextBoxes;
using Common_Library.Interfaces;

namespace Common_Library.GUI.WinForms.Forms
{
    public class TextBoxForm : ParentForm
    {
        protected Control Control;

        public virtual ValidableTextBox TextBox
        {
            get
            {
                return Control as ValidableTextBox;
            }
            set
            {
                Control = value;
            }
        }

        public Button ApplyButton { get; }
        public Button DenyButton { get; }
        
        public TextBoxForm()
        {
            ApplyButton = new Button();
            DenyButton = new Button();
            TextBox = new ValidableTextBox();

            if (TextBox != null)
            {
                TextBox.Location = new Point(0, 0);
                TextBox.Size = new Size(500, 24);
                TextBox.TextChanged += OnTextBoxTextChanged;
                Controls.Add(TextBox);
            }

            ApplyButton.Text = @"Apply";
            ApplyButton.Location = new Point(1, 24);
            ApplyButton.Size = new Size(244, 28);
            ApplyButton.Click += (sender, args) =>
            {
                DialogResult = DialogResult.OK;
                Close();
            };

            DenyButton.Text = @"Cancel";
            DenyButton.Location = new Point(255, 24);
            DenyButton.Size = new Size(244, 28);
            DenyButton.Click += (sender, args) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            
            Text = @"Write value";

            ClientSize = new Size(500, 54);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            AcceptButton = ApplyButton;
            CancelButton = DenyButton;
            MaximizeBox = false;
            
            Controls.Add(ApplyButton);
            Controls.Add(DenyButton);
        }

        public virtual void OnTextBoxTextChanged(Object sender, EventArgs e)
        {
            ReturnValue = TextBox.Text;
            ApplyButton.Enabled = TextBox.CheckValidFormat();
        }
    }
}