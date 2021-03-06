// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common_Library.Utils.GUI.ListView
{
    public static class ListViewUtils
    {
        public static Boolean Contains(this ImageList images, Image image)
        {
            return Contains(images.Images, image);
        }
        
        public static Boolean Contains(this ImageList.ImageCollection images, Image image)
        {
            return IndexOfImage(images, image) >= 0;
        }
        
        public static String KeyOfImage(this ImageList images, Image image)
        {
            return KeyOfImage(images.Images, image);
        }
        
        public static String KeyOfImage(this ImageList images, Image image, out Int32 index)
        {
            return KeyOfImage(images.Images, image, out index);
        }

        public static String KeyOfImage(this ImageList.ImageCollection images, Image image)
        {
            return KeyOfImage(images, image, out _);
        }
        
        public static String KeyOfImage(this ImageList.ImageCollection images, Image image, out Int32 index)
        {
            foreach (String key in images.Keys)
            {
                if (images[key] != image)
                {
                    continue;
                }

                index = images.IndexOfKey(key);
                return key;
            }

            index = -1;
            return null;
        }

        public static Int32 IndexOfImage(this ImageList images, Image image)
        {
            return IndexOfImage(images.Images, image);
        }
        
        public static Int32 IndexOfImage(this ImageList.ImageCollection images, Image image)
        {
            for (Int32 i = 0; i < images.Count; i++)
            {
                if (images[i] == image)
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static String GetOrSetImageKey(this ImageList images, Image image)
        {
            return GetOrSetImageKey(images.Images, image);
        }
        
        public static String GetOrSetImageKey(this ImageList.ImageCollection images, Image image)
        {
            String key = images.KeyOfImage(image);

            if (!key.IsNullOrEmpty())
            {
                return key;
            }

            key = image.GetHash().Text();
            Int32 index = images.IndexOfImage(image);

            if (index >= 0)
            {
                images.SetKeyName(index, key);
                return key;
            }

            images.Add(key, image);
            return key;
        }
        
        public static Image GetImage(this ListViewItem item)
        {
            Int32 index = item.ImageIndex;
            ImageList.ImageCollection images = item.ImageList.Images;

            return images[item.ImageKey] ?? (index >= 0 && index < images.Count ? images[index] : null);
        }
        
        public static void SetImage(this ListViewItem item, Image image)
        {
            item.ImageKey = GetOrSetImageKey(item.ImageList, image);
        }
    }
}