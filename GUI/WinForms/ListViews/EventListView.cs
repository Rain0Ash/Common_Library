// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Windows.Forms;

namespace Common_Library.GUI.WinForms.ListViews
{
    public class EventListView : RowImageColorListView
    {
        public Int32 MultiClickCount { get; set; } = 3;

        public MouseButtons MouseButtons { get; set; } = MouseButtons.Left;
        
        public event Handlers.KeyValueHandler<ListViewItem, MouseEventArgs> ItemClick;
        public event Handlers.KeyValueHandler<ListViewItem, MouseEventArgs> ItemDoubleClick;
        public event Handlers.TypeHandler<MouseEventArgs> EmptyClick; 
        public event Handlers.TypeHandler<MouseEventArgs> EmptyDoubleClick; 
        
        public event Handlers.IndexObjectHandler ItemAdded;
        public event Handlers.ObjectHandler ItemRemoved;
        public event Handlers.EmptyHandler ItemsChanged;

        public EventListView()
        {
            MouseDown += OnMouseClick;
        }
        
        public override void Insert(Int32 index, Object item)
        {
            base.Insert(index, item);
            ItemAdded?.Invoke(index, item);
            ItemsChanged?.Invoke();
        }

        public override void Remove(Object item)
        {
            base.Remove(item);
            ItemRemoved?.Invoke(item);
            ItemsChanged?.Invoke();
        }

        private void OnItemClick(ListViewItem item, MouseEventArgs e)
        {
            switch (e.Clicks)
            {
                case 1:
                    ItemClick?.Invoke(item, e);
                    break;
                case 2:
                    ItemDoubleClick?.Invoke(item, e);
                    break;
                default:
                    break;
            }
        }
        
        private void OnEmptyClick(MouseEventArgs e)
        {
            switch (e.Clicks)
            {
                case 1:
                    EmptyClick?.Invoke(e);
                    break;
                case 2:
                    EmptyDoubleClick?.Invoke(e);
                    break;
                default:
                    break;
            }
            
            SelectedItems.Clear();
        }
        
        public void OnMouseClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons)
            {
                return;
            }
            
            ListViewItem item = HitTest(e.X, e.Y).Item;

            if (item != null)
            {
                OnItemClick(item, e);
                return;
            }
            
            OnEmptyClick(e);
        }
    }
}