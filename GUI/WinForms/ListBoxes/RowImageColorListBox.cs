// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Common_Library.GUI.WinForms.ListBoxes
{
    public class RowImageColorListBox : FixedListBox
    {
        public new DrawMode DrawMode
        {
            get
            {
                return base.DrawMode;
            }
            private set
            {
                base.DrawMode = value;
            }
        }

        private Brush _defaultTextBrush = Brushes.Black;

        public Brush DefaultTextBrush
        {
            get
            {
                return _defaultTextBrush;
            }
            set
            {
                if (_defaultTextBrush == value)
                {
                    return;
                }
                
                _defaultTextBrush = value ?? Brushes.Black;
                
                RefreshItems();
            }
        }
        
        private Brush _defaultBackgroundBrush = Brushes.White;

        public Brush DefaultBackgroundBrush
        {
            get
            {
                return _defaultBackgroundBrush;
            }
            set
            {
                if (_defaultBackgroundBrush == value)
                {
                    return;
                }
                
                _defaultBackgroundBrush = value ?? Brushes.White;
                
                RefreshItems();
            }
        }
        
        private Image _defaultImage;

        public Image DefaultImage
        {
            get
            {
                return _defaultImage;
            }
            set
            {
                if (_defaultImage == value)
                {
                    return;
                }

                _defaultImage = value;
                
                RefreshItems();
            }
        }

        private Boolean _firstByIndex = true;
        public Boolean FirstByIndex
        {
            get
            {
                return _firstByIndex;
            }
            set
            {
                if (_firstByIndex == value)
                {
                    return;
                }

                _firstByIndex = value;
                
                RefreshItems();
            }
        }

        public new Int32 SelectedIndex
        {
            get
            {
                return base.SelectedIndex;
            }
            set
            {
                Int32 index = base.SelectedIndex;
                if (index == value)
                {
                    return;
                }

                base.SelectedIndex = value;
                RefreshItem(index);
                RefreshItem(value);
            }
        }

        private readonly EventDictionary<Object, Image> _itemsImageDictionary = new EventDictionary<Object, Image>();
        private readonly EventDictionary<Int32, Image> _indexImageDictionary = new EventDictionary<Int32, Image>();
        private readonly EventDictionary<Object, (Brush, Brush)> _itemsColorDictionary = new EventDictionary<Object, (Brush, Brush)>();
        private readonly EventDictionary<Int32, (Brush, Brush)> _indexColorDictionary = new EventDictionary<Int32, (Brush, Brush)>();

        public RowImageColorListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            _itemsImageDictionary.ItemsChanged += Refresh;
            _indexImageDictionary.ItemsChanged += Refresh;
            _itemsColorDictionary.ItemsChanged += Refresh;
            _indexColorDictionary.ItemsChanged += Refresh;
        }

        protected virtual void Draw(DrawItemEventArgs e, Image image, (Brush, Brush)? colors)
        {
            Int32 index = e.Index;

            if (!IndexInItems(index))
            {
                return;
            }
            
            Object item = Items[index];
            (Brush backgroundColor, Brush textColor) color = colors ?? (DefaultBackgroundBrush, DefaultTextBrush);

            if (FirstByIndex)
            {
                if (image == null)
                {
                    if (!_indexImageDictionary.TryGetValue(index, out image))
                    {
                        if (!_itemsImageDictionary.TryGetValue(item, out image))
                        {
                            image = DefaultImage;
                        }
                    }
                }

                if (colors == null)
                {
                    if (!_indexColorDictionary.TryGetValue(index, out color))
                    {
                        if (!_itemsColorDictionary.TryGetValue(item, out color))
                        {
                            color = (DefaultBackgroundBrush, DefaultTextBrush);
                        }
                    }
                }
            }
            else
            {
                if (image == null)
                {
                    if (!_itemsImageDictionary.TryGetValue(item, out image))
                    {
                        if (!_indexImageDictionary.TryGetValue(index, out image))
                        {
                            image = DefaultImage;
                        }
                    }
                }

                if (colors == null)
                {
                    if (!_itemsColorDictionary.TryGetValue(item, out color))
                    {
                        if (!_indexColorDictionary.TryGetValue(index, out color))
                        {
                            color = (DefaultBackgroundBrush, DefaultTextBrush);
                        }
                    }
                }
            }
            
            OnDrawItem(e, image, color);
        }

        protected virtual void OnDrawItem(DrawItemEventArgs e, Image image, (Brush, Brush) color)
        {
            Int32 index = e.Index;
            if (!IndexInItems(index))
            {
                return;
            }
            
            Object item = Items[index];
            Graphics graphics = e.Graphics;
            Rectangle bounds = e.Bounds;

            (Brush backgroundColor, Brush textColor) = color;

            graphics.FillRectangle(backgroundColor ?? DefaultBackgroundBrush, bounds);

            if (SelectedIndices.Count > 0 && SelectedIndices.Contains(index))
            {
                Pen pen = Pens.Red;
                graphics.DrawLine(pen, new Point(bounds.X, bounds.Y), new Point(bounds.Width - 1, bounds.Y));
            
                graphics.DrawLine(pen, new Point(bounds.X, bounds.Height), new Point(bounds.Width - 1, bounds.Height));
                
                graphics.DrawLine(pen, new Point(bounds.X, bounds.Y), new Point(bounds.X, bounds.Height));
                graphics.DrawLine(pen, new Point(bounds.Width - 1, bounds.Y), new Point(bounds.Width - 1, bounds.Height));
            }

            if (image != null)
            {
                graphics.DrawImage(image, new Rectangle(bounds.X, bounds.Y, bounds.Height, bounds.Height));
                
                graphics.DrawString(item.ToString(), e.Font, textColor ?? DefaultTextBrush, new Rectangle(bounds.X + bounds.Height, bounds.Y, bounds.Width - bounds.Height, bounds.Height), StringFormat.GenericDefault);
            }
            else
            {
                graphics.DrawString(item.ToString(), e.Font, textColor ?? DefaultTextBrush, bounds, StringFormat.GenericDefault);
            }
        }
        
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            
            e.DrawBackground();

            Draw(e, null, null);

            e.DrawFocusRectangle();
        }

        public void SetColorByIndex(Int32 index, Brush backgroundColor, Brush textColor = default)
        {
            _indexColorDictionary.Set(index, (backgroundColor, textColor));
            RefreshItem(index);
        }
        
        public void SetColor(Object item, Brush backgroundColor, Brush textColor = default)
        {
            _itemsColorDictionary.Set(item, (backgroundColor, textColor));
            RefreshItem(Items.IndexOf(item));
        }

        public void RemoveColor(Int32 index)
        {
            _indexColorDictionary.Remove(index);
            RefreshItem(index);
        }

        public void RemoveColor(Object item)
        {
            _itemsColorDictionary.Remove(item);
            RefreshItem(Items.IndexOf(item));
        }
        
        public void SetImageByIndex(Int32 index, Image image)
        {
            _indexImageDictionary.Set(index, image);
            RefreshItem(index);
        }
        
        public void SetImage(Object item, Image image)
        {
            _itemsImageDictionary.Set(item, image);
            RefreshItem(Items.IndexOf(item));
        }

        public void RemoveImage(Int32 index)
        {
            _indexImageDictionary.Remove(index);
            RefreshItem(index);
        }

        public void RemoveImage(Object item)
        {
            _itemsImageDictionary.Remove(item);
            RefreshItem(Items.IndexOf(item));
        }
    }
}