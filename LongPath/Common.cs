﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

// ReSharper disable MemberCanBePrivate.Global

namespace Common_Library.LongPath
{
    public static class Common
    {
        public static Boolean IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        private const UInt32 ProtectedDiscretionaryAcl = 0x80000000;
        private const UInt32 ProtectedSystemAcl = 0x40000000;
        private const UInt32 UnprotectedDiscretionaryAcl = 0x20000000;
        private const UInt32 UnprotectedSystemAcl = 0x10000000;

        internal static void SetAttributes(String path, FileAttributes fileAttributes)
        {
            String normalizedPath = Path.NormalizeLongPath(path);
            if (!NativeMethods.SetFileAttributes(normalizedPath, fileAttributes))
            {
                throw GetExceptionFromLastWin32Error();
            }
        }

        internal static FileAttributes GetAttributes(String path)
        {
            String normalizedPath = Path.NormalizeLongPath(path);

            Int32 errorCode = TryGetDirectoryAttributes(normalizedPath, out FileAttributes fileAttributes);
            if (errorCode != NativeMethods.ErrorSuccess)
            {
                throw GetExceptionFromWin32Error(errorCode);
            }

            return fileAttributes;
        }

        internal static FileAttributes GetAttributes(String path, out Int32 errorCode)
        {
            String normalizedPath = Path.NormalizeLongPath(path);

            errorCode = TryGetDirectoryAttributes(normalizedPath, out FileAttributes fileAttributes);

            return fileAttributes;
        }

        internal static FileAttributes GetFileAttributes(String path)
        {
            String normalizedPath = Path.NormalizeLongPath(path);

            Int32 errorCode = TryGetFileAttributes(normalizedPath, out FileAttributes fileAttributes);
            if (errorCode != NativeMethods.ErrorSuccess)
            {
                throw GetExceptionFromWin32Error(errorCode);
            }

            return fileAttributes;
        }

        internal static String NormalizeSearchPattern(String searchPattern)
        {
            return String.IsNullOrEmpty(searchPattern) || searchPattern == "." ? "*" : searchPattern;
        }

        internal static Boolean Exists(String path, out Boolean isDirectory)
        {
            if (Path.TryNormalizeLongPath(path, out String normalizedPath) || IsPathUnc(path))
            {
                Int32 errorCode = TryGetFileAttributes(normalizedPath, out FileAttributes attributes);
                if (errorCode == 0 && (Int32)attributes != NativeMethods.InvalidFileAttributes)
                {
                    isDirectory = Directory.IsDirectory(attributes);
                    return true;
                }
            }

            isDirectory = false;
            return false;
        }

        internal static Int32 TryGetDirectoryAttributes(String normalizedPath, out FileAttributes attributes)
        {
            Int32 errorCode = TryGetFileAttributes(normalizedPath, out attributes);
            return errorCode;
        }

        internal static Int32 TryGetFileAttributes(String normalizedPath, out FileAttributes attributes)
        {
            NativeMethods.Win32FileAttributeData data = new NativeMethods.Win32FileAttributeData();

            Int32 errorMode = NativeMethods.SetErrorMode(1);
            Boolean success;
            try
            {
                success = NativeMethods.GetFileAttributesEx(normalizedPath, 0, ref data);

            }
            finally
            {
                NativeMethods.SetErrorMode(errorMode);
            }

            if (!success)
            {
                attributes = (FileAttributes)NativeMethods.InvalidFileAttributes;
                return Marshal.GetLastWin32Error();
            }

            attributes = data.FileAttributes;
            return 0;
        }

        internal static Exception GetExceptionFromLastWin32Error(String parameterName = "path")
        {
            return GetExceptionFromWin32Error(Marshal.GetLastWin32Error(), parameterName);
        }

        internal static Exception GetExceptionFromWin32Error(Int32 errorCode, String parameterName = "path")
        {
            String message = GetMessageFromErrorCode(errorCode);

            return errorCode switch
            {
                NativeMethods.ErrorFileNotFound => (Exception) new FileNotFoundException(message),
                NativeMethods.ErrorPathNotFound => new DirectoryNotFoundException(message),
                NativeMethods.ErrorAccessDenied => new UnauthorizedAccessException(message),
                NativeMethods.ErrorFilenameExcedRange => new PathTooLongException(message),
                NativeMethods.ErrorInvalidDrive => new DriveNotFoundException(message),
                NativeMethods.ErrorOperationAborted => new OperationCanceledException(message),
                NativeMethods.ErrorInvalidName => new ArgumentException(message, parameterName),
                _ => new IOException(message, NativeMethods.MakeHrFromErrorCode(errorCode))
            };
        }

