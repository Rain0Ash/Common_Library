// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Globalization;
using System.Text;
#if !NET_2_0
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
#endif

namespace Common_Library.LongPath
{
	public static class Path
	{
		public static readonly Char[] InvalidPathChars = System.IO.Path.GetInvalidPathChars();
		private static readonly Char[] InvalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
		internal const String LongPathPrefix = @"\\?\";
        internal const String UNCLongPathPrefix = @"\\?\UNC\";

		public static readonly Char DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
		public static readonly Char AltDirectorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar;
		public const Char VolumeSeparatorChar = ':';

		public static readonly Char PathSeparator = System.IO.Path.PathSeparator;

		internal static String NormalizeLongPath(String path)
		{
			return Common.IsRunningOnMono() ? path : NormalizeLongPath(path, "path");
		}

		// Normalizes path (can be longer than MAX_PATH) and adds \\?\ long path prefix
		internal static String NormalizeLongPath(String path, String parameterName)
		{
			if (path == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, @"'{0}' cannot be an empty string.", parameterName), parameterName);
            }

            if (Common.IsPathUnc(path))
            {
                return CheckAddLongPathPrefix(path);
            }

            StringBuilder buffer = new StringBuilder(path.Length + 1); // Add 1 for NULL
			UInt32 length = NativeMethods.GetFullPathName(path, (UInt32)buffer.Capacity, buffer, IntPtr.Zero);
			if (length > buffer.Capacity)
			{
				// Resulting path longer than our buffer, so increase it

				buffer.Capacity = (Int32)length;
				length = NativeMethods.GetFullPathName(path, length, buffer, IntPtr.Zero);
			}

			if (length == 0)
			{
				throw Common.GetExceptionFromLastWin32Error(parameterName);
			}

			if (length > NativeMethods.MaxLongPath)
			{
				throw Common.GetExceptionFromWin32Error(NativeMethods.ErrorFilenameExcedRange, parameterName);
			}

			if (length <= 1 || buffer[0] != DirectorySeparatorChar || buffer[1] != DirectorySeparatorChar)
			{
				return AddLongPathPrefix(buffer.ToString());
			}
			
			if (length < 2 || buffer.ToString().Split(new [] {DirectorySeparatorChar}, StringSplitOptions.RemoveEmptyEntries).Length < 2)
            {
                throw new ArgumentException("The UNC path should be of the form \\\\server\\share.");
            }

