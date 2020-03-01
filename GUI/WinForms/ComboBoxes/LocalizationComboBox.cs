// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Localization;

namespace Common_Library.GUI.WinForms.ComboBoxes
{
    public sealed class LocalizationComboBox : ImagedComboBox
    {
        public event Handlers.StringHandler LanguageChanged;
        private readonly LocalizationBase _localizationBase;

        public LocalizationComboBox(LocalizationBase localization)
        {
            _localizationBase = localization ?? throw new NullReferenceException();
            BindingContext = new BindingContext();
            DataSource = GetItemsForDataSource();
            LocalizationBase.LanguageChanged += () => SetLanguage();
            LanguageChanged += LocalizationBase.UpdateLocalization;
        }

        protected override void UpdateText()
        {
            //pass
        }

        private DropDownItem[] GetItemsForDataSource()
        {
            return _localizationBase.GetCultures()
                .Select(culture => new DropDownItem(culture.CultureName) {Image = culture.CultureImage}).ToArray();
        }

        public void SetLanguage(String languageCodeOrName = null)
        {
            Int32 selectedIndex = _localizationBase.GetLanguageID(languageCodeOrName ?? LocalizationBase.GetLocalizationCultureCode());
            if (selectedIndex > -1)
            {
                SelectedIndex = selectedIndex;
                return;
            }
            
            SelectedIndex = -1;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible && !Disposing)
            {
                SetLanguage();
            }
        }

        public String GetLanguageCode()
        {
            return CountryData.LanguageNameByISO2.FirstOrDefault(x => x.Value.CustomName == Text).Key?.ToLower() ?? LocalizationBase.DefaultCultureCode;
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            LanguageChanged?.Invoke(GetLanguageCode());
        }

        protected override void Dispose(Boolean disposing)
        {
            LanguageChanged = null;
            base.Dispose(disposing);
        }
    }
}