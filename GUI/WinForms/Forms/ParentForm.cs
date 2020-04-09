// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Common_Library.GUI.WinForms.Forms
{
    public abstract class ParentForm : CenterForm
    {
        protected ParentForm()
        {
            Icon = Owner?.Icon;
            ShowInTaskbar = Owner == null;
        }
        
        protected override void CenterTo()
        {
            CenterToParent();
        }
    }
}