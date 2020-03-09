// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Common_Library.LongPath
{
    using Win32Exception=global::System.ComponentModel.Win32Exception;
    using PrivilegeNotHeldException=System.Security.AccessControl.PrivilegeNotHeldException;
	using System.Security.Principal;

    public delegate void PrivilegedCallback(Object state);

	/// <summary>
	/// From MSDN Magazine March 2005
	/// </summary>
    public sealed class Privilege
    {
        private static readonly LocalDataStoreSlot TlsSlot = Thread.AllocateDataSlot();
        private static readonly HybridDictionary Privileges = new HybridDictionary();
        private static readonly HybridDictionary Luids = new HybridDictionary();
        private static readonly ReaderWriterLock PrivilegeLock = new ReaderWriterLock();

        private Boolean _initialState;
        private Boolean _stateWasChanged;
        private readonly NativeMethods.Luid _luid;
        private readonly Thread _currentThread = Thread.CurrentThread;
        private TlsContents _tlsContents;

		// ReSharper disable UnusedMember.Global
        public const String CreateToken                     = "SeCreateTokenPrivilege";
        public const String AssignPrimaryToken              = "SeAssignPrimaryTokenPrivilege";
        public const String LockMemory                      = "SeLockMemoryPrivilege";
        public const String IncreaseQuota                   = "SeIncreaseQuotaPrivilege";
        public const String UnsolicitedInput                = "SeUnsolicitedInputPrivilege";
        public const String MachineAccount                  = "SeMachineAccountPrivilege";
        public const String TrustedComputingBase            = "SeTcbPrivilege";
        public const String Security                        = "SeSecurityPrivilege";
        public const String TakeOwnership                   = "SeTakeOwnershipPrivilege";
        public const String LoadDriver                      = "SeLoadDriverPrivilege";
        public const String SystemProfile                   = "SeSystemProfilePrivilege";
        public const String SystemTime                      = "SeSystemtimePrivilege";
        public const String ProfileSingleProcess            = "SeProfileSingleProcessPrivilege";
        public const String IncreaseBasePriority            = "SeIncreaseBasePriorityPrivilege";
        public const String CreatePageFile                  = "SeCreatePagefilePrivilege";
        public const String CreatePermanent                 = "SeCreatePermanentPrivilege";
        public const String Backup                          = "SeBackupPrivilege";
        public const String Restore                         = "SeRestorePrivilege";
        public const String Shutdown                        = "SeShutdownPrivilege";
        public const String Debug                           = "SeDebugPrivilege";
        public const String Audit                           = "SeAuditPrivilege";
        public const String SystemEnvironment               = "SeSystemEnvironmentPrivilege";
        public const String ChangeNotify                    = "SeChangeNotifyPrivilege";
        public const String RemoteShutdown                  = "SeRemoteShutdownPrivilege";
        public const String Undock                          = "SeUndockPrivilege";
        public const String SyncAgent                       = "SeSyncAgentPrivilege";
        public const String EnableDelegation                = "SeEnableDelegationPrivilege";
        public const String ManageVolume                    = "SeManageVolumePrivilege";
        public const String Impersonate                     = "SeImpersonatePrivilege";
        public const String CreateGlobal                    = "SeCreateGlobalPrivilege";
        public const String TrustedCredentialManagerAccess  = "SeTrustedCredManAccessPrivilege";
        public const String ReserveProcessor                = "SeReserveProcessorPrivilege";
		// ReSharper restore UnusedMember.Global


        //
        // This routine is a wrapper around a hashtable containing mappings
        // of privilege names to luids
        //

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        private static NativeMethods.Luid LuidFromPrivilege(String privilege)
        {
            NativeMethods.Luid luid;
            luid.LowPart = 0;
            luid.HighPart = 0;

            //
            // Look up the privilege LUID inside the cache
            //

            RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                PrivilegeLock.AcquireReaderLock(Timeout.Infinite);

                if (Luids.Contains(privilege))
                {
                    luid = (NativeMethods.Luid)Luids[ privilege ];

                    PrivilegeLock.ReleaseReaderLock();
                }
                else
                {
                    PrivilegeLock.ReleaseReaderLock();

                    if (false == NativeMethods.LookupPrivilegeValue(null, privilege, ref luid))
                    {
                        Int32 error = Marshal.GetLastWin32Error();

                        throw error switch
                        {
                            NativeMethods.ErrorNotEnoughMemory => (Exception) new OutOfMemoryException(),
                            NativeMethods.ErrorAccessDenied => new UnauthorizedAccessException(
                                "Caller does not have the rights to look up privilege local unique identifier"),
                            NativeMethods.ErrorNoSuchPrivilege => new ArgumentException(
                                $@"{privilege} is not a valid privilege name", nameof(privilege)),
                            _ => new Win32Exception(error)
                        };
                    }

                    PrivilegeLock.AcquireWriterLock(Timeout.Infinite);
                }
            }
            finally
            {
                if (PrivilegeLock.IsReaderLockHeld)
                {
                    PrivilegeLock.ReleaseReaderLock();
                }

                if (PrivilegeLock.IsWriterLockHeld)
                {
                    if (!Luids.Contains(privilege))
                    {
                        Luids[ privilege ] = luid;
                        Privileges[ luid ] = privilege;
                    }

                    PrivilegeLock.ReleaseWriterLock();
                }
            }

            return luid;
        }

        private sealed class TlsContents : IDisposable
        {
            private Boolean _disposed;
            private SafeTokenHandle _threadHandle = new SafeTokenHandle(IntPtr.Zero);

            private static SafeTokenHandle processHandle = new SafeTokenHandle(IntPtr.Zero);
            private static readonly Object SyncRoot = new Object();

            
            public TlsContents()
            {
                Int32 error = 0;
				Int32 cachingError = 0;
                Boolean success = true;

                if (processHandle.IsInvalid)
                {
                    lock (SyncRoot)
                    {
                        if (processHandle.IsInvalid)
                        {
                            if (false == NativeMethods.OpenProcessToken(
                                            NativeMethods.GetCurrentProcess(),
                                            TokenAccessLevels.Duplicate,
                                            ref processHandle))
                            {
                                cachingError = Marshal.GetLastWin32Error();
                                success = false;
                            }
                        }
                    }
                }

                RuntimeHelpers.PrepareConstrainedRegions();

                try
                {
                    //
                    // Open the thread token; if there is no thread token,
                    // copy the process token onto the thread
                    //

					if (false == NativeMethods.OpenThreadToken(
						NativeMethods.GetCurrentThread(),
						TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges,
						true,
						ref _threadHandle))
					{
						if (success)
						{
							error = Marshal.GetLastWin32Error();

							if (error != NativeMethods.ErrorNoToken)
							{
								success = false;
							}

							if (success)
							{
								error = 0;

								if (false == NativeMethods.DuplicateTokenEx(
									processHandle,
									TokenAccessLevels.Impersonate | TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges,
									IntPtr.Zero,
									NativeMethods.SecurityImpersonationLevel.Impersonation,
									NativeMethods.TokenType.Impersonation,
									ref _threadHandle))
								{
									error = Marshal.GetLastWin32Error();
									success = false;
								}
							}

							if (success)
							{
								if (false == NativeMethods.SetThreadToken(
									IntPtr.Zero,
									_threadHandle))
								{
									error = Marshal.GetLastWin32Error();
									success = false;
								}
							}

							if (success)
							{
								//
								// This thread is now impersonating; it needs to be reverted to its original state
								//

								IsImpersonating = true;
							}
						}
						else
						{
							error = cachingError;
						}
					}
					else
					{
						success = true;
					}
                }
                finally
                {
                    if (!success)
                    {
                        Dispose();
                    }
                }

                switch (error)
                {
                    case NativeMethods.ErrorNotEnoughMemory:
                        throw new OutOfMemoryException();
                    case NativeMethods.ErrorAccessDenied:
                    case NativeMethods.ErrorCantOpenAnonymous:
                        throw new UnauthorizedAccessException("The caller does not have the rights to perform the operation");
                    default:
                    {
                        if (error != 0)
                        {
                            throw new Win32Exception(error);
                        }

                        break;
                    }
                }
            }

            ~TlsContents()
            {
                if (!_disposed)
                {
                    Dispose(false);
                }
            }
            

            
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            // ReSharper disable once UnusedParameter.Local
            private void Dispose(Boolean disposing)
            {
                if (_disposed)
                {
                    return;
                }

                if (_threadHandle != null)
                {
                    _threadHandle.Dispose();
                    _threadHandle = null;
                }

                if (IsImpersonating)
                {
                    NativeMethods.RevertToSelf();
                }

                _disposed = true;
            }
            

            
            public void IncrementReferenceCount()
            {
                ReferenceCountValue++;
            }

            public Int32 DecrementReferenceCount()
            {
                Int32 result = --ReferenceCountValue;

                if (result == 0)
                {
                    Dispose();
                }

                return result;
            }

            public Int32 ReferenceCountValue { get; private set; } = 1;

            

            
            public SafeTokenHandle ThreadHandle
            {
                get { return _threadHandle; }
            }

            public Boolean IsImpersonating { get; }

            
            
        }

        public Privilege(String privilegeName)
        {
            if (privilegeName == null)
            {
                throw new ArgumentNullException(nameof(privilegeName));
            }

            _luid = LuidFromPrivilege(privilegeName);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public void Enable()
        {
            ToggleState(true);
        }

#if NOT_USED
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public void Disable()
        {
            this.ToggleState(false);
        }
#endif

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public void Revert()
        {
            Int32 error = 0;

            //
            // All privilege operations must take place on the same thread
            //

            if (!_currentThread.Equals(Thread.CurrentThread))
            {
                throw new InvalidOperationException("Operation must take place on the thread that created the object");
            }

            if (!NeedToRevert)
            {
                return;
            }

            //
            // This code must be eagerly prepared and non-interruptible.
            //

            RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                //
                // The payload is entirely in the finally block
                // This is how we ensure that the code will not be
                // interrupted by catastrophic exceptions
                //
            }
            finally
            {
                Boolean success = true;

                try
                {
                    //
                    // Only call AdjustTokenPrivileges if we're not going to be reverting to self,
                    // on this Revert, since doing the latter obliterates the thread token anyway
                    //

                    if (_stateWasChanged &&
                        (_tlsContents.ReferenceCountValue > 1 ||
                        !_tlsContents.IsImpersonating))
                    {
	                    NativeMethods.TokenPrivilege newState = new NativeMethods.TokenPrivilege
	                    {
		                    PrivilegeCount = 1,
		                    Privilege =
		                    {
			                    Luid = _luid,
			                    Attributes =
				                    _initialState ? NativeMethods.SePrivilegeEnabled : NativeMethods.SePrivilegeDisabled
		                    }
	                    };

	                    NativeMethods.TokenPrivilege previousState = new NativeMethods.TokenPrivilege();
                        UInt32 previousSize = 0;

                        if (false == NativeMethods.AdjustTokenPrivileges(
                                        _tlsContents.ThreadHandle,
                                        false,
                                        ref newState,
                                        (UInt32)Marshal.SizeOf(previousState),
                                        ref previousState,
                                        ref previousSize))
                        {
                            error = Marshal.GetLastWin32Error();
                            success = false;
                        }
                    }
                }
                finally
                {
                    if (success)
                    {
                        Reset();
                    }
                }
            }

            switch (error)
            {
                case NativeMethods.ErrorNotEnoughMemory:
                    throw new OutOfMemoryException();
                case NativeMethods.ErrorAccessDenied:
                    throw new UnauthorizedAccessException("Caller does not have the permission to change the privilege");
                default:
                {
                    if (error != 0)
                    {
                        throw new Win32Exception(error);
                    }

                    break;
                }
            }
        }

        public Boolean NeedToRevert { get; private set; }

#if NOT_USED
        public static void RunWithPrivilege(string privilege, bool enabled, PrivilegedCallback callback, object state)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            Privilege p = new Privilege(privilege);

            RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                if (enabled)
                {
                    p.Enable();
                }
                else
                {
                    p.Disable();
                }

                callback(state);
            }
            catch (Exception)
{
                p.Revert();
                throw;
            }
            finally
            {
                p.Revert();
            }
        }
