// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Common_Library.Utils.Math;

namespace Common_Library.GUI.WinForms.ListBoxes
{
    public class EventListBox : RowImageColorListBox
    {
        public Boolean OverlapAllowed { get; set; } = false;
        public event Handlers.IndexObjectHandler ItemAdded;
        public event Handlers.IndexObjectHandler ItemSourceAdded;
        public event Handlers.IndexObjectHandler ItemRemoved;
        public event Handlers.IndexObjectHandler ItemSourceRemoved;
        public event Handlers.EmptyHandler ItemsChanged;

        private void AddToItems(Object item)
        {
            if (!OverlapAllowed && Items.OfType<Object>().Any(item.Equals))
            {
                return;
            }

            Items.Add(item);
            ItemAdded?.Invoke(Items.Count - 1, item);
            ItemsChanged?.Invoke();
        }

        private void AddToSource(Object item)
        {
            /*
            if (!(DataSource is List<Object> source))
            {
                return;
            }

            if (!OverlapAllowed && source.Any(item.Equals))
            {
                return;
            }
            
            source.Add(item);
            ItemAdded?.Invoke(item);
            ItemsChanged?.Invoke();
            */

            ItemSourceAdded?.Invoke(-1, item);
        }

        public void Add(Object item)
        {
            if (DataSource != null)
            {
                AddToSource(item);
                return;
            }

            AddToItems(item);
        }

        public void Add(Object item, Image image)
        {
            Add(item);
            SetImage(item, image);
        }

        public void Add(Object item, Brush color)
        {
            Add(item);
            SetColor(item, color);
        }

        public void Add(Object item, Image image, Brush color)
        {
            Add(item, image);
            SetColor(item, color);
        }

        public void Add(Object item, Brush backgroundColor, Brush textColor)
        {
            Add(item);
            SetColor(item, backgroundColor, textColor);
        }

        public void Add(Object item, Image image, Brush backgroundColor, Brush textColor)
        {
            Add(item, image);
            SetColor(item, backgroundColor, textColor);
        }

        private void InsertToItems(Object item, Int32 index = 0)
        {
            if (!OverlapAllowed && Items.OfType<Object>().Any(item.Equals))
            {
                return;
            }

            index = MathUtils.ToRange(index, 0, Items.Count);
            Items.Insert(index, item);
            ItemAdded?.Invoke(index, item);
            ItemsChanged?.Invoke();
        }

        private void InsertToSource(Object item, Int32 index = 0)
        {
            /*
            if (!(DataSource is List<Object> source))
            {
                return;
            }
            
            if (!OverlapAllowed && source.Any(item.Equals))
            {
                return;
            }
            
            source.Insert(MathUtils.Range(index, 0, source.Count), item);
            ItemAdded?.Invoke(item);
            ItemsChanged?.Invoke();
            */

            ItemSourceAdded?.Invoke(index, item);
        }

        public void Insert(Object item, Int32 index = 0)
        {
            if (DataSource != null)
            {
                InsertToSource(item, index);
                return;
            }

            InsertToItems(item, index);
        }

        public void Insert(Object item, Image image, Int32 index = 0)
        {
            Insert(item, index);
            SetImage(item, image);
        }

        public void Insert(Object item, Int32 index, Brush color)
        {
            Insert(item, index);
            SetColor(item, color);
        }

        public void Insert(Object item, Image image, Int32 index, Brush color)
        {
            Insert(item, image, index);
            SetColor(item, color);
        }

        public void Insert(Object item, Int32 index, Brush backgroundColor, Brush textColor)
        {
            Insert(item, index);
            SetColor(item, backgroundColor, textColor);
        }

        public void Insert(Object item, Image image, Int32 index, Brush backgroundColor, Brush textColor)
        {
            Insert(item, image, index);
            SetColor(item, backgroundColor, textColor);
        }

        private void RemoveFromItems(Object item)
        {
            RemoveAt(Items.IndexOf(item));
        }

        private void RemoveFromSource(Object item)
        {
            /*
            if (!(DataSource is List<Object> source))
            {
                return;
            }
            
            RemoveAt(source.IndexOf(item));
            */

            ItemSourceRemoved?.Invoke(-1, item);
        }

        public void Remove(Object item)
        {
            if (DataSource != null)
            {
                RemoveFromSource(item);
                return;
            }

            RemoveFromItems(item);
        }

        public void Remove(IEnumerable<Object> items)
        {
            foreach (Object item in items)
            {
                Remove(item);
            }
        }

        private void RemoveAtFromItems(Int32 index)
        {
            try
            {
                Object item = Items[index];
                Items.RemoveAt(index);
                RemoveColor(index);
                RemoveColor(item);
                RemoveImage(index);
                RemoveImage(item);

                ItemRemoved?.Invoke(index, item);
                ItemsChanged?.Invoke();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private void RemoveAtFromSource(Int32 index)
        {
            /*
            if (!(DataSource is List<Object> source))
            {
                return;
            }
            
            try
            {
                Object item = source[index];
                source.RemoveAt(index);
                RemoveColor(index);
                RemoveColor(item);
                RemoveImage(index);
                RemoveImage(item);

                ItemRemoved?.Invoke(item);
                ItemsChanged?.Invoke();
            }
            catch (ArgumentOutOfRangeException)
            {
                
            }
            */

            ItemSourceRemoved?.Invoke(index, null);
        }

        public void RemoveAt(SelectedIndexCollection indexes)
        {
            if (DataSource != null)
            {
                foreach (Int32 index in indexes.OfType<Int32>().OrderByDescending(i => i))
                {
                    RemoveAt(index);
                }

                return;
            }

            Remove(new List<Object>(Items.OfType<Object>()).Where(item => indexes.Contains(Items.IndexOf(item))));
        }

        public void RemoveAt(Int32 index)
        {
            if (!IndexInItems(index))
            {
                return;
            }

            if (DataSource != null)
            {
                RemoveAtFromSource(index);
                return;
            }

            RemoveAtFromItems(index);
        }

        public void AddRange(IEnumerable<Object> items)
        {
            foreach (Object item in items)
            {
                Add(item);
            }
        }
    }
}