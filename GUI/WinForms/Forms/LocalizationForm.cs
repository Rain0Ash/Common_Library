// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using Common_Library.Localization;

namespace Common_Library.GUI.WinForms.Forms
{
    public abstract class LocalizationForm : FixedForm
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LocalizationBase.LanguageChanged += OnUpdateText;
            OnUpdateText();
        }

        private void OnUpdateText()
        {
            SuspendLayout();
            UpdateText();
            ResumeLayout(false);
            PerformLayout();
        }

        protected virtual void UpdateText()
        {
            //override
        }

        protected override void Dispose(Boolean disposing)
        {
            LocalizationBase.LanguageChanged -= OnUpdateText;
            base.Dispose(disposing);
        }
    }
}