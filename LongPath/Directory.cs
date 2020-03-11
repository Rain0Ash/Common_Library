// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Linq;
using System.Security.Principal;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace Common_Library.LongPath
{
    using SafeFileHandle = Microsoft.Win32.SafeHandles.SafeFileHandle;
    using FileMode = System.IO.FileMode;
    using FileShare = System.IO.FileShare;
    using SearchOption = System.IO.SearchOption;

    public static class Directory
    {
        internal static SafeFileHandle GetDirectoryHandle(String normalizedPath)
        {
            SafeFileHandle handle = NativeMethods.CreateFile(normalizedPath,
                NativeMethods.EFileAccess.GenericWrite,
                (UInt32) (FileShare.Write | FileShare.Delete),
                IntPtr.Zero, (Int32) FileMode.Open, NativeMethods.FileFlagBackupSemantics, IntPtr.Zero);
            if (!handle.IsInvalid)
            {
                return handle;
            }

            Exception ex = Common.GetExceptionFromLastWin32Error();
            Console.WriteLine(@"error {0} with {1}{2}", ex.Message, normalizedPath, ex.StackTrace);
            throw ex;
        }

        public static void SetAttributes(String path, FileAttributes fileAttributes)
        {
            Common.SetAttributes(path, fileAttributes);
        }

        public static FileAttributes GetAttributes(String path)
        {
            return Common.GetAttributes(path);
        }

        public static String GetCurrentDirectory()
        {
            return Path.RemoveLongPathPrefix(Path.NormalizeLongPath("."));
        }

        public static void Delete(String path, Boolean recursive)
        {
            if (Common.IsRunningOnMono())
            {
                System.IO.Directory.Delete(path, recursive);
                return;
            }

            /* MSDN: https://msdn.microsoft.com/en-us/library/fxeahc5f.aspx
			   The behavior of this method differs slightly when deleting a directory that contains a reparse point, 
			   such as a symbolic link or a mount point. 
			   (1) If the reparse point is a directory, such as a mount point, it is unmounted and the mount point is deleted. 
			   This method does not recurse through the reparse point. 
			   (2) If the reparse point is a symbolic link to a file, the reparse point is deleted and not the target of 
			   the symbolic link.
			*/

            try
            {
                const FileAttributes reparseFlags = FileAttributes.Directory | FileAttributes.ReparsePoint;
                Boolean isDirectoryReparsePoint = (Common.GetAttributes(path) & reparseFlags) == reparseFlags;

                if (isDirectoryReparsePoint)
                {
                    Delete(path);
                    return;
                }
            }
            catch (FileNotFoundException)
            {
                // ignore: not there when we try to delete, it doesn't matter
            }

            if (recursive == false)
            {
                Delete(path);
                return;
            }

            try
            {
                foreach (String file in EnumerateFileSystemEntries(path, "*", false, true, SearchOption.TopDirectoryOnly))
                {
                    File.Delete(file);
                }
            }
            catch (FileNotFoundException)
            {
                // ignore: not there when we try to delete, it doesn't matter
            }

            try
            {
                foreach (String subPath in EnumerateFileSystemEntries(path, "*", true, false, SearchOption.TopDirectoryOnly))
                {
                    Delete(subPath, true);
                }
            }
            catch (FileNotFoundException)
            {
                // ignore: not there when we try to delete, it doesn't matter
            }

            try
            {
                Delete(path);
            }
            catch (FileNotFoundException)
            {
                // ignore: not there when we try to delete, it doesn't matter
            }
        }

        /// <summary>
        ///     Deletes the specified empty directory.
        /// </summary>
        /// <param name="path">
        ///      A <see cref="string"/> containing the path of the directory to delete.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white
        ///     space, or contains one or more invalid characters as defined in
        ///     <see cref="Common_Library.LongPath.Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must not exceed
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        ///     <paramref name="path"/> could not be found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> refers to a directory that is read-only.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        ///     <paramref name="path"/> is a file.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> refers to a directory that is not empty.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> refers to a directory that is in use.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        public static void Delete(String path)
        {
            if (Common.IsRunningOnMono())
            {
                System.IO.Directory.Delete(path);
            }

            String normalizedPath = Path.NormalizeLongPath(path);
            if (!NativeMethods.RemoveDirectory(normalizedPath))
            {
                throw Common.GetExceptionFromLastWin32Error();
            }
        }

        /// <summary>
        ///     Returns a value indicating whether the specified path refers to an existing directory.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="string"/> containing the path to check.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="path"/> refers to an existing directory;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        ///     Note that this method will return false if any error occurs while trying to determine
        ///     if the specified directory exists. This includes situations that would normally result in
        ///     thrown exceptions including (but not limited to); passing in a directory name with invalid
        ///     or too many characters, an I/O error such as a failing or missing disk, or if the caller
        ///     does not have Windows or Code Access Security (CAS) permissions to to read the directory.
        /// </remarks>
        public static Boolean Exists(String path)
        {
            if (Common.IsRunningOnMono())
            {
                return System.IO.Directory.Exists(path);
            }

            return Common.Exists(path, out Boolean isDirectory) && isDirectory;
        }

        /// <summary>
        ///     Returns a enumerable containing the directory names of the specified directory.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="string"/> containing the path of the directory to search.
        /// </param>
        /// <returns>
        ///     A <see cref="IEnumerable{T}"/> containing the directory names within <paramref name="path"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white
        ///     space, or contains one or more invalid characters as defined in
        ///     <see cref="Common_Library.LongPath.Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must not exceed
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        ///     <paramref name="path"/> contains one or more directories that could not be
        ///     found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        ///     <paramref name="path"/> is a file.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        public static IEnumerable<String> EnumerateDirectories(String path)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.EnumerateDirectories(path)
                : EnumerateFileSystemEntries(path, "*", true, false, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        ///     Returns a enumerable containing the directory names of the specified directory that
        ///     match the specified search pattern.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="String"/> containing the path of the directory to search.
        /// </param>
        /// <param name="searchPattern">
        ///     A <see cref="String"/> containing search pattern to match against the names of the
        ///     directories in <paramref name="path"/>, otherwise, <see langword="null"/> or an empty
        ///     string ("") to use the default search pattern, "*".
        /// </param>
        /// <returns>
        ///     A <see cref="IEnumerable{T}"/> containing the directory names within <paramref name="path"/>
        ///     that match <paramref name="searchPattern"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white
        ///     space, or contains one or more invalid characters as defined in
        ///     <see cref="Common_Library.LongPath.Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must not exceed
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        ///     <paramref name="path"/> contains one or more directories that could not be
        ///     found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        ///     <paramref name="path"/> is a file.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        public static IEnumerable<String> EnumerateDirectories(String path, String searchPattern)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.EnumerateDirectories(path, searchPattern)
                : EnumerateFileSystemEntries(path, searchPattern, true, false, SearchOption.TopDirectoryOnly);
        }

        public static IEnumerable<String> EnumerateDirectories(String path, String searchPattern, SearchOption options)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.EnumerateDirectories(path, searchPattern, options)
                : EnumerateFileSystemEntries(path, searchPattern, true, false, options);
        }

        /// <summary>
        ///     Returns a enumerable containing the file names of the specified directory.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="string"/> containing the path of the directory to search.
        /// </param>
        /// <returns>
        ///     A <see cref="IEnumerable{T}"/> containing the file names within <paramref name="path"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white
        ///     space, or contains one or more invalid characters as defined in
        ///     <see cref="Common_Library.LongPath.Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must not exceed
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        ///     <paramref name="path"/> contains one or more directories that could not be
        ///     found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        ///     <paramref name="path"/> is a file.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        public static IEnumerable<String> EnumerateFiles(String path)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.EnumerateFiles(path)
                : EnumerateFileSystemEntries(path, "*", false, true, SearchOption.TopDirectoryOnly);
        }

        public static IEnumerable<String> EnumerateFiles(String path, String searchPattern, SearchOption options)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.EnumerateFiles(path, searchPattern, options)
                : EnumerateFileSystemEntries(path, searchPattern, false, true, options);
        }

        /// <summary>
        ///     Returns a enumerable containing the file names of the specified directory that
        ///     match the specified search pattern.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="string"/> containing the path of the directory to search.
        /// </param>
        /// <param name="searchPattern">
        ///     A <see cref="string"/> containing search pattern to match against the names of the
        ///     files in <paramref name="path"/>, otherwise, <see langword="null"/> or an empty
        ///     string ("") to use the default search pattern, "*".
        /// </param>
        /// <returns>
        ///     A <see cref="IEnumerable{T}"/> containing the file names within <paramref name="path"/>
        ///     that match <paramref name="searchPattern"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white
        ///     space, or contains one or more invalid characters as defined in
        ///     <see cref="Common_Library.LongPath.Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must not exceed
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        ///     <paramref name="path"/> contains one or more directories that could not be
        ///     found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        ///     <paramref name="path"/> is a file.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        public static IEnumerable<String> EnumerateFiles(String path, String searchPattern)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.EnumerateFiles(path, searchPattern)
                : EnumerateFileSystemEntries(path, searchPattern, false, true, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        ///     Returns a enumerable containing the file and directory names of the specified directory.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="string"/> containing the path of the directory to search.
        /// </param>
        /// <returns>
        ///     A <see cref="IEnumerable{T}"/> containing the file and directory names within
        ///     <paramref name="path"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white
        ///     space, or contains one or more invalid characters as defined in
        ///     <see cref="Common_Library.LongPath.Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must not exceed
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        ///     <paramref name="path"/> contains one or more directories that could not be
        ///     found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        ///     <paramref name="path"/> is a file.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        public static IEnumerable<String> EnumerateFileSystemEntries(String path)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.EnumerateFileSystemEntries(path)
                : EnumerateFileSystemEntries(path, null, true, true, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        ///     Returns a enumerable containing the file and directory names of the specified directory
        ///     that match the specified search pattern.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="string"/> containing the path of the directory to search.
        /// </param>
        /// <param name="searchPattern">
        ///     A <see cref="string"/> containing search pattern to match against the names of the
        ///     files and directories in <paramref name="path"/>, otherwise, <see langword="null"/>
        ///     or an empty string ("") to use the default search pattern, "*".
        /// </param>
        /// <returns>
        ///     A <see cref="IEnumerable{T}"/> containing the file and directory names within
        ///     <paramref name="path"/>that match <paramref name="searchPattern"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white
        ///     space, or contains one or more invalid characters as defined in
        ///     <see cref="Common_Library.LongPath.Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must not exceed
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        ///     <paramref name="path"/> contains one or more directories that could not be
        ///     found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        ///     <paramref name="path"/> is a file.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        public static IEnumerable<String> EnumerateFileSystemEntries(String path, String searchPattern)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.EnumerateFileSystemEntries(path, searchPattern)
                : EnumerateFileSystemEntries(path, searchPattern, true, true, SearchOption.TopDirectoryOnly);
        }

        public static IEnumerable<String> EnumerateFileSystemEntries(String path, String searchPattern, SearchOption options)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.EnumerateFileSystemEntries(path, searchPattern, options)
                : EnumerateFileSystemEntries(path, searchPattern, true, true, options);
        }

        internal static IEnumerable<String> EnumerateFileSystemEntries(String path, String searchPattern, Boolean includeDirectories,
            Boolean includeFiles, SearchOption option)
        {
            String normalizedSearchPattern = Common.NormalizeSearchPattern(searchPattern);
            String normalizedPath = Path.NormalizeLongPath(path);

            return EnumerateNormalizedFileSystemEntries(includeDirectories, includeFiles, option, normalizedPath, normalizedSearchPattern);
        }

        private static IEnumerable<String> EnumerateNormalizedFileSystemEntries(Boolean includeDirectories, Boolean includeFiles,
            SearchOption option, String normalizedPath, String normalizedSearchPattern)
        {
            // First check whether the specified path refers to a directory and exists
            Int32 errorCode = Common.TryGetDirectoryAttributes(normalizedPath, out FileAttributes _);
            if (errorCode != 0)
            {
                throw Common.GetExceptionFromWin32Error(errorCode);
            }

            if (option == SearchOption.AllDirectories)
            {
                return EnumerateFileSystemIteratorRecursive(normalizedPath, normalizedSearchPattern, includeDirectories,
                    includeFiles);
            }

            return EnumerateFileSystemIterator(normalizedPath, normalizedSearchPattern, includeDirectories, includeFiles);
        }

        private static IEnumerable<String> EnumerateFileSystemIterator(String normalizedPath, String normalizedSearchPattern,
            Boolean includeDirectories, Boolean includeFiles)
        {
            // NOTE: Any exceptions thrown from this method are thrown on a call to IEnumerator<string>.MoveNext()

            String path = Common.IsPathUnc(normalizedPath) ? normalizedPath : Path.RemoveLongPathPrefix(normalizedPath);

            using SafeFindHandle handle = BeginFind(Path.Combine(normalizedPath, normalizedSearchPattern),
                out NativeMethods.Win32FindData findData);
            if (handle == null)
            {
                yield break;
            }

            do
            {
                if (IsDirectory(findData.dwFileAttributes))
                {
                    if (IsCurrentOrParentDirectory(findData.cFileName))
                    {
                        continue;
                    }

                    if (includeDirectories)
                    {
                        yield return Path.Combine(Path.RemoveLongPathPrefix(path), findData.cFileName);
                    }
                }
                else
                {
                    if (includeFiles)
                    {
                        yield return Path.Combine(Path.RemoveLongPathPrefix(path), findData.cFileName);
                    }
                }
            } while (NativeMethods.FindNextFile(handle, out findData));

            Int32 errorCode = Marshal.GetLastWin32Error();
            if (errorCode != NativeMethods.ErrorNoMoreFiles)
            {
                throw Common.GetExceptionFromWin32Error(errorCode);
            }
        }

        private static IEnumerable<String> EnumerateFileSystemIteratorRecursive(String normalizedPath, String normalizedSearchPattern,
            Boolean includeDirectories, Boolean includeFiles)
        {
            // NOTE: Any exceptions thrown from this method are thrown on a call to IEnumerator<string>.MoveNext()
            Queue<String> pendingDirectories = new Queue<String>();
            pendingDirectories.Enqueue(normalizedPath);
            while (pendingDirectories.Count > 0)
            {
                normalizedPath = pendingDirectories.Dequeue();
                // get all subdirs to recurse in the next iteration
                foreach (String subdir in EnumerateNormalizedFileSystemEntries(true, false, SearchOption.TopDirectoryOnly, normalizedPath,
                    "*"))
                {
                    pendingDirectories.Enqueue(Path.NormalizeLongPath(subdir));
                }

                String path = Common.IsPathUnc(normalizedPath) ? normalizedPath : Path.RemoveLongPathPrefix(normalizedPath);

                using SafeFindHandle handle = BeginFind(Path.Combine(normalizedPath, normalizedSearchPattern),
                    out NativeMethods.Win32FindData findData);
                if (handle == null)
                {
                    continue;
                }

                do
                {
                    String fullPath = Path.Combine(path, findData.cFileName);
                    if (IsDirectory(findData.dwFileAttributes))
                    {
                        if (IsCurrentOrParentDirectory(findData.cFileName))
                        {
                            continue;
                        }

                        if (includeDirectories)
                        {
                            yield return Path.RemoveLongPathPrefix(fullPath);
                        }
                    }
                    else if (includeFiles)
                    {
                        yield return Path.RemoveLongPathPrefix(fullPath);
                    }
                } while (NativeMethods.FindNextFile(handle, out findData));

                Int32 errorCode = Marshal.GetLastWin32Error();
                if (errorCode != NativeMethods.ErrorNoMoreFiles)
                {
                    throw Common.GetExceptionFromWin32Error(errorCode);
                }
            }
        }

        internal static SafeFindHandle BeginFind(String normalizedPathWithSearchPattern,
            out NativeMethods.Win32FindData findData)
        {
            normalizedPathWithSearchPattern = normalizedPathWithSearchPattern.TrimEnd('\\');
            SafeFindHandle handle = NativeMethods.FindFirstFile(normalizedPathWithSearchPattern, out findData);
            if (!handle.IsInvalid)
            {
                return handle;
            }

            Int32 errorCode = Marshal.GetLastWin32Error();
            if (errorCode != NativeMethods.ErrorFileNotFound &&
                errorCode != NativeMethods.ErrorPathNotFound &&
                errorCode != NativeMethods.ErrorNotReady)
            {
                throw Common.GetExceptionFromWin32Error(errorCode);
            }

            return null;
        }

        internal static Boolean IsDirectory(FileAttributes attributes)
        {
            return (attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        private static Boolean IsCurrentOrParentDirectory(String directoryName)
        {
            return directoryName.Equals(".", StringComparison.OrdinalIgnoreCase) ||
                   directoryName.Equals("..", StringComparison.OrdinalIgnoreCase);
        }

        public static void Move(String sourcePath, String destinationPath)
        {
            if (Common.IsRunningOnMono())
            {
                System.IO.File.Move(sourcePath, destinationPath);
                return;
            }

            String normalizedSourcePath = Path.NormalizeLongPath(sourcePath, "sourcePath");
            String normalizedDestinationPath = Path.NormalizeLongPath(destinationPath, "destinationPath");

            if (NativeMethods.MoveFile(normalizedSourcePath, normalizedDestinationPath))
            {
                return;
            }

            Int32 lastWin32Error = Marshal.GetLastWin32Error();
            if (lastWin32Error == NativeMethods.ErrorAccessDenied)
            {
                throw new IOException($"Access to the path '{sourcePath}'is denied.", NativeMethods.MakeHrFromErrorCode(lastWin32Error));
            }

            throw Common.GetExceptionFromWin32Error(lastWin32Error);
        }

        private static DirectoryInfo CreateDirectoryUnc(String path)
        {
            Int32 length = path.Length;
            if (length >= 2 && Path.IsDirectorySeparator(path[length - 1]))
            {
                --length;
            }

            Int32 rootLength = Path.GetRootLength(path);

            List<String> pathComponents = new List<String>();

            if (length > rootLength)
            {
                for (Int32 index = length - 1; index >= rootLength; --index)
                {
                    String subPath = path.Substring(0, index + 1);
                    if (!Exists(subPath))
                    {
                        pathComponents.Add(subPath);
                    }

                    while (index > rootLength && path[index] != System.IO.Path.DirectorySeparatorChar &&
                           path[index] != System.IO.Path.AltDirectorySeparatorChar)
                    {
                        --index;
                    }
                }
            }

            while (pathComponents.Count > 0)
            {
                String str = Path.NormalizeLongPath(pathComponents[pathComponents.Count - 1]);
                pathComponents.RemoveAt(pathComponents.Count - 1);

                if (NativeMethods.CreateDirectory(str, IntPtr.Zero))
                {
                    continue;
                }

                // To mimic Directory.CreateDirectory, we don't throw if the directory (not a file) already exists
                Int32 errorCode = Marshal.GetLastWin32Error();
                if (errorCode != NativeMethods.ErrorAlreadyExists || !Exists(path))
                {
                    throw Common.GetExceptionFromWin32Error(errorCode);
                }
            }

            return new DirectoryInfo(path);
        }

        /// <summary>
        ///     Creates the specified directory.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="string"/> containing the path of the directory to create.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white
        ///     space, or contains one or more invalid characters as defined in
        ///     <see cref="Common_Library.LongPath.Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must not exceed
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        ///     <paramref name="path"/> contains one or more directories that could not be
        ///     found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        ///     <paramref name="path"/> is a file.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        /// <remarks>
        ///     Note: Unlike <see cref="Common_Library.LongPath.Directory.CreateDirectory(System.String)"/>, this method only creates
        ///     the last directory in <paramref name="path"/>.
        /// </remarks>
        public static DirectoryInfo CreateDirectory(String path)
        {
            if (Common.IsRunningOnMono())
            {
                return new DirectoryInfo(System.IO.Directory.CreateDirectory(path).FullName);
            }

            if (Common.IsPathUnc(path))
            {
                return CreateDirectoryUnc(path);
            }

            String normalizedPath = Path.NormalizeLongPath(path);
            String fullPath = Path.RemoveLongPathPrefix(normalizedPath);

            Int32 length = fullPath.Length;
            if (length >= 2 && Path.IsDirectorySeparator(fullPath[length - 1]))
            {
                --length;
            }

            Int32 rootLength = Path.GetRootLength(fullPath);

            List<String> pathComponents = new List<String>();

            if (length > rootLength)
            {
                for (Int32 index = length - 1; index >= rootLength; --index)
                {
                    String subPath = fullPath.Substring(0, index + 1);
                    if (!Exists(subPath))
                    {
                        pathComponents.Add(Path.NormalizeLongPath(subPath));
                    }

                    while (index > rootLength && fullPath[index] != System.IO.Path.DirectorySeparatorChar &&
                           fullPath[index] != System.IO.Path.AltDirectorySeparatorChar)
                    {
                        --index;
                    }
                }
            }

            while (pathComponents.Count > 0)
            {
                String str = pathComponents[pathComponents.Count - 1];
                pathComponents.RemoveAt(pathComponents.Count - 1);

                if (NativeMethods.CreateDirectory(str, IntPtr.Zero))
                {
                    continue;
                }

                // To mimic Directory.CreateDirectory, we don't throw if the directory (not a file) already exists
                Int32 errorCode = Marshal.GetLastWin32Error();
                if (errorCode != NativeMethods.ErrorAlreadyExists || !Exists(path))
                {
                    throw Common.GetExceptionFromWin32Error(errorCode);
                }
            }

            return new DirectoryInfo(fullPath);
        }

        public static String[] GetDirectories(String path, String searchPattern, SearchOption searchOption)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.GetDirectories(path, searchPattern, searchOption)
                : EnumerateFileSystemEntries(path, searchPattern, true, false, searchOption).ToArray();
        }

        public static String[] GetFiles(String path)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.GetFiles(path)
                : EnumerateFileSystemEntries(path, "*", false, true, SearchOption.TopDirectoryOnly).ToArray();
        }

        /// <summary>
        /// Test a directory for create file access permissions
        /// </summary>
        /// <param name="directoryPath">Full path to directory </param>
        /// <param name="accessRight">File System right tested</param>
        /// <returns>State [bool]</returns>
        public static Boolean HasPermission(String directoryPath, FileSystemRights accessRight)
        {
            while (true)
            {
                if (String.IsNullOrEmpty(directoryPath))
                {
                    return false;
                }

                if (!Exists(directoryPath))
                {
                    directoryPath = GetParent(directoryPath).DisplayPath;
                    continue;
                }

                try
                {
                    AuthorizationRuleCollection rules = GetAccessControl(directoryPath)
                        .GetAccessRules(true, true, typeof(SecurityIdentifier));
                    WindowsIdentity identity = WindowsIdentity.GetCurrent();

                    if ((from FileSystemAccessRule rule in rules
                        where identity.Groups != null && identity.Groups.Contains(rule.IdentityReference)
                        where (accessRight & rule.FileSystemRights) == accessRight
                        select rule).Any(rule => rule.AccessControlType != AccessControlType.Deny))
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                return false;
            }
        }

        public static unsafe void SetCreationTimeUtc(String path, DateTime creationTimeUtc)
        {
            if (Common.IsRunningOnMono())
            {
                System.IO.Directory.SetCreationTimeUtc(path, creationTimeUtc);
                return;
            }

            String normalizedPath = Path.NormalizeLongPath(Path.GetFullPath(path));

            using SafeFileHandle handle = GetDirectoryHandle(normalizedPath);
            NativeMethods.FileTime fileTime = new NativeMethods.FileTime(creationTimeUtc.ToFileTimeUtc());
            Boolean r = NativeMethods.SetFileTime(handle, &fileTime, null, null);
            if (r)
            {
                return;
            }

            Int32 errorCode = Marshal.GetLastWin32Error();
            Common.ThrowIOError(errorCode, path);
        }

        public static unsafe void SetLastWriteTimeUtc(String path, DateTime lastWriteTimeUtc)
        {
            if (Common.IsRunningOnMono())
            {
                System.IO.Directory.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
                return;
            }

            String normalizedPath = Path.NormalizeLongPath(Path.GetFullPath(path));

            using SafeFileHandle handle = GetDirectoryHandle(normalizedPath);
            NativeMethods.FileTime fileTime = new NativeMethods.FileTime(lastWriteTimeUtc.ToFileTimeUtc());
            Boolean r = NativeMethods.SetFileTime(handle, null, null, &fileTime);
            if (r)
            {
                return;
            }

            Int32 errorCode = Marshal.GetLastWin32Error();
            Common.ThrowIOError(errorCode, path);
        }

        public static unsafe void SetLastAccessTimeUtc(String path, DateTime lastWriteTimeUtc)
        {
            if (Common.IsRunningOnMono())
            {
                System.IO.Directory.SetLastAccessTimeUtc(path, lastWriteTimeUtc);
                return;
            }

            String normalizedPath = Path.NormalizeLongPath(Path.GetFullPath(path));

            using SafeFileHandle handle = GetDirectoryHandle(normalizedPath);
            NativeMethods.FileTime fileTime = new NativeMethods.FileTime(lastWriteTimeUtc.ToFileTimeUtc());
            Boolean r = NativeMethods.SetFileTime(handle, null, &fileTime, null);
            if (r)
            {
                return;
            }

            Int32 errorCode = Marshal.GetLastWin32Error();
            Common.ThrowIOError(errorCode, path);
        }

        public static DirectoryInfo GetParent(String path)
        {
            String directoryName = Path.GetDirectoryName(path);
            return directoryName == null ? null : new DirectoryInfo(directoryName);
        }

        public static DirectoryInfo CreateDirectory(String path, DirectorySecurity directorySecurity)
        {
            CreateDirectory(path);
            SetAccessControl(path, directorySecurity);
            return new DirectoryInfo(path);
        }

        public static DirectorySecurity GetAccessControl(String path)
        {
            const AccessControlSections includeSections =
                AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group;
            return GetAccessControl(path, includeSections);
        }

        public static DirectorySecurity GetAccessControl(String path, AccessControlSections includeSections)
        {
            if (Common.IsRunningOnMono())
            {
                return new DirectoryInfo(path).GetAccessControl(includeSections);
            }

            String normalizedPath = Path.NormalizeLongPath(Path.GetFullPath(path));
            SecurityInfos securityInfos = Common.ToSecurityInfos(includeSections);

            Int32 errorCode = (Int32) NativeMethods.GetSecurityInfoByName(normalizedPath,
                (UInt32) ResourceType.FileObject,
                (UInt32) securityInfos,
                out IntPtr _,
                out IntPtr _,
                out IntPtr _,
                out IntPtr _,
                out IntPtr byteArray);

            Common.ThrowIfError(errorCode, byteArray);

            UInt32 length = NativeMethods.GetSecurityDescriptorLength(byteArray);

            Byte[] binaryForm = new Byte[length];

            Marshal.Copy(byteArray, binaryForm, 0, (Int32) length);

            NativeMethods.LocalFree(byteArray);
            DirectorySecurity ds = new DirectorySecurity();
            ds.SetSecurityDescriptorBinaryForm(binaryForm);
            return ds;
        }

        public static DateTime GetCreationTime(String path)
        {
            return GetCreationTimeUtc(path).ToLocalTime();
        }

        public static DateTime GetCreationTimeUtc(String path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            return di.CreationTimeUtc;
        }

        public static String[] GetDirectories(String path)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.GetDirectories(path)
                : EnumerateFileSystemEntries(path, "*", true, false, SearchOption.TopDirectoryOnly).ToArray();
        }

        public static String[] GetDirectories(String path, String searchPattern)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.GetDirectories(path, searchPattern)
                : EnumerateFileSystemEntries(path, searchPattern, true, false, SearchOption.TopDirectoryOnly).ToArray();
        }

        public static String GetDirectoryRoot(String path)
        {
            String fullPath = Path.GetFullPath(path);
            return fullPath.Substring(0, Path.GetRootLength(fullPath));
        }

        public static String[] GetFiles(String path, String searchPattern)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.GetFiles(path, searchPattern)
                : EnumerateFileSystemEntries(path, searchPattern, false, true, SearchOption.TopDirectoryOnly).ToArray();
        }

        public static String[] GetFiles(String path, String searchPattern, SearchOption options)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.GetFiles(path, searchPattern, options)
                : EnumerateFileSystemEntries(path, searchPattern, false, true, options).ToArray();
        }

        public static String[] GetFileSystemEntries(String path)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.GetFileSystemEntries(path)
                : EnumerateFileSystemEntries(path, null, true, true, SearchOption.TopDirectoryOnly).ToArray();
        }

        public static String[] GetFileSystemEntries(String path, String searchPattern)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.GetFileSystemEntries(path, searchPattern)
                : EnumerateFileSystemEntries(path, searchPattern, true, true, SearchOption.TopDirectoryOnly).ToArray();
        }

        public static String[] GetFileSystemEntries(String path, String searchPattern, SearchOption options)
        {
            return Common.IsRunningOnMono()
                ? System.IO.Directory.GetFileSystemEntries(path, searchPattern)
                : EnumerateFileSystemEntries(path, searchPattern, true, true, options).ToArray();
        }

        public static DateTime GetLastAccessTime(String path)
        {
            return GetLastAccessTimeUtc(path).ToLocalTime();
        }

        public static DateTime GetLastAccessTimeUtc(String path)
        {
            if (Common.IsRunningOnMono())
            {
                return System.IO.Directory.GetLastAccessTimeUtc(path);
            }

            DirectoryInfo di = new DirectoryInfo(path);
            return di.LastAccessTimeUtc;
        }

        public static DateTime GetLastWriteTime(String path)
        {
            return GetLastWriteTimeUtc(path).ToLocalTime();
        }

        public static DateTime GetLastWriteTimeUtc(String path)
        {
            if (Common.IsRunningOnMono())
            {
                return System.IO.Directory.GetLastWriteTimeUtc(path);
            }

            DirectoryInfo di = new DirectoryInfo(path);
            return di.LastWriteTimeUtc;
        }

        public static String[] GetLogicalDrives()
        {
            return System.IO.Directory.GetLogicalDrives();
        }

        public static void SetAccessControl(String path, DirectorySecurity directorySecurity)
        {
            if (Common.IsRunningOnMono())
            {
                new DirectoryInfo(path).SetAccessControl(directorySecurity);
                return;
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (directorySecurity == null)
            {
                throw new ArgumentNullException(nameof(directorySecurity));
            }

            String name = Path.NormalizeLongPath(Path.GetFullPath(path));

            Common.SetAccessControlExtracted(directorySecurity, name);
        }

        public static void SetCreationTime(String path, DateTime creationTime)
        {
            SetCreationTimeUtc(path, creationTime.ToUniversalTime());
        }

        public static void SetLastAccessTime(String path, DateTime lastAccessTime)
        {
            SetLastAccessTimeUtc(path, lastAccessTime.ToUniversalTime());
        }

        public static void SetLastWriteTime(String path, DateTime lastWriteTimeUtc)
        {
            if (Common.IsRunningOnMono())
            {
                System.IO.Directory.SetLastWriteTime(path, lastWriteTimeUtc);
                return;
            }

            unsafe
            {
                String normalizedPath = Path.NormalizeLongPath(Path.GetFullPath(path));

                using SafeFileHandle handle = GetDirectoryHandle(normalizedPath);
                NativeMethods.FileTime fileTime = new NativeMethods.FileTime(lastWriteTimeUtc.ToFileTimeUtc());
                Boolean r = NativeMethods.SetFileTime(handle, null, null, &fileTime);
                if (r)
                {
                    return;
                }

                Int32 errorCode = Marshal.GetLastWin32Error();
                Common.ThrowIOError(errorCode, path);
            }
        }

        public static void SetCurrentDirectory(String path)
        {
            if (!Common.IsRunningOnMono())
            {
                throw new NotSupportedException(
                    "Windows does not support setting the current directory to a long path");
            }

            System.IO.Directory.SetCurrentDirectory(path);
#if true
#else
			string normalizedPath = Path.NormalizeLongPath(Path.GetFullPath(path));
			if (!NativeMethods.SetCurrentDirectory(normalizedPath))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == NativeMethods.ERROR_FILE_NOT_FOUND)
				{
					lastWin32Error = NativeMethods.ERROR_PATH_NOT_FOUND;
				}
				Common.ThrowIOError(lastWin32Error, normalizedPath);
			}
#endif
        }
    }
}