#endif
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        private void ToggleState(Boolean enable)
        {
            Int32 error = 0;

            //
            // All privilege operations must take place on the same thread
            //

            if (!_currentThread.Equals(Thread.CurrentThread))
            {
                throw new InvalidOperationException("Operation must take place on the thread that created the object");
            }

            //
            // This privilege was already altered and needs to be reverted before it can be altered again
            //

            if (NeedToRevert)
            {
                throw new InvalidOperationException("Must revert the privilege prior to attempting this operation");
            }

            //
            // Need to make this block of code non-interruptible so that it would preserve
            // consistency of thread oken state even in the face of catastrophic exceptions
            //

            RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                //
                // The payload is entirely in the finally block
                // This is how we ensure that the code will not be
                // interrupted by catastrophic exceptions
                //
            }
            finally
            {
                try
                {
                    //
                    // Retrieve TLS state
                    //

                    _tlsContents = Thread.GetData(TlsSlot) as TlsContents;

                    if (_tlsContents == null)
                    {
                        _tlsContents = new TlsContents();
                        Thread.SetData(TlsSlot, _tlsContents);
                    }
                    else
                    {
                        _tlsContents.IncrementReferenceCount();
                    }

	                NativeMethods.TokenPrivilege newState = new NativeMethods.TokenPrivilege
	                {
		                PrivilegeCount = 1,
		                Privilege =
		                {
			                Luid = _luid,
			                Attributes = enable ? NativeMethods.SePrivilegeEnabled : NativeMethods.SePrivilegeDisabled
		                }
	                };

	                NativeMethods.TokenPrivilege previousState = new NativeMethods.TokenPrivilege();
                    UInt32 previousSize = 0;

                    //
                    // Place the new privilege on the thread token and remember the previous state.
                    //

                    if (false == NativeMethods.AdjustTokenPrivileges(
                                    _tlsContents.ThreadHandle,
                                    false,
                                    ref newState,
                                    (UInt32)Marshal.SizeOf(previousState),
                                    ref previousState,
                                    ref previousSize))
                    {
                        error = Marshal.GetLastWin32Error();
                    }
                    else if (NativeMethods.ErrorNotAllAssigned == Marshal.GetLastWin32Error())
                    {
                        error = NativeMethods.ErrorNotAllAssigned;
                    }
                    else
                    {
                        //
                        // This is the initial state that revert will have to go back to
                        //

                        _initialState = (previousState.Privilege.Attributes & NativeMethods.SePrivilegeEnabled) != 0;

                        //
                        // Remember whether state has changed at all
                        //

                        _stateWasChanged = _initialState != enable;

                        //
                        // If we had to impersonate, or if the privilege state changed we'll need to revert
                        //

                        NeedToRevert = _tlsContents.IsImpersonating || _stateWasChanged;
                    }
                }
                finally
                {
                    if (!NeedToRevert)
                    {
                        Reset();
                    }
                }
            }

            switch (error)
            {
                case NativeMethods.ErrorNotAllAssigned:
                    throw new PrivilegeNotHeldException(Privileges[_luid] as String);
                case NativeMethods.ErrorNotEnoughMemory:
                    throw new OutOfMemoryException();
                case NativeMethods.ErrorAccessDenied:
                case NativeMethods.ErrorCantOpenAnonymous:
                    throw new UnauthorizedAccessException("The caller does not have the right to change the privilege");
            }

            if (error != 0)
            {
                throw new Win32Exception(error);
            }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        private void Reset()
        {
            RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                // Payload is in the finally block
                // as a way to guarantee execution
            }
            finally
            {
                _stateWasChanged = false;
                _initialState = false;
                NeedToRevert = false;

                if (_tlsContents != null)
                {
                    if (0 == _tlsContents.DecrementReferenceCount())
                    {
                        _tlsContents = null;
                        Thread.SetData(TlsSlot, null);
                    }
                }
            }
        }
    }
}
