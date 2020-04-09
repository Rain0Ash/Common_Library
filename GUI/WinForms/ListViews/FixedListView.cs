// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Windows.Forms;

namespace Common_Library.GUI.WinForms.ListViews
{
    public class FixedListView : ListView
    {
        public FixedListView()
        {
            DoubleBuffered = true;
        }

        public event Handlers.EmptyHandler ItemsChanged;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case 0x1007:
                    ItemsChanged?.Invoke();
                    break;
                case 0x104D:
                    ItemsChanged?.Invoke();
                    break;
                case 0x1008:
                    ItemsChanged?.Invoke();
                    break;
                case 0x1009:
                    ItemsChanged?.Invoke();
                    break;
                default:
                    break;
            }
        }

        protected void SwapItems(Int32 inx1, Int32 inx2)
        {
            ListViewItem item = Items[inx1];
            Items.Remove(item);
            Items.Insert(inx2, item);
        }
        
        public Boolean IndexInItems(Int32 index)
        {
            return index >= 0 && index < Items.Count;
        }
    }
}