			return AddLongPathPrefix(buffer.ToString());
		}

		internal static Boolean TryNormalizeLongPath(String path, out String result)
		{
			try
			{
				result = NormalizeLongPath(path);
				return true;
			}
			catch (ArgumentException)
			{
			}
			catch (System.IO.PathTooLongException)
			{
			}

			result = null;
			return false;
		}

        internal static String CheckAddLongPathPrefix(String path)
        {
            if (String.IsNullOrEmpty(path) || path.StartsWith(@"\\?\"))
            {
                return path;
            }

            Int32 maxPathLimit = NativeMethods.MaxPath;
            if (!Uri.TryCreate(path, UriKind.Absolute, out Uri uri) || !uri.IsUnc)
            {
	            return path.Length >= maxPathLimit ? AddLongPathPrefix(path) : path;
            }

            // What's going on here?  Empirical evidence shows that Windows has trouble dealing with UNC paths
            // longer than MAX_PATH *minus* the length of the "\\hostname\" prefix.  See the following tests:
            //  - UncDirectoryTests.TestDirectoryCreateNearMaxPathLimit
            //  - UncDirectoryTests.TestDirectoryEnumerateDirectoriesNearMaxPathLimit
            Int32 rootPathLength = 3 + uri.Host.Length;
            maxPathLimit -= rootPathLength;

            return path.Length >= maxPathLimit ? AddLongPathPrefix(path) : path;
        }

		internal static String RemoveLongPathPrefix(String normalizedPath)
		{

            if (String.IsNullOrEmpty(normalizedPath) || !normalizedPath.StartsWith(LongPathPrefix))
            {
                return normalizedPath;
            }

            return normalizedPath.StartsWith(UNCLongPathPrefix, StringComparison.InvariantCultureIgnoreCase) ? $@"\\{normalizedPath.Substring(UNCLongPathPrefix.Length)}"
	            : normalizedPath.Substring(LongPathPrefix.Length);
		}

		private static String AddLongPathPrefix(String path)
		{
            if (String.IsNullOrEmpty(path) || path.StartsWith(LongPathPrefix))
            {
                return path;
            }

            // http://msdn.microsoft.com/en-us/library/aa365247.aspx
            if (path.StartsWith(@"\\"))
            {
                // UNC.
                return UNCLongPathPrefix + path.Substring(2);
            }

			return LongPathPrefix + path;
		}

		public static String Combine(String path1, String path2)
		{
			if (path1 == null || path2 == null)
            {
                throw new ArgumentNullException(path1 == null ? "path1" : "path2");
            }

            CheckInvalidPathChars(path1);
			CheckInvalidPathChars(path2);
			if (path2.Length == 0)
            {
                return path1;
            }

            if (path1.Length == 0 || IsPathRooted(path2))
            {
                return path2;
            }

            Char ch = path1[path1.Length - 1];
			if (ch != DirectorySeparatorChar && ch != AltDirectorySeparatorChar &&
				ch != VolumeSeparatorChar)
            {
                return path1 + DirectorySeparatorChar + path2;
            }

            return path1 + path2;
		}

		public static Boolean IsPathRooted(String path)
		{
			return System.IO.Path.IsPathRooted(path);
		}

		public static String Combine(String path1, String path2, String path3)
		{
			if (path1 == null || path2 == null || path3 == null)
            {
                throw new ArgumentNullException(path1 == null ? "path1" : path2 == null ? "path2" : "path3");
            }

            return Combine(Combine(path1, path2), path3);
		}

		public static String Combine(String path1, String path2, String path3, String path4)
		{
			if (path1 == null || path2 == null || path3 == null || path4 == null)
            {
                throw new ArgumentNullException(path1 == null ? "path1" : path2 == null ? "path2" : path3 == null ? "path3" : "path4");
            }

            return Combine(Combine(Combine(path1, path2), path3), path4);
		}

		private static void CheckInvalidPathChars(String path)
		{
			if (HasIllegalCharacters(path))
            {
                throw new ArgumentException(@"Invalid characters in path", nameof(path));
            }
        }

		private static Boolean HasIllegalCharacters(String path)
		{
#if NET_2_0
			foreach (var e in path)
			{
				if (InvalidPathChars.Contains(e))
				{
					return true;
				}
			}
			return false;
#else
			return path.Any(InvalidPathChars.Contains);
#endif
		}

		public static String GetFileName(String path)
		{
			return path == null ? null : System.IO.Path.GetFileName(NormalizeLongPath(path));
		}

		public static String GetFullPath(String path)
		{
			return Common.IsPathUnc(path) ? path : RemoveLongPathPrefix(NormalizeLongPath(path));
		}

		public static String GetDirectoryName(String path)
		{
		    if (Common.IsRunningOnMono())
            {
                return System.IO.Path.GetDirectoryName(path);
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            CheckInvalidPathChars(path);
		    String basePath = null;
            if (!IsPathRooted(path))
            {
                basePath = System.IO.Directory.GetCurrentDirectory();
            }

            path = RemoveLongPathPrefix(NormalizeLongPath(path));
		    Int32 rootLength = GetRootLength(path);
		    
            if (path.Length <= rootLength)
            {
                return null;
            }

            Int32 length = path.Length;
			do
			{
			} while (length > rootLength && path[--length] != System.IO.Path.DirectorySeparatorChar &&
					 path[length] != System.IO.Path.AltDirectorySeparatorChar);

			if (basePath == null)
			{
				return path.Substring(0, length);
			}

			path = path.Substring(basePath.Length + 1);
		    length = length - basePath.Length - 1;
		    if (length < 0)
            {
                length = 0;
            }

            return path.Substring(0, length);
		}

	    private static Int32 GetUncRootLength(String path)
        {
            String[] components = path.Split(new[] { DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            String root = $@"\\{components[0]}\{components[1]}\";
	        return root.Length;
        }

		internal static Int32 GetRootLength(String path)
		{
			if (Common.IsPathUnc(path))
            {
                return GetUncRootLength(path);
            }

            path = GetFullPath(path);
			CheckInvalidPathChars(path);
			Int32 rootLength = 0;
			Int32 length = path.Length;
			if (length >= 1 && IsDirectorySeparator(path[0]))
			{
				rootLength = 1;
				if (length < 2 || !IsDirectorySeparator(path[1]))
				{
					return rootLength;
				}

				rootLength = 2;
				Int32 num = 2;
				while (rootLength >= length ||
				       (path[rootLength] == System.IO.Path.DirectorySeparatorChar ||
				        path[rootLength] == System.IO.Path.AltDirectorySeparatorChar) && --num <= 0)
                {
                    ++rootLength;
                }
            }
			else if (length >= 2 && path[1] == System.IO.Path.VolumeSeparatorChar)
			{
				rootLength = 2;
				if (length >= 3 && IsDirectorySeparator(path[2]))
                {
                    ++rootLength;
                }
            }
			return rootLength;
		}

		internal static Boolean IsDirectorySeparator(Char c)
		{
			return c == DirectorySeparatorChar || c == AltDirectorySeparatorChar;
		}

		public static Char[] GetInvalidPathChars()
		{
			return InvalidPathChars;
		}

		public static Char[] GetInvalidFileNameChars()
		{
			return InvalidFileNameChars;
		}

		public static String GetRandomFileName()
		{
			return System.IO.Path.GetRandomFileName();
		}

		public static String GetPathRoot(String path)
		{
			if (path == null)
            {
                return null;
            }

            if (!IsPathRooted(path))
			{
				return String.Empty;
			}

			if(!Common.IsPathUnc(path))
            {
                path = RemoveLongPathPrefix(NormalizeLongPath(path));
            }

            return path.Substring(0, GetRootLength(path));
		}

		public static String GetExtension(String path)
		{
			return System.IO.Path.GetExtension(path);
		}

		public static Boolean HasExtension(String path)
		{
			return System.IO.Path.HasExtension(path);
		}

		public static String GetTempPath()
		{
			return System.IO.Path.GetTempPath();
		}

		public static String GetTempFileName()
		{
			return System.IO.Path.GetTempFileName();
		}

		public static String GetFileNameWithoutExtension(String path)
		{
			return System.IO.Path.GetFileNameWithoutExtension(path);
		}

		public static String ChangeExtension(String filename, String extension)
		{
			return System.IO.Path.ChangeExtension(filename, extension);
		}

		public static String Combine(String[] paths)
		{
			if(paths == null)
            {
                throw new ArgumentNullException(nameof(paths));
            }

            switch (paths.Length)
            {
	            case 0:
		            return String.Empty;
	            case 1:
		            return paths[0];
            }

            String path = paths[0];
			for (Int32 i = 1; i < paths.Length; ++i)
			{
				path = Combine(path, paths[i]);
			}
			return path;
		}
	}
}