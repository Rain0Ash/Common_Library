﻿﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Windows.Forms;
using Common_Library.Localization;

namespace Common_Library.GUI.WinForms.ComboBoxes
{
    public class ImagedComboBox : LanguageComboBox
    {
        public ImagedComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();

            e.DrawFocusRectangle();

            if (e.Index >= 0 && e.Index < Items.Count)
            {
                DropDownItem item = (DropDownItem) Items[e.Index];

                e.Graphics.DrawImage(item.Image, e.Bounds.Left, e.Bounds.Top);

                e.Graphics.DrawString(item.Value, e.Font, new SolidBrush(e.ForeColor), e.Bounds.Left + item.Image.Width, e.Bounds.Top + 2);
            }

            base.OnDrawItem(e);
        }
    }
    public sealed class DropDownItem
    {
        public CultureStringsBase Value { get; }

        public Image Image { get; set; }

        public DropDownItem()
            : this(String.Empty)
        { }

        public DropDownItem(String text)
            : this(new CultureStringsBase(text))
        {
        }

        public DropDownItem(CultureStringsBase text)
        {
            Value = text;
            Image = new Bitmap(16, 16);
            using Graphics graphics = Graphics.FromImage(Image);
            using Brush brush = new SolidBrush(Color.FromName(text));
            graphics.DrawRectangle(Pens.White, 0, 0, Image.Width, Image.Height);
            graphics.FillRectangle(brush, 1, 1, Image.Width - 1, Image.Height - 1);
        }

        public override String ToString()
        {
            return Value;
        }
    }
}