        private static String GetMessageFromErrorCode(Int32 errorCode)
        {
            StringBuilder buffer = new StringBuilder(512);

            NativeMethods.FormatMessage(NativeMethods.FormatMessageIgnoreInserts | NativeMethods.FormatMessageFromSystem | NativeMethods.FormatMessageArgumentArray, IntPtr.Zero, errorCode, 0, buffer, buffer.Capacity, IntPtr.Zero);
			
            return buffer.ToString();
        }

        internal static void ThrowIOError(Int32 errorCode, String maybeFullPath)
        {
            // This doesn't have to be perfect, but is a perf optimization.
            Boolean isInvalidPath = errorCode == NativeMethods.ErrorInvalidName || errorCode == NativeMethods.ErrorBadPathname;
            String str = isInvalidPath ? Path.GetFileName(maybeFullPath) : maybeFullPath;

            switch (errorCode)
            {
                case NativeMethods.ErrorFileNotFound:
                    if (String.IsNullOrEmpty(str))
                    {
                        throw new FileNotFoundException("Empty filename");
                    }
                    else
                    {
                        throw new FileNotFoundException($"File {str} not found", str);
                    }

                case NativeMethods.ErrorPathNotFound:
                    if (String.IsNullOrEmpty(str))
                    {
                        throw new DirectoryNotFoundException("Empty directory");
                    }
                    else
                    {
                        throw new DirectoryNotFoundException($"Directory {str} not found");
                    }

                case NativeMethods.ErrorAccessDenied:
                    if (String.IsNullOrEmpty(str))
                    {
                        throw new UnauthorizedAccessException("Empty path");
                    }
                    else
                    {
                        throw new UnauthorizedAccessException($"Access denied accessing {str}");
                    }

                case NativeMethods.ErrorAlreadyExists:
                    if (String.IsNullOrEmpty(str))
                    {
                        goto default;
                    }

                    throw new IOException($"File {str}", NativeMethods.MakeHrFromErrorCode(errorCode));

                case NativeMethods.ErrorFilenameExcedRange:
                    throw new PathTooLongException("Path too long");

                case NativeMethods.ErrorInvalidDrive:
                    throw new DriveNotFoundException($"Drive {str} not found");

                case NativeMethods.ErrorInvalidParameter:
                    throw new IOException(NativeMethods.GetMessage(errorCode), NativeMethods.MakeHrFromErrorCode(errorCode));

                case NativeMethods.ErrorSharingViolation:
                    if (String.IsNullOrEmpty(str))
                    {
                        throw new IOException("Sharing violation with empty filename", NativeMethods.MakeHrFromErrorCode(errorCode));
                    }

                    throw new IOException($"Sharing violation: {str}", NativeMethods.MakeHrFromErrorCode(errorCode));
                  
                case NativeMethods.ErrorFileExists:
                    if (String.IsNullOrEmpty(str))
                    {
                        goto default;
                    }

                    throw new IOException($"File exists {str}", NativeMethods.MakeHrFromErrorCode(errorCode));

                case NativeMethods.ErrorOperationAborted:
                    throw new OperationCanceledException();

                default:
                    throw new IOException(NativeMethods.GetMessage(errorCode), NativeMethods.MakeHrFromErrorCode(errorCode));
            }
        }

        internal static void ThrowIfError(Int32 errorCode, IntPtr byteArray)
        {
            if (errorCode == NativeMethods.ErrorSuccess)
            {
                if (IntPtr.Zero.Equals(byteArray))
                {
                    //
                    // This means that the object doesn't have a security descriptor. And thus we throw
                    // a specific exception for the caller to catch and handle properly.
                    //
                    throw new InvalidOperationException("Object does not have security descriptor,");
                }
            }
            else
            {
                throw errorCode switch
                {
                    NativeMethods.ErrorNotAllAssigned => (Exception) new PrivilegeNotHeldException(
                        "SeSecurityPrivilege"),
                    NativeMethods.ErrorPrivilegeNotHeld => new PrivilegeNotHeldException("SeSecurityPrivilege"),
                    NativeMethods.ErrorAccessDenied => new UnauthorizedAccessException(),
                    NativeMethods.ErrorCantOpenAnonymous => new UnauthorizedAccessException(),
                    NativeMethods.ErrorLogonFailure => new UnauthorizedAccessException(),
                    NativeMethods.ErrorNotEnoughMemory => new OutOfMemoryException(),
                    _ => new IOException(NativeMethods.GetMessage(errorCode),
                        NativeMethods.MakeHrFromErrorCode(errorCode))
                };
            }
        }

