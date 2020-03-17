// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using Common_Library.Utils.GUI;

namespace Common_Library.GUI.WinForms.Forms
{
    public class FlashForm : FixedForm
    {
        public void NotifyFlash()
        {
            FormUtils.NotifyFlash(this);
        }
        
        public void StartFlash(UInt32 count = UInt32.MaxValue)
        {
            FormUtils.StartFlash(this, count);
        }
        
        public void StopFlash()
        {
            FormUtils.StopFlash(this);
        }
    }
}