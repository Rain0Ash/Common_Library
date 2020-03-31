// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Utils;

namespace Common_Library.GUI.WinForms.ListBoxes
{
    public class EditableListBox : ValidableListBox
    {
        public ActionType ActionType { get; set; } = ActionType.All;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete when ActionType.HasFlag(ActionType.Remove):
                    try
                    {
                        foreach (Int32 index in SelectedIndices)
                        {
                            RemoveAt(index);
                        }
                    }
                    catch (Exception)
                    {
                        //ignored
                    }

                    e.Handled = true;
                    return;
                case Keys.Up when ActionType.HasFlag(ActionType.Swap) && e.Shift:
                    foreach (Int32 index in SelectedIndices.OfType<Int32>().Sort())
                    {
                        ListUtils.TrySwap(Items, index, index - 1);
                    }
                    
                    break;
                case Keys.Down when ActionType.HasFlag(ActionType.Swap) && e.Shift:
                    foreach (Int32 index in SelectedIndices.OfType<Int32>().Sort())
                    {
                        ListUtils.TrySwap(Items, index, index + 1);
                    }
                    
                    break;
                default:
                    break;
            }

            base.OnKeyDown(e);
        }
    }
}