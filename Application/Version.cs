// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Common_Library.Utils;

namespace Common_Library.App
{
    public static class App
    {
        public enum Status
        {
            None,
            NotFunctional,
            PreAlpha,
            ClosedAlpha,
            Alpha,
            OpenAlpha,
            PreBeta,
            ClosedBeta,
            Beta,
            OpenBeta,
            Release
        }

        public enum Branch
        {
            Master,
            Prototype,
            Develop,
            NewArchitecture
        }

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static AppVersion Version = new AppVersion();

        public static Boolean AlreadyStarted
        {
            get
            {
                return Version.AlreadyStarted;
            }
        }
    }

    public struct AppVersion : IComparable<AppVersion>, IEquatable<AppVersion>, IDisposable
    {
        private static readonly IReadOnlyDictionary<App.Status, String> StatusDictionary = new Dictionary<App.Status, String>
        {
            [App.Status.None] = String.Empty,
            [App.Status.NotFunctional] = "NF",
            [App.Status.PreAlpha] = "PA",
            [App.Status.ClosedAlpha] = "CA",
            [App.Status.Alpha] = "A",
            [App.Status.OpenAlpha] = "OA",
            [App.Status.PreBeta] = "PB",
            [App.Status.ClosedBeta] = "CB",
            [App.Status.Beta] = "B",
            [App.Status.OpenBeta] = "OB",
            [App.Status.Release] = "R",
        };

        private static readonly IReadOnlyDictionary<App.Branch, String> BranchDictionary = new Dictionary<App.Branch, String>
        {
            [App.Branch.Master] = String.Empty,
            [App.Branch.Prototype] = "P",
            [App.Branch.Develop] = "DEV",
            [App.Branch.NewArchitecture] = "NA"
        };

        public static Boolean operator ==(AppVersion first, AppVersion second)
        {
            return first.CompareTo(second) == 0;
        }

        public static Boolean operator !=(AppVersion first, AppVersion second)
        {
            return !(first == second);
        }

        public static Boolean operator >(AppVersion first, AppVersion second)
        {
            return first.CompareTo(second) > 0;
        }

        public static Boolean operator <(AppVersion first, AppVersion second)
        {
            return first.CompareTo(second) < 0;
        }

        public static Boolean operator >=(AppVersion first, AppVersion second)
        {
            return first.CompareTo(second) >= 0;
        }

        public static Boolean operator <=(AppVersion first, AppVersion second)
        {
            return first.CompareTo(second) <= 0;
        }

        public String AppName { get; }
        public IComparable Version { get; }

        public App.Status Status { get; }

        public App.Branch Branch { get; }

        private readonly Mutex _mutex;

        public Boolean AlreadyStarted
        {
            get
            {
                return _mutex?.WaitOne(0) != true;
            }
        }
        
        public AppVersion(IComparable version, App.Status status = App.Status.Release, App.Branch branch = App.Branch.Master)
            : this(Process.GetCurrentProcess().ProcessName, version, status, branch)
        {
        }
        
        public AppVersion(String name, IComparable version, App.Status status = App.Status.Release, App.Branch branch = App.Branch.Master)
        {
            AppName = name;
            Version = version;
            Status = status;
            Branch = branch;
            
            _mutex = new Mutex(true, AppName);
        }

        public static DateTime GetBuildDateTime(String path)
        {
            return !File.Exists(path) ? new DateTime() : File.GetLastWriteTime(path);
        }

        public String GetVersion()
        {
            return Convert.ToString(Version, CultureInfo.InvariantCulture);
        }

        public String GetStatus()
        {
            return StatusDictionary[Status];
        }

        public String GetBranch()
        {
            return BranchDictionary[Branch];
        }

        public Int32 CompareTo(AppVersion other)
        {
            return CompareTo(other, false);
        }

        public Int32 CompareTo(AppVersion other, Boolean versionFirst)
        {
            if (AppName != other.AppName)
            {
                throw new ArgumentException("Application names is not equals");
            }
            
            Int32 versionCompare = ComparerUtils.ToCompare(Version, other.Version, false) ?? 0;

            if (versionFirst && versionCompare != 0)
            {
                return versionCompare;
            }

            if (Status != App.Status.None && other.Status != App.Status.None)
            {
                return Status.CompareTo(other.Status);
            }

            return versionCompare;
        }

        public override String ToString()
        {
            String version = GetVersion();
            String status = GetStatus();
            String branch = GetBranch();
            return $"{version}:{status}{branch}";
        }

        public Boolean Equals(AppVersion other)
        {
            return Version.Equals(other.Version) && Status == other.Status && Branch == other.Branch;
        }

        public override Boolean Equals(Object? obj)
        {
            return obj is AppVersion other && Equals(other);
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hashCode = Version.GetHashCode();
                hashCode = (hashCode * 397) ^ (Int32) Status;
                hashCode = (hashCode * 397) ^ (Int32) Branch;
                return hashCode;
            }
        }

        public void Dispose()
        {
            try
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}