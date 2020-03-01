// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Utils;
using Microsoft.WindowsAPICodePack.Dialogs;
using Directory = Common_Library.LongPath.Directory;

namespace Common_Library.GUI.WinForms.Buttons
{
    public sealed class DialogButton : LocalizationButton
    {
        private readonly Control _linkedControl;

        public event Handlers.StringHandler PathBeenSelected;
        public event Handlers.EnumerableHandler PathsBeenSelected;

        public Boolean AddSeparatorToPickedFolder { get; set; } = true;
        
        private CommonOpenFileDialog _openFileDialog;

        public CommonOpenFileDialog OpenFileDialog
        {
            get
            {
                return _openFileDialog;
            }
            private set
            {
                if (value == null || _openFileDialog == value)
                {
                    return;
                }

                _openFileDialog = value;
            }
        }

        public DialogButton()
            : this(null)
        {
        }
        
        public DialogButton(Control control)
        {
            OpenFileDialog = new CommonOpenFileDialog
            {
                Multiselect = false,
                IsFolderPicker = true,
            };
            
            _linkedControl = control;
            Text = @"...";
            TextAlign = ContentAlignment.MiddleCenter;
        }

        private String CheckLinkedControlText()
        {
            String fullPath = PathUtils.GetFullPath(_linkedControl?.Text) ?? Directory.GetCurrentDirectory();

            if (Directory.Exists(fullPath))
            {
                return fullPath;
            }

            fullPath = StringUtils.BeforeFormatVariables(fullPath);
            if (!String.IsNullOrEmpty(fullPath) && Directory.Exists(fullPath))
            {
                return fullPath;
            }

            return Directory.Exists(OpenFileDialog.InitialDirectory) ? OpenFileDialog.InitialDirectory : Directory.GetCurrentDirectory();
        }
        
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            OpenFileDialog.InitialDirectory = CheckLinkedControlText();

            if (OpenFileDialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            Boolean addSeparator = OpenFileDialog.IsFolderPicker && AddSeparatorToPickedFolder;
            
            if (OpenFileDialog.Multiselect)
            {
                PathsBeenSelected?.Invoke(addSeparator ? OpenFileDialog.FileNames.Select(PathUtils.ConvertToFolder) : OpenFileDialog.FileNames);
            }
            else
            {
                PathBeenSelected?.Invoke(addSeparator ? PathUtils.ConvertToFolder(OpenFileDialog.FileName) : OpenFileDialog.FileName);
            }
        }

        protected override void Dispose(Boolean disposing)
        {
            PathBeenSelected = null;
            PathsBeenSelected = null;
            OpenFileDialog.Dispose();
            base.Dispose(disposing);
        }
    }
}