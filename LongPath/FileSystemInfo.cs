// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable UnusedMemberInSuper.Global

namespace Common_Library.LongPath
{
	using FileAttributes = System.IO.FileAttributes;
	using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;
	using DirectoryNotFoundException = System.IO.DirectoryNotFoundException;

	public abstract class FileSystemInfo
	{
		protected String OriginalPath;
		protected String FullPath;
		protected State StateType;
		protected readonly FileAttributeData Data = new FileAttributeData();
		protected Int32 ErrorCode;

	    public abstract System.IO.FileSystemInfo SystemInfo { get; }

        // Summary:
        //     Gets or sets the attributes for the current file or directory.
        //
        // Returns:
        //     System.IO.FileAttributes of the current System.IO.FileSystemInfo.
        //
        // Exceptions:
        //   System.IO.FileNotFoundException:
        //     The specified file does not exist.
        //
        //   System.IO.DirectoryNotFoundException:
        //     The specified path is invalid; for example, it is on an unmapped drive.
        //
        //   System.Security.SecurityException:
        //     The caller does not have the required permission.
        //
        //   System.ArgumentException:
        //     The caller attempts to set an invalid file attribute. -or-The user attempts
        //     to set an attribute value but does not have write permission.
        //
        //   System.IO.IOException:
        //     System.IO.FileSystemInfo.Refresh() cannot initialize the data.
        public FileAttributes Attributes
		{
			get
			{
				return Common.IsRunningOnMono() ? SystemInfo.Attributes : Common.GetAttributes(FullPath);
			}
			set
			{
			    if (Common.IsRunningOnMono())
                {
                    SystemInfo.Attributes = value;
                }
                else
                {
                    Common.SetAttributes(FullPath, value);
                }
            }
		}

		//
		// Summary:
		//     Gets or sets the creation time of the current file or directory.
		//
		// Returns:
		//     The creation date and time of the current System.IO.FileSystemInfo object.
		//
		// Exceptions:
		//   System.IO.IOException:
		//     System.IO.FileSystemInfo.Refresh() cannot initialize the data.
		//
		//   System.IO.DirectoryNotFoundException:
		//     The specified path is invalid; for example, it is on an unmapped drive.
		//
		//   System.PlatformNotSupportedException:
		//     The current operating system is not Windows NT or later.
		//
		//   System.ArgumentOutOfRangeException:
		//     The caller attempts to set an invalid creation time.
		public DateTime CreationTime
		{
			get
			{
				return Common.IsRunningOnMono() ? SystemInfo.CreationTime : CreationTimeUtc.ToLocalTime();
			}
			set
			{
			    if (Common.IsRunningOnMono())
                {
                    SystemInfo.CreationTime = value;
                }
                else
                {
                    CreationTimeUtc = value.ToUniversalTime();
                }
            }
		}

		//
		// Summary:
		//     Gets or sets the creation time, in coordinated universal time (UTC), of the
		//     current file or directory.
		//
		// Returns:
		//     The creation date and time in UTC format of the current System.IO.FileSystemInfo
		//     object.
		//
		// Exceptions:
		//   System.IO.IOException:
		//     System.IO.FileSystemInfo.Refresh() cannot initialize the data.
		//
		//   System.IO.DirectoryNotFoundException:
		//     The specified path is invalid; for example, it is on an unmapped drive.
		//
		//   System.PlatformNotSupportedException:
		//     The current operating system is not Windows NT or later.
		//
		//   System.ArgumentOutOfRangeException:
		//     The caller attempts to set an invalid access time.
		public DateTime CreationTimeUtc
		{
			get
			{
			    if (Common.IsRunningOnMono())
                {
                    return SystemInfo.CreationTimeUtc;
                }

                if (StateType == State.Uninitialized)
				{
					Refresh();
				}
				if (StateType == State.Error)
                {
                    Common.ThrowIOError(ErrorCode, FullPath);
                }

                Int64 fileTime = ((Int64)Data.FtCreationTime.dwHighDateTime << 32) | (Data.FtCreationTime.dwLowDateTime & 0xffffffff);
				return DateTime.FromFileTimeUtc(fileTime);
			}
			set
			{
			    if (Common.IsRunningOnMono())
			    {
			        SystemInfo.CreationTimeUtc = value;
			        return;
			    }

                if (this is DirectoryInfo)
                {
                    Directory.SetCreationTimeUtc(FullPath, value);
                }
                else
                {
                    File.SetCreationTimeUtc(FullPath, value);
                }

                StateType = State.Uninitialized;
			}
		}

