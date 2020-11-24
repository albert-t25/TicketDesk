using System.Net;
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
            SmtpClient client = new SmtpClient(Settings.Default.Host)
            {
                Port = Settings.Default.Port,
                EnableSsl = Settings.Default.EnableSSL,
                Credentials = new NetworkCredential(Settings.Default.UserName, Settings.Default.Password)
            };

            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress(Settings.Default.From),
                Subject = subject,
                IsBodyHtml = true,
                Body = body
            };
            //mailMessage.To.Add("enxhela.rustani@pragmatic.al");
            mailMessage.To.Add("enrustani@gmail.com");
            //mailMessage.To.Add(to);

            client.Send(mailMessage);
        }

    }
}
