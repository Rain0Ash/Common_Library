// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using JetBrains.Annotations;

namespace Common_Library.Mail
{
    public class Mail
    {
        public SmtpClient SmtpClient { get; }

        public MailAddress Address { get; set; }
        
        public delegate void EmailHandler(Object sender, Boolean cancelled, Exception exception);

        public event EmailHandler EmailSendCompleted;
        
        public Mail([NotNull] SmtpClient smtpClient, [NotNull] MailAddress address)
        {
            SmtpClient = smtpClient;
            SmtpClient.SendCompleted += OnEmailSend;
            
            Address = address;
        }

        private void OnEmailSend(Object sender, AsyncCompletedEventArgs args)
        {
            EmailSendCompleted?.Invoke(sender, args.Cancelled, args.Error);
        }

        public MailMessage GetMessage([NotNull] IEnumerable<MailAddress> to, [NotNull] String subject, [NotNull] String body, Boolean replyToAdmin = false)
        {
            return GetMessage(Address, to, subject, body, replyToAdmin);
        }

        public Boolean Send([NotNull] MailMessage message, out Exception exception)
        {
            try
            {
                SmtpClient.Send(message);
                exception = null;
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        public void SendAsync([NotNull] MailMessage message)
        {
            SmtpClient.SendMailAsync(message);
        }
        
        public static MailMessage GetMessage([NotNull] MailAddress from, [NotNull] IEnumerable<MailAddress> to, [NotNull] String subject, [NotNull] String body, Boolean replyToAdmin = false)
        {
            MailMessage message = new MailMessage
            {
                From = from,
                Subject = subject,
                Body = body
            };

            if (replyToAdmin)
            {
                message.ReplyToList.Add(from);
            }

            foreach (MailAddress address in to)
            {
                message.To.Add(address);
            }

            return message;
        }
    }
}