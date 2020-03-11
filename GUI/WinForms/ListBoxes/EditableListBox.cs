// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
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
                case Keys.Delete:
                    if (!ActionType.HasFlag(ActionType.Remove))
                    {
                        return;
                    }

                    try
                    {
                        foreach (Int32 index in SelectedIndices)
                        {
                            RemoveAt(index);
                        }
                    }
                    catch (Exception)
                    {
                        //ignore
                    }

                    e.Handled = true;
                    return;
                default:
                    break;
            }

            base.OnKeyDown(e);
        }
    }
}