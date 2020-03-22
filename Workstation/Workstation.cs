// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Security.Principal;

namespace Common_Library.Workstation
{
    public static partial class WorkStation
    {
        public static readonly String CurrentUserSID = GetCurrentUserSID();

        public static String GetCurrentUserSID()
        {
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            SecurityIdentifier sid = user.User;
            return sid?.Value;
        }
    }
}