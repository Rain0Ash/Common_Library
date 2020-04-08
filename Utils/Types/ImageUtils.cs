// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Common_Library.Crypto;
using Common_Library.Utils.Types;

namespace Common_Library.Utils
{
    public static class ImageUtils
    {
        public static Icon IconFromImage(Image img)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            // Header
            bw.Write((Int16) 0); // 0 : reserved
            bw.Write((Int16) 1); // 2 : 1=ico, 2=cur
            bw.Write((Int16) 1); // 4 : number of images
            // Image directory
            Int32 w = img.Width;
            if (w >= 256)
            {
                w = 0;
            }

            bw.Write((Byte) w); // 0 : width of image
            Int32 h = img.Height;
            if (h > 255)
            {
                h = 0;
            }

            bw.Write((Byte) h); // 1 : height of image
            bw.Write((Byte) 0); // 2 : number of colors in palette
            bw.Write((Byte) 0); // 3 : reserved
            bw.Write((Int16) 0); // 4 : number of color planes
            bw.Write((Int16) 0); // 6 : bits per pixel
            Int64 sizeHere = ms.Position;
            bw.Write(0); // 8 : image size
            Int32 start = (Int32) ms.Position + 4;
            bw.Write(start); // 12: offset of image data
            // Image data
            img.Save(ms, ImageFormat.Png);
            Int32 imageSize = (Int32) ms.Position - start;
            ms.Seek(sizeHere, SeekOrigin.Begin);
            bw.Write(imageSize);
            ms.Seek(0, SeekOrigin.Begin);

            // And load it
            return new Icon(ms);
        }

        public static Byte[] ToBytes(this Image img)
        {
            using MemoryStream stream = new MemoryStream();
            img.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }

        public static Byte[] GetHash(this Image img)
        {
            return img.ToBytes().GetHash();
        }
        
        public static Byte[] GetHash(this Image img, HashType type)
        {
            return Cryptography.Hash.Hashing(img.ToBytes(), type);
        }
        
        public static Image BytesToImage(Byte[] img)
        {
            using MemoryStream stream = new MemoryStream(img);
            return Image.FromStream(stream);
        }
    }
}