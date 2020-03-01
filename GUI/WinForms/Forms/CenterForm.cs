// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.GUI.WinForms.Forms
{
    public abstract class CenterForm : LocalizationForm
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CenterTo();
        }
        
        protected virtual void CenterTo()
        {
            CenterToScreen();
        }
    }
}