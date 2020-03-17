// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Localization;
using Common_Library.Utils;
using Common_Library.Utils.Math;

namespace Common_Library.GUI.WinForms.ComboBoxes
{
    public sealed class LocalizationComboBox : ImagedComboBox
    {
        public event Handlers.Int32Handler LanguageChanged;
        private readonly LocalizationBase _localizationBase;

        public LocalizationComboBox(LocalizationBase localization)
        {
            _localizationBase = localization ?? throw new NullReferenceException();
            BindingContext = new BindingContext();
            DataSource = GetItemsForDataSource();
            LocalizationBase.LanguageChanged += SetLanguage;
            LanguageChanged += lcid => LocalizationBase.UpdateLocalization(lcid, _localizationBase.AvailableLocalization);
            SetLanguage();
        }

        protected override void UpdateText()
        {
            //pass
        }

        private DropDownItem[] GetItemsForDataSource()
        {
            return _localizationBase.GetCultures()
                .Select(culture => new DropDownItem(culture.CustomName) {Image = culture.Image}).ToArray();
        }

        public void SetLanguage()
        {
            SetLanguage(LocalizationBase.CurrentCulture.LCID);
        }

        public void SetLanguage(Int32 lcid)
        {
            Int32 selectedIndex = LocalizationBase.GetLanguageOrderID(lcid);
            if (MathUtils.InRange(selectedIndex, MathUtils.Position.Left, 0, Items.Count))
            {
                SelectedIndex = selectedIndex;
                return;
            }

            SelectedIndex = LocalizationBase.GetLanguageOrderID(LocalizationBase.BasicCulture.LCID);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible && !Disposing)
            {
                SetLanguage();
            }
        }

        public Int32 GetLanguageLCID()
        {
            return LocalizationBase.CultureByLCID.FirstOr(
                x => String.Equals(x.Value.CustomName, Text, StringComparison.CurrentCultureIgnoreCase),
                new KeyValuePair<Int32, CultureInfoFixed>(LocalizationBase.DefaultCulture.LCID, LocalizationBase.DefaultCulture)).Key;
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            LanguageChanged?.Invoke(GetLanguageLCID());
        }

        protected override void Dispose(Boolean disposing)
        {
            LanguageChanged = null;
            base.Dispose(disposing);
        }
    }
}