		public DateTime LastWriteTime
		{
			get
			{
				return Common.IsRunningOnMono() ? SystemInfo.LastWriteTime : LastWriteTimeUtc.ToLocalTime();
			}
			set
			{
			    if (Common.IsRunningOnMono())
                {
                    SystemInfo.LastWriteTime = value;
                }
                else
                {
                    LastWriteTimeUtc = value.ToUniversalTime();
                }
            }
		}

		private static void ThrowLastWriteTimeUtcIOError(Int32 errorCode, String maybeFullPath)
		{
			// This doesn't have to be perfect, but is a perf optimization.
			Boolean isInvalidPath = errorCode == NativeMethods.ErrorInvalidName || errorCode == NativeMethods.ErrorBadPathname;
			String str = isInvalidPath ? Path.GetFileName(maybeFullPath) : maybeFullPath;

			switch (errorCode)
			{
				case NativeMethods.ErrorFileNotFound:
					break;

				case NativeMethods.ErrorPathNotFound:
					break;

				case NativeMethods.ErrorAccessDenied:
					if (String.IsNullOrEmpty(str))
                    {
                        throw new UnauthorizedAccessException("Empty path");
                    }

                    throw new UnauthorizedAccessException($"Access denied accessing {str}");

                case NativeMethods.ErrorAlreadyExists:
					if (String.IsNullOrEmpty(str))
                    {
                        goto default;
                    }

                    throw new System.IO.IOException($"File {str}", NativeMethods.MakeHrFromErrorCode(errorCode));

				case NativeMethods.ErrorFilenameExcedRange:
					throw new System.IO.PathTooLongException("Path too long");

				case NativeMethods.ErrorInvalidDrive:
					throw new System.IO.DriveNotFoundException($"Drive {str} not found");

				case NativeMethods.ErrorInvalidParameter:
					throw new System.IO.IOException(NativeMethods.GetMessage(errorCode), NativeMethods.MakeHrFromErrorCode(errorCode));

				case NativeMethods.ErrorSharingViolation:
					if (String.IsNullOrEmpty(str))
                    {
                        throw new System.IO.IOException("Sharing violation with empty filename", NativeMethods.MakeHrFromErrorCode(errorCode));
                    }
                    else
                    {
                        throw new System.IO.IOException($"Sharing violation: {str}", NativeMethods.MakeHrFromErrorCode(errorCode));
                    }

                case NativeMethods.ErrorFileExists:
					if (String.IsNullOrEmpty(str))
                    {
                        goto default;
                    }

                    throw new System.IO.IOException($"File exists {str}", NativeMethods.MakeHrFromErrorCode(errorCode));

				case NativeMethods.ErrorOperationAborted:
					throw new OperationCanceledException();

				default:
					throw new System.IO.IOException(NativeMethods.GetMessage(errorCode), NativeMethods.MakeHrFromErrorCode(errorCode));
			}
		}
		public DateTime LastWriteTimeUtc
		{
			get
			{
			    if (Common.IsRunningOnMono())
                {
                    return SystemInfo.LastWriteTimeUtc;
                }

                if (StateType == State.Uninitialized)
				{
					Refresh();
				}
				if (StateType == State.Error)
                {
                    ThrowLastWriteTimeUtcIOError(ErrorCode, FullPath);
                }

                Int64 fileTime = ((Int64)Data.FtLastWriteTime.dwHighDateTime << 32) | (Data.FtLastWriteTime.dwLowDateTime & 0xffffffff);
				return DateTime.FromFileTimeUtc(fileTime);
			}
			set
			{
			    if (Common.IsRunningOnMono())
			    {
			        SystemInfo.LastWriteTimeUtc = value;
			        return;
			    }


                if (this is DirectoryInfo)
                {
                    Directory.SetLastWriteTimeUtc(FullPath, value);
                }
                else
                {
                    File.SetLastWriteTimeUtc(FullPath, value);
                }

                StateType = State.Uninitialized;
			}
		}

