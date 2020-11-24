﻿using System.Net;
using System.Net.Mail;
using TicketDesk.Web.Identity.Properties;
namespace TicketDesk.Web.Identity.Model
{
    public class EmailHelper
    {
        public static void SendEmail(string subject, string body)
        {
            SendEmail(Settings.Default.AfraEmail, subject, body);
        }
        public static void SendEmail(string to, string subject, string body)
        {
            SmtpClient client = new SmtpClient(Settings.Default.Host);
            client.Port = Settings.Default.Port;
            client.EnableSsl = Settings.Default.EnableSSL;
            client.Credentials = new NetworkCredential(Settings.Default.UserName, Settings.Default.Password);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Settings.Default.From);
            //mailMessage.To.Add("enxhela.rustani@pragmatic.al");
            mailMessage.To.Add("enrustani@gmail.com");
            //mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;

            client.Send(mailMessage);
        }
        
    }
}
