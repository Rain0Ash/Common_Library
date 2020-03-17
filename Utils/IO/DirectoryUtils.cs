// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using Common_Library.LongPath;

namespace Common_Library.Utils.IO
{
    public static class DirectoryUtils
    {
        public static void CreateDirectory(String path)
        {
            CreateDirectory(path, out _);
        }

        public static void CreateDirectory(String path, out DirectoryInfo directoryInfo)
        {
            if (Directory.Exists(path))
            {
                directoryInfo = new DirectoryInfo(path);
                return;
            }

            directoryInfo = Directory.CreateDirectory(path);
        }

        public static Boolean TryCreateDirectory(String path, PathAction remove = PathAction.Standart)
        {
            return TryCreateDirectory(path, remove, out _);
        }

        public static Boolean TryCreateDirectory(String path, out DirectoryInfo directoryInfo)
        {
            return TryCreateDirectory(path, PathAction.Standart, out directoryInfo);
        }

        public static Boolean TryCreateDirectory(String path, PathAction remove, out DirectoryInfo directoryInfo)
        {
            directoryInfo = null;
            try
            {
                if (Directory.Exists(path))
                {
                    return true;
                }

                CreateDirectory(path, out directoryInfo);

                return Directory.Exists(path);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                try
                {
                    switch (remove)
                    {
                        case PathAction.Standart:
                            if (Directory.GetFiles(path).Length == 0 &&
                                Directory.GetDirectories(path).Length == 0)
                            {
                                Directory.Delete(path, false);
                            }

                            break;
                        case PathAction.Force:
                            Directory.Delete(path, true);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            }
        }
    }
}