		public DateTime LastAccessTime
		{
			get
			{
				return Common.IsRunningOnMono() ? SystemInfo.LastAccessTime : LastAccessTimeUtc.ToLocalTime();
			}
			set
			{
			    if (Common.IsRunningOnMono())
                {
                    SystemInfo.LastAccessTime = value;
                }
                else
                {
                    LastAccessTimeUtc = value.ToUniversalTime();
                }
            }
		}

		public DateTime LastAccessTimeUtc
		{
			get
			{
			    if (Common.IsRunningOnMono())
                {
                    return SystemInfo.LastAccessTimeUtc;
                }

                if (StateType == State.Uninitialized)
				{
					Refresh();
				}
				if (StateType == State.Error)
                {
                    Common.ThrowIOError(ErrorCode, FullPath);
                }

                Int64 fileTime = ((Int64)Data.FtLastAccessTime.dwHighDateTime << 32) | (Data.FtLastAccessTime.dwLowDateTime & 0xffffffff);
				return DateTime.FromFileTimeUtc(fileTime);
			}
			set
			{
			    if (Common.IsRunningOnMono())
			    {
			        SystemInfo.LastAccessTimeUtc = value;
			        return;
			    }


                if (this is DirectoryInfo)
                {
                    Directory.SetLastAccessTimeUtc(FullPath, value);
                }
                else
                {
                    File.SetLastAccessTimeUtc(FullPath, value);
                }

                StateType = State.Uninitialized;
			}
		}

		public virtual String FullName
		{
			get { return FullPath; }
		}

		public String Extension
		{
			get
			{
				return Path.GetExtension(FullPath);
			}
		}

		public abstract String Name { get; }
		public abstract Boolean Exists { get; }
		internal String DisplayPath { get; set; }

		protected enum State
		{
			Uninitialized, Initialized, Error
		}

		protected class FileAttributeData
		{
			public FileAttributes FileAttributes;
			public FILETIME FtCreationTime;
			public FILETIME FtLastAccessTime;
			public FILETIME FtLastWriteTime;
			public Int32 FileSizeHigh;
			public Int32 FileSizeLow;

			internal void From(NativeMethods.Win32FindData findData)
			{
				FileAttributes = findData.dwFileAttributes;
				FtCreationTime = findData.ftCreationTime;
				FtLastAccessTime = findData.ftLastAccessTime;
				FtLastWriteTime = findData.ftLastWriteTime;
				FileSizeHigh = findData.nFileSizeHigh;
				FileSizeLow = findData.nFileSizeLow;
			}
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			//(new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.FullPath)).Demand();
			info.AddValue("OriginalPath", OriginalPath, typeof(String));
			info.AddValue("FullPath", FullPath, typeof(String));
		}

        internal virtual String GetNormalizedPathWithSearchPattern()
        {
	        // https://docs.microsoft.com/en-us/windows/desktop/api/fileapi/nf-fileapi-findfirstfilew
            // "If the string ends with a wildcard, period (.), or directory name, the user must have access permissions to the root and all subdirectories on the path"
            // This is a problem if the executing principal has no access to the parent folder;
            // appending "\*" fixes this while still allowing retrieval of attributes
            return Path.NormalizeLongPath(this is DirectoryInfo ? Path.Combine(FullPath, "*") : FullPath);
        }


        public void Refresh()
		{
			try
			{
				// TODO: BeginFind fails on "\\?\c:\"
				using SafeFindHandle handle = Directory.BeginFind(GetNormalizedPathWithSearchPattern(), out NativeMethods.Win32FindData findData);
				if (handle == null)
				{
					StateType = State.Error;
					ErrorCode = Marshal.GetLastWin32Error();
				}
				else
				{
					Data.From(findData);
					StateType = State.Initialized;
				}
			}
			catch (DirectoryNotFoundException)
			{
				StateType = State.Error;
				ErrorCode = NativeMethods.ErrorPathNotFound;
			}
			catch (Exception)
			{
				if (StateType != State.Error)
                {
                    Common.ThrowIOError(Marshal.GetLastWin32Error(), FullPath);
                }
            }
		}

		public abstract void Delete();
	}
}