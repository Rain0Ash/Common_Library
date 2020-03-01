// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Common_Library.GUI.WinForms.Forms;
using Common_Library.GUI.WinForms.ToolTips;

namespace System.Windows.Forms
{
    public class HelpForm : LocalizationForm
    {
        private readonly HelpToolTip _helpToolTip;

        public HelpForm()
        {
            _helpToolTip = new HelpToolTip
            {
                OwnerDraw = true,
            };
        }
        
        public void SetMessage(Control control, String message)
        {
            if (!Controls.Contains(control))
            {
                return;
            }
            
            _helpToolTip.SetToolTip(control, message);
        }

        public void RemoveMessage(Control control)
        {
            _helpToolTip.SetToolTip(control, null);
        }

        public void RemoveAllMessages()
        {
            _helpToolTip.RemoveAll();
        }

        protected override void Dispose(Boolean disposing)
        {
            _helpToolTip.Dispose();
            base.Dispose(disposing);
        }
    }
}