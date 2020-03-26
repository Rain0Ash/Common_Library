// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Drawing;
using Common_Library.Images.icons.blue;
using Common_Library.Images.icons.circular;
using Common_Library.Images.icons.fill;
using Common_Library.Images.icons.flat;
using Common_Library.Images.icons.gradient;
using Common_Library.Images.icons.line;
using Common_Library.Images.icons.lineal;
using Common_Library.Images.icons.other;

namespace Common_Library.Images
{
    public static class Images
    {
        public static class Basic
        {
            public static readonly Image Null = OtherImages._null;

            public static readonly Image Application = SystemIcons.Application.ToBitmap();

            public static readonly Image Asterisk = SystemIcons.Asterisk.ToBitmap();

            public static readonly Image Error = SystemIcons.Error.ToBitmap();

            public static readonly Image Exclamation = SystemIcons.Exclamation.ToBitmap();

            public static readonly Image Hand = SystemIcons.Hand.ToBitmap();

            public static readonly Image Information = SystemIcons.Information.ToBitmap();

            public static readonly Image Question = SystemIcons.Question.ToBitmap();

            public static readonly Image Shield = SystemIcons.Shield.ToBitmap();

            public static readonly Image Warning = SystemIcons.Warning.ToBitmap();

            public static readonly Image WinLogo = SystemIcons.WinLogo.ToBitmap();
        }

        public static class Line
        {
            public static readonly Image ResetGear = LineImages.reset;

            public static readonly Image Optimization = LineImages.optimization;

            public static readonly Image Program = LineImages.program;

            public static readonly Image Settings = LineImages.settings;

            public static readonly Image Tech = LineImages.tech;
        }

        public static class Lineal
        {
            public static readonly Image Gear = LinealImages.gear;

            public static readonly Image Plus = LinealImages.plus;

            public static readonly Image Minus = LinealImages.minus;

            public static readonly Image Download = LinealImages.download;

            public static readonly Image Reload = LinealImages.reload;

            public static readonly Image Refresh = LinealImages.refresh;

            public static readonly Image Reuse = LinealImages.reuse;

            public static readonly Image File = LinealImages.file;

            public static readonly Image NotFile = LinealImages.not_file;

            public static readonly Image Folder = LinealImages.folder;

            public static readonly Image NotFolder = LinealImages.not_folder;
            
            public static readonly Image WWW = LinealImages.www;
            
            public static readonly Image Wifi = LinealImages.wifi;
        }

        public static class Fill
        {
            public static readonly Image ResetGear = FillImages.reset;

            public static readonly Image Program = FillImages.program;

            public static readonly Image File = FillImages.file;

            public static readonly Image NotFile = FillImages.not_file;

            public static readonly Image Folder = FillImages.folder;

            public static readonly Image NotFolder = FillImages.not_folder;
        }

        public static class Flat
        {
            public static readonly Image Program = FlatImages.program;

            public static readonly Image File = FlatImages.file;

            public static readonly Image NotFile = FlatImages.not_file;

            public static readonly Image Folder = FlatImages.folder;

            public static readonly Image NotFolder = FlatImages.not_folder;
        }

        public static class Gradient
        {
            public static readonly Image Program = GradientImages.program;

            public static readonly Image File = GradientImages.file;

            public static readonly Image NotFile = GradientImages.not_file;

            public static readonly Image Folder = GradientImages.folder;

            public static readonly Image NotFolder = GradientImages.not_folder;
            
            public static readonly Image Wifi = GradientImages.wifi;
        }

        /*
        public static class LinearColor
        {
            public static readonly Image File = LinearColorImages.file;

            public static readonly Image NotFile = LinearColorImages.not_file;

            public static readonly Image Folder = LinearColorImages.folder;

            public static readonly Image NotFolder = LinearColorImages.not_folder;
        }
        */

        public static class Blue
        {
            public static readonly Image Question = BlueImages.question;
        }
        
        public static class Circular
        {
            public static readonly Image Wifi = CircularImages.wifi;
        }
        
        public static class Others
        {
            public static readonly Image Monitor = OtherImages.monitor;

            public static readonly Image Proxy = OtherImages.proxy;

            public static readonly Image XButton = OtherImages.xbutton;
        }
    }
}