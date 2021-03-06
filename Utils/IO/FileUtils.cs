// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Common_Library.Utils.IO
{
    public static class FileUtils
    {
        public static FileStream WaitForFile(String fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            for (Int32 numTries = 0; numTries < 10; numTries++)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fullPath, mode, access, share);
                    return fs;
                }
                catch (IOException)
                {
                    fs?.Dispose();
                    Thread.Sleep(250);
                }
            }

            return null;
        }

        public static Boolean CheckPermissions(String path, FileSystemRights access, Boolean? error = false)
        {
            return CheckPermissions(new FileInfo(path), access, error);
        }
        
        public static Boolean CheckPermissions(this FileInfo info, FileSystemRights access, Boolean? error = false)
        {
            try
            {
                if (info.Exists)
                {
                    return info
                        .GetAccessControl()
                        .GetAccessRules(true, true, typeof(NTAccount))
                        .Cast<FileSystemAccessRule>()
                        .Any(rule => (rule.FileSystemRights & access) > 0);
                }
                
                return info.Directory.CheckPermissions(access, error);
            }           
            catch (Exception)
            {
                if (error == null)
                {
                    throw;
                }
                
                return error.Value;
            }
        }
        
        public static Byte[] ReadFileBytes(String path, Boolean isThrow = false)
        {
            try
            {
                return PathUtils.IsExistAsFile(path) ? File.ReadAllBytes(path) : null;
            }
            catch (Exception)
            {
                if (isThrow)
                {
                    throw;
                }

                return null;
            }
        }

        public static String ReadFileText(String path, Boolean isThrow = false)
        {
            try
            {
                return PathUtils.IsExistAsFile(path) ? File.ReadAllText(path) : null;
            }
            catch (Exception)
            {
                if (isThrow)
                {
                    throw;
                }

                return null;
            }
        }

        public static String[] ReadFileLines(String path, Boolean isThrow = false)
        {
            try
            {
                return PathUtils.IsExistAsFile(path) ? File.ReadAllLines(path) : null;
            }
            catch (Exception)
            {
                if (isThrow)
                {
                    throw;
                }

                return null;
            }
        }

        public static String GetFileContents(String file, Int32 timeOut = 5000)
        {
            StreamReader reader = null;
            Int32 startTime = Environment.TickCount;
            try
            {
                if (!PathUtils.IsExistAsFile(file))
                {
                    return String.Empty;
                }

                Boolean opened = false;
                while (!opened)
                {
                    if (Environment.TickCount - startTime >= timeOut)
                    {
                        throw new TimeoutException("File opening timed out");
                    }

                    reader = File.OpenText(file);
                    opened = true;
                }

                String contents = reader.ReadToEnd();
                reader.Close();
                return contents;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }
    }
}