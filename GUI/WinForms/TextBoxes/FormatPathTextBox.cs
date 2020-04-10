// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common_Library;
using Common_Library.Attributes;
using Common_Library.GUI.WinForms.TextBoxes;
using Common_Library.Utils;
using Common_Library.Utils.IO;

namespace System.Windows.Forms
{
    public class FormatPathTextBox : PathTextBox
    {
        public IEnumerable<FormatedField> AvailableFormatingPartsGroupList;

        public event Handlers.EmptyHandler AvailableFormatingPartsChanged;

        private OrderedSet<String> _availableFormatingParts = new OrderedSet<String>();

        public OrderedSet<String> AvailableFormatingParts
        {
            get
            {
                return _availableFormatingParts;
            }
            private set
            {
                if (Equals(_availableFormatingParts, value))
                {
                    return;
                }

                _availableFormatingParts = value?.ToOrderedSet();
                AvailableFormatingPartsChanged?.Invoke();
            }
        }

        public Boolean EnableUniquenessFormatingParts { get; set; } = true;

        public event Handlers.EmptyHandler UniqueFormatingPartsChanged;

        private OrderedSet<String> _uniqueFormatingParts = new OrderedSet<String>();

        public OrderedSet<String> UniqueFormatingParts
        {
            get
            {
                return _uniqueFormatingParts;
            }
            private set
            {
                if (Equals(_uniqueFormatingParts, value))
                {
                    return;
                }

                if (value != null && value.Any() && value.Intersect(AvailableFormatingParts).Count() != value.Count())
                {
                    throw new ArgumentException();
                }

                _uniqueFormatingParts = value?.ToOrderedSet();
                UniqueFormatingPartsChanged?.Invoke();
            }
        }

        public FormatPathTextBox(IReflect type = null)
        {
            WellFormedCheck = true;
            UpdateAvailableFormatingParts(type);

            AvailableFormatingPartsChanged += ItemValidateColor;
            UniqueFormatingPartsChanged += ItemValidateColor;
        }

        public void UpdateAvailableFormatingParts(IReflect type)
        {
            if (type == null)
            {
                return;
            }

            AvailableFormatingPartsGroupList = type
                .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(members => members.GetCustomAttributes<FormatedField>())
                .SelectMany(members => members);

            foreach (FormatedField attr in AvailableFormatingPartsGroupList)
            {
                foreach (String linkedName in attr.LinkedNames)
                {
                    if (String.IsNullOrEmpty(linkedName))
                    {
                        continue;
                    }


                    AvailableFormatingParts.Add(linkedName);

                    if (attr.Uniqueness)
                    {
                        UniqueFormatingParts.Add(linkedName);
                    }

                    foreach (String attribute in attr.Attributes)
                    {
                        String str = linkedName + ":" + attribute;

                        AvailableFormatingParts.Add(str);

                        if (attr.Uniqueness)
                        {
                            UniqueFormatingParts.Add(str);
                        }
                    }
                }
            }
        }

        public override Boolean WellFormedValidate()
        {
            Boolean check = base.WellFormedValidate();

            if (!check)
            {
                return false;
            }

            if (AvailableFormatingParts?.Any() != true)
            {
                return true;
            }

            String[] linkedNames = StringUtils.GetFormatVariables(Text).ToArray();

            check &= linkedNames.All(format => AvailableFormatingParts.Contains(format));

            if (EnableUniquenessFormatingParts && UniqueFormatingParts?.Any() == true)
            {
                check &= linkedNames.Any(format => UniqueFormatingParts.Contains(format));
            }

            return check;
        }
    }
}