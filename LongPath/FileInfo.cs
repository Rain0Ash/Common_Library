// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Text;
using FileAccess = System.IO.FileAccess;
using FileMode = System.IO.FileMode;
using FileStream = System.IO.FileStream;
using StreamWriter = System.IO.StreamWriter;
using FileShare = System.IO.FileShare;
using FileOptions = System.IO.FileOptions;
using StreamReader = System.IO.StreamReader;
using FileAttributes = System.IO.FileAttributes;
using System.Security.AccessControl;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Common_Library.LongPath
{
    public class FileInfo : FileSystemInfo
    {
        public DirectoryInfo Directory
        {
            get
            {
                String dirName = DirectoryName;
                return dirName == null ? null : new DirectoryInfo(dirName);
            }
        }

        public System.IO.FileInfo SysFileInfo
        {
            get
            {
                return new System.IO.FileInfo(FullPath);
            }
        }

        public override System.IO.FileSystemInfo SystemInfo
        {
            get
            {
                return SysFileInfo;
            }
        }

        public String DirectoryName
        {
            get
            {
                return Path.GetDirectoryName(FullPath);
            }
        }

        public override Boolean Exists
        {
            get
            {
                if (Common.IsRunningOnMono())
                {
                    return SysFileInfo.Exists;
                }

                if (StateType == State.Uninitialized)
                {
                    Refresh();
                }

                return StateType == State.Initialized &&
                       (Data.FileAttributes & FileAttributes.Directory) != FileAttributes.Directory;
            }
        }

        /// <summary>
        /// <inheritdoc cref="System.IO.FileInfo.Length"/>
        /// </summary>
        public Int64 Length
        {
            get
            {
                return GetFileLength();
            }
        }

        public override String Name { get; }

        public FileInfo(String fileName)
        {
            OriginalPath = fileName;
            FullPath = Path.GetFullPath(fileName);
            Name = Path.GetFileName(fileName);
            DisplayPath = GetDisplayPath(fileName);
        }

        private static String GetDisplayPath(String originalPath)
        {
            return originalPath;
        }

        private Int64 GetFileLength()
        {
            if (Common.IsRunningOnMono())
            {
                return SysFileInfo.Length;
            }

            if (StateType == State.Uninitialized)
            {
                Refresh();
            }

            if (StateType == State.Error)
            {
                Common.ThrowIOError(ErrorCode, FullPath);
            }

            return ((Int64) Data.FileSizeHigh << 32) | (Data.FileSizeLow & 0xFFFFFFFFL);
        }

        public StreamWriter AppendText()
        {
            return File.CreateStreamWriter(FullPath, true);
        }

        public FileInfo CopyTo(String destFileName, Boolean overwrite = false)
        {
            File.Copy(FullPath, destFileName, overwrite);
            return new FileInfo(destFileName);
        }

        public FileStream Create()
        {
            return File.Create(FullPath);
        }

        public StreamWriter CreateText()
        {
            return File.CreateStreamWriter(FullPath, false);
        }

        public override void Delete()
        {
            File.Delete(FullPath);
        }

        public void MoveTo(String destFileName)
        {
            File.Move(FullPath, destFileName);
        }

        public FileStream Open(FileMode mode, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None)
        {
            return Common.IsRunningOnMono()
                ? SysFileInfo.Open(mode, access, share)
                : File.Open(FullPath, mode, access, share, 4096, FileOptions.SequentialScan);
        }

        public FileStream OpenRead()
        {
            return Common.IsRunningOnMono()
                ? SysFileInfo.OpenRead()
                : File.Open(FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.None);
        }

        public StreamReader OpenText()
        {
            return File.CreateStreamReader(FullPath, Encoding.UTF8, true, 1024);
        }

        public FileStream OpenWrite()
        {
            return File.Open(FullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        }

        public override String ToString()
        {
            return DisplayPath;
        }

        public void Encrypt()
        {
            File.Encrypt(FullPath);
        }

        public void Decrypt()
        {
            File.Decrypt(FullPath);
        }

        public Boolean IsReadOnly
        {
            get
            {
                if (Common.IsRunningOnMono())
                {
                    return SysFileInfo.IsReadOnly;
                }

                return (Attributes & FileAttributes.ReadOnly) != 0;
            }
            set
            {
                if (Common.IsRunningOnMono())
                {
                    SysFileInfo.IsReadOnly = value;
                    return;
                }

                if (value)
                {
                    Attributes |= FileAttributes.ReadOnly;
                    return;
                }

                Attributes &= ~FileAttributes.ReadOnly;
            }
        }

        public FileInfo Replace(String destinationFilename, String backupFilename, Boolean ignoreMetadataErrors = false)
        {
            File.Replace(FullPath, destinationFilename, backupFilename, ignoreMetadataErrors);
            return new FileInfo(destinationFilename);
        }

        public FileSecurity GetAccessControl()
        {
            return File.GetAccessControl(FullPath);
        }

        public FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            return File.GetAccessControl(FullPath, includeSections);
        }

        public void SetAccessControl(FileSecurity security)
        {
            File.SetAccessControl(FullPath, security);
        }
    }
}