        internal static SecurityInfos ToSecurityInfos(AccessControlSections accessControlSections)
        {
            SecurityInfos securityInfos = 0;

            if ((accessControlSections & AccessControlSections.Owner) != 0)
            {
                securityInfos |= SecurityInfos.Owner;
            }

            if ((accessControlSections & AccessControlSections.Group) != 0)
            {
                securityInfos |= SecurityInfos.Group;
            }

            if ((accessControlSections & AccessControlSections.Access) != 0)
            {
                securityInfos |= SecurityInfos.DiscretionaryAcl;
            }

            if ((accessControlSections & AccessControlSections.Audit) != 0)
            {
                securityInfos |= SecurityInfos.SystemAcl;
                //privilege = new Privilege(Privilege.Security);
            }

            return securityInfos;
        }

        internal static void SetAccessControlExtracted(FileSystemSecurity security, String name)
        {
            //security.WriteLock();
            AccessControlSections includeSections = AccessControlSections.Owner | AccessControlSections.Group;
            if(security.GetAccessRules(true, false, typeof(SecurityIdentifier)).Count > 0)
            {
                includeSections |= AccessControlSections.Access;
            }
            if (security.GetAuditRules(true, false, typeof(SecurityIdentifier)).Count > 0)
            {
                includeSections |= AccessControlSections.Audit;
            }
			
            SecurityInfos securityInfo = 0;
            SecurityIdentifier owner = null;
            SecurityIdentifier group = null;
            SystemAcl sacl = null;
            DiscretionaryAcl dacl = null;
            if ((includeSections & AccessControlSections.Owner) != AccessControlSections.None)
            {
                owner = (SecurityIdentifier)security.GetOwner(typeof(SecurityIdentifier));
                if (owner != null)
                {
                    securityInfo |= SecurityInfos.Owner;
                }
            }

            if ((includeSections & AccessControlSections.Group) != AccessControlSections.None)
            {
                group = (SecurityIdentifier)security.GetGroup(typeof(SecurityIdentifier));
                if (group != null)
                {
                    securityInfo |= SecurityInfos.Group;
                }
            }
            Byte[] securityDescriptorBinaryForm = security.GetSecurityDescriptorBinaryForm();
            RawSecurityDescriptor rawSecurityDescriptor = new RawSecurityDescriptor(securityDescriptorBinaryForm, 0);
            Boolean isDiscretionaryAclPresent = (rawSecurityDescriptor.ControlFlags & ControlFlags.DiscretionaryAclPresent) != ControlFlags.None;

            if ((includeSections & AccessControlSections.Audit) != AccessControlSections.None)
            {
                securityInfo |= SecurityInfos.SystemAcl;
                {
                    Boolean isSystemAclPresent = (rawSecurityDescriptor.ControlFlags & ControlFlags.SystemAclPresent) != ControlFlags.None;
                    if (isSystemAclPresent && rawSecurityDescriptor.SystemAcl != null && rawSecurityDescriptor.SystemAcl.Count > 0)
                    {
                        // are all system acls on a file not a container?
                        const Boolean notAContainer = false;
                        const Boolean notADirectoryObjectACL = false;

                        sacl = new SystemAcl(notAContainer, notADirectoryObjectACL,
                            rawSecurityDescriptor.SystemAcl);
                    }
                    securityInfo = (SecurityInfos)((rawSecurityDescriptor.ControlFlags & ControlFlags.SystemAclProtected) == ControlFlags.None ?
                        (UInt32)securityInfo | UnprotectedSystemAcl : (UInt32)securityInfo | ProtectedSystemAcl);
                }
            }
            if ((includeSections & AccessControlSections.Access) != AccessControlSections.None && isDiscretionaryAclPresent)
            {
                securityInfo |= SecurityInfos.DiscretionaryAcl;
                {
                    dacl = new DiscretionaryAcl(false, false, rawSecurityDescriptor.DiscretionaryAcl);
                }
                securityInfo = (SecurityInfos)((rawSecurityDescriptor.ControlFlags & ControlFlags.DiscretionaryAclProtected) == ControlFlags.None ?
                    (UInt32)securityInfo | UnprotectedDiscretionaryAcl : (UInt32)securityInfo | ProtectedDiscretionaryAcl);
            }
            if (securityInfo == 0)
            {
                return;
            }

            Int32 errorNum = SetSecurityInfo(ResourceType.FileObject, name, null, securityInfo, owner, group, sacl, dacl);
            if (errorNum == 0)
            {
                return;
            }

            Exception exception = GetExceptionFromWin32Error(errorNum, name);
            if (exception != null)
            {
                throw exception;
            }

            switch (errorNum)
            {
                case NativeMethods.ErrorAccessDenied:
                    exception = new UnauthorizedAccessException();
                    break;
                case NativeMethods.ErrorInvalidOwner:
                    exception = new InvalidOperationException("Invalid owner");
                    break;
                case NativeMethods.ErrorInvalidPrimaryGroup:
                    exception = new InvalidOperationException("Invalid group");
                    break;
                case NativeMethods.ErrorInvalidName:
                    exception = new ArgumentException(@"Invalid name", nameof(name));
                    break;
                case NativeMethods.ErrorInvalidHandle:
                    exception = new NotSupportedException("Invalid Handle");
                    break;
                case NativeMethods.ErrorFileNotFound:
                    exception = new FileNotFoundException();
                    break;
                default:
                {
                    if (errorNum != NativeMethods.ErrorNoSecurityOnObject)
                    {
                        exception = new InvalidOperationException("Unexpected error");
                    }
                    else
                    {
                        exception = new NotSupportedException("No associated security");
                    }

                    break;
                }
            }
            throw exception;
        }

