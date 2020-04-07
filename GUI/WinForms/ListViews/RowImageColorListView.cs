// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Crypto;
using Common_Library.Types.Map;
using Common_Library.Utils;
using Common_Library.Utils.IO;

namespace Common_Library.GUI.WinForms.ListViews
{
    public class RowImageColorListView : ValidableListView
    {
        public Boolean OverlapAllowed { get; set; } = false;
        
        private Color _defaultForegroundColor = Color.Black;

        public Color DefaultForegroundColor
        {
            get
            {
                return _defaultForegroundColor;
            }
            set
            {
                if (_defaultForegroundColor == value)
                {
                    return;
                }

                _defaultForegroundColor = value.IsEmpty ? Color.Black : value;

                Refresh();
            }
        }

        private Color _defaultBackgroundColor = Color.White;

        public Color DefaultBackgroundColor
        {
            get
            {
                return _defaultBackgroundColor;
            }
            set
            {
                if (_defaultBackgroundColor == value)
                {
                    return;
                }

                _defaultBackgroundColor = value.IsEmpty ? Color.White : value;

                Refresh();
            }
        }
        
        protected readonly ImageList Images = new ImageList();
        protected readonly Map<Object, ListViewItem> ItemsDictionary = new Map<Object, ListViewItem>();
        protected readonly EventDictionary<Object, String> ImageDictionary = new EventDictionary<Object, String>();
        protected readonly EventDictionary<Object, (Color background, Color foreground, Font font)> ColorDictionary = new EventDictionary<Object, (Color background, Color foreground, Font font)>();

        public RowImageColorListView()
        {
            OwnerDraw = true;
            GridLines = true;
            FullRowSelect = true;
            Images.Images.Add("null", Common_Library.Images.Images.Basic.Null);
            SmallImageList = Images;
            DrawColumnHeader += OnDrawColumnHeader;
            DrawItem += OnDrawItem;
            DrawSubItem += OnDrawSubItem;
            SizeChanged += OnSizeChanged;
            ImageDictionary.ItemsChanged += Refresh;
            ColorDictionary.ItemsChanged += Refresh;
        }

        protected virtual void OnDrawItem(Object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            
            Int32 index = e.ItemIndex;
            ListViewItem lvitem = e.Item;
            
            if (!IndexInItems(index))
            {
                return;
            }

            Object item = ItemsDictionary.TryGetValue(lvitem, (Object) lvitem);

            (Color background, Color foreground, Font font) = ColorDictionary.TryGetValue(item, (DefaultBackgroundColor, DefaultForegroundColor, Font));
            
            lvitem.ImageKey = ImageDictionary.TryGetValue(item, "null");
            lvitem.BackColor = !CheckValidFormatItem(lvitem) ? InvalidColor : background.IsEmpty ? DefaultBackgroundColor : background;
            lvitem.ForeColor = foreground.IsEmpty ? DefaultForegroundColor : foreground;
            lvitem.Font = font ?? lvitem.Font;
            e.Graphics.FillRectangle(new SolidBrush(lvitem.BackColor), e.Item.Bounds);
        }

        public void OnDrawColumnHeader(Object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }
        
        public void OnDrawSubItem(Object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        public void SetColor(Object item, Color background, Color foreground, Font font = null)
        {
            ColorDictionary.Set(item, (background, foreground, font));
        }
        
        public void RemoveColor(Object item)
        {
            ColorDictionary.Remove(item);
        }

        public Image GetImage(Object item)
        {
            if (!ImageDictionary.TryGetValue(item, out String hash))
            {
                return null;
            }

            return Images.Images.ContainsKey(hash) ? Images.Images[hash] : null;
        }
        
        public void SetImage(Object item, Image image)
        {
            String hash = image.Hash(HashType.MD5).ToByteString();

            if (!Images.Images.ContainsKey(hash))
            {
                Images.Images.Add(hash, image);
            }
            
            ImageDictionary.Set(item, hash);
        }
        
        public void RemoveImage(Object item)
        {
            if (!ImageDictionary.TryGetValue(item, out String hash))
            {
                return;
            }

            ImageDictionary.Remove(item);
            
            if (hash != null && !hash.Equals("null", StringComparison.OrdinalIgnoreCase)
                             && !ImageDictionary.Any(pair => pair.Value.Equals(hash, StringComparison.OrdinalIgnoreCase)))
            {
                Images.Images.RemoveByKey(hash);
            }
        }

        public void Add(Object item)
        {
            Insert(Items.Count, item);
        }

        public void Add(Object item, Image image)
        {
            Add(item);
            SetImage(item, image);
        }

        public void Add(Object item, Image image, Color background, Color foreground, Font font = null)
        {
            Add(item, image);
            SetColor(item, background, foreground, font);
        }

        public virtual void Insert(Int32 index, Object item)
        {
            if (!(item is ListViewItem lvitem))
            {
                lvitem = ItemsDictionary.GetOrAdd(item, new ListViewItem(item.ToString()));
            }
            
            if (!OverlapAllowed && Items.OfType<Object>().Any(item.Equals))
            {
                return;
            }

            if (index < Items.Count)
            {
                Items.Insert(index, lvitem);
                return;
            }

            Items.Add(lvitem);
        }
        
        public void Insert(Int32 index, Object item, Image image)
        {
            Insert(index, item);
            SetImage(item, image);
        }

        public void Insert(Int32 index, Object item, Image image, Color background, Color foreground, Font font = null)
        {
            Insert(index, item, image);
            SetColor(item, background, foreground, font);
        }

        public virtual void Remove(Object item)
        {
            if (!(item is ListViewItem lvitem))
            {
                lvitem = ItemsDictionary.TryGetValue(item);
            }
            else
            {
                item = ItemsDictionary.TryGetValue(lvitem, item);
            }

            if (lvitem == null)
            {
                return;
            }
            
            Items.Remove(lvitem);

            if (item == null)
            {
                return;
            }
            
            RemoveImage(item);
            RemoveColor(item);
        }
        
        public void Remove(IEnumerable<Object> items)
        {
            foreach (Object item in items)
            {
                Remove(item);
            }
        }

        public void RemoveAt(IEnumerable<Int32> indexes)
        {
            foreach (ListViewItem item in indexes.Where(IndexInItems).Select(index => Items[index]))
            {
                Remove(item);
            }
        }
        
        public void RemoveAt(SelectedIndexCollection indexes)
        {
            RemoveAt(indexes.OfType<Int32>());
        }

        public void RemoveAt(Int32 index)
        {
            if (!IndexInItems(index))
            {
                return;
            }
            
            Remove(Items[index]);
        }

        public void AddRange(IEnumerable<Object> items)
        {
            foreach (Object item in items)
            {
                Add(item);
            }
        }
        
        public void InsertRange(Int32 index, IEnumerable<Object> items)
        {
            foreach (Object item in items)
            {
                Insert(index++, item);
            }
        }
        
        public void OnSizeChanged(Object sender, EventArgs e)
        {
            if (Columns.Count == 1)
            {
                Columns[0].Width = ClientSize.Width;
            }
        }
    }
}