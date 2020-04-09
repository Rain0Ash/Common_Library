// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Drawing;
using System.Windows.Forms;
using Common_Library.Utils.GUI.ListView;

namespace Common_Library.GUI.WinForms.ListViews.Items
{
    public class FixedListViewItem : ListViewItem
    {
        public Image Image
        {
            get
            {
                return this.GetImage();
            }
            set
            {
                this.SetImage(value);
            }
        }
    }
}