        internal static Int32 SetSecurityInfo(
            ResourceType type,
            String name,
            SafeHandle handle,
            SecurityInfos securityInformation,
            SecurityIdentifier owner,
            SecurityIdentifier group,
            GenericAcl sacl,
            GenericAcl dacl)
        {
            Int32 errorCode;
            Int32 length;
            Byte[] ownerBinary = null, groupBinary = null, saclBinary = null, daclBinary = null;
            Privilege securityPrivilege = null;

            //
            // Demand unmanaged code permission
            // The integrator layer is free to assert this permission
            // and, in turn, demand another permission of its caller
            //

            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();

            if (owner != null)
            {
                length = owner.BinaryLength;
                ownerBinary = new Byte[length];
                owner.GetBinaryForm(ownerBinary, 0);
            }

            if (group != null)
            {
                length = group.BinaryLength;
                groupBinary = new Byte[length];
                group.GetBinaryForm(groupBinary, 0);
            }

            if (dacl != null)
            {
                length = dacl.BinaryLength;
                daclBinary = new Byte[length];
                dacl.GetBinaryForm(daclBinary, 0);
            }

            if (sacl != null)
            {
                length = sacl.BinaryLength;
                saclBinary = new Byte[length];
                sacl.GetBinaryForm(saclBinary, 0);
            }

            if ((securityInformation & SecurityInfos.SystemAcl) != 0)
            {
                //
                // Enable security privilege if trying to set a SACL.
                // Note: even setting it by handle needs this privilege enabled!
                //

                securityPrivilege = new Privilege(Privilege.Security);
            }

            // Ensure that the finally block will execute
            RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                if (securityPrivilege != null)
                {
                    try
                    {
                        securityPrivilege.Enable();
                    }
                    catch (PrivilegeNotHeldException)
                    {
                        // we will ignore this exception and press on just in case this is a remote resource
                    }
                }

                if (name != null)
                {
                    errorCode = (Int32)NativeMethods.SetSecurityInfoByName(name, (UInt32)type, (UInt32)securityInformation, ownerBinary, groupBinary, daclBinary, saclBinary);
                }
                else if (handle != null)
                {
                    if (handle.IsInvalid)
                    {
                        throw new ArgumentException("Invalid safe handle");
                    }

                    errorCode = (Int32)NativeMethods.SetSecurityInfoByHandle(handle, (UInt32)type, (UInt32)securityInformation, ownerBinary, groupBinary, daclBinary, saclBinary);
                }
                else
                {
                    // both are null, shouldn't happen
                    throw new InvalidProgramException();
                }

                switch (errorCode)
                {
                    case NativeMethods.ErrorNotAllAssigned:
                    case NativeMethods.ErrorPrivilegeNotHeld:
                        throw new PrivilegeNotHeldException(Privilege.Security);
                    case NativeMethods.ErrorAccessDenied:
                    case NativeMethods.ErrorCantOpenAnonymous:
                        throw new UnauthorizedAccessException();
                    default:
                    {
                        if (errorCode != NativeMethods.ErrorSuccess)
                        {
                            goto Error;
                        }

                        break;
                    }
                }
            }
            catch (Exception)
            {
                // protection against exception filter-based luring attacks
                securityPrivilege?.Revert();
                throw;
            }
            finally
            {
                securityPrivilege?.Revert();
            }

            return 0;

            Error:

            if (errorCode == NativeMethods.ErrorNotEnoughMemory)
            {
                throw new OutOfMemoryException();
            }

            return errorCode;
        }

        public static Boolean IsPathUnc(String path)
        {
            return !String.IsNullOrEmpty(path) && path.StartsWith(Path.UNCLongPathPrefix, StringComparison.InvariantCultureIgnoreCase) || Uri.TryCreate(path, UriKind.Absolute, out Uri uri) && uri.IsUnc;
        }

        public static Boolean IsPathDots(String path)
        {
            return path == "." || path == "..";
        }
    }
}