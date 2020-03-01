// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Net.Mail;

namespace Common_Library.Utils
{
    public static class EmailUtils
    {
        public static Boolean CheckValidEmail(String email)
        {
            try
            {
                MailAddress unused = new MailAddress(email);
            }
            catch (FormatException)
            {
                return false;
            }
            
            return true;
        }
    }
}