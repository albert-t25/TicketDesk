using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.IO;

namespace TicketDesk.ReceiveEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            Pop3Client client = new Pop3Client();
            client.Connect(Properties.Settings.Default.MailServer, Properties.Settings.Default.MailPort, Properties.Settings.Default.MailUseSSL);
            client.Authenticate(Properties.Settings.Default.Username, Properties.Settings.Default.Password);

            //if no existing create the directory to save the number of email read
            string currentPath = Directory.GetCurrentDirectory();
            string folder = String.Format("{0}\\LastEmail", currentPath);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string fileName = "LastNumber.txt";
            string fullPath = folder + "\\" + fileName;

            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Dispose();
                //for the first time that the file is created write the 0 number of email that are read.
                using (StreamWriter write = new StreamWriter(fullPath))
                {
                    write.WriteLine("0");
                }
            }

            int theLastMessageNumber = 0;

            using (StreamReader readNumber = new StreamReader(fullPath))
            {
                theLastMessageNumber = Convert.ToInt32(readNumber.ReadLine());
            }

            int messageCount = client.GetMessageCount();

            for (int i = theLastMessageNumber + 1; i <= messageCount; i++)
            {
                Message message = client.GetMessage(i);

                MessagePart mesgPart = message.MessagePart.MessageParts[0];

                string body = mesgPart.BodyEncoding.GetString(mesgPart.Body);

                Ticket.SubmitTicket newTicket = new Ticket.SubmitTicket();

                bool result = newTicket.EmailAddTicket("Kerkese", "Nga email", message.Headers.Subject, "Email personit qe ka derguar kete email: " + message.Headers.From + "\nPermbajtja e email eshte: \n" + body, "Mesem", false, message.Headers.DateSent);

                using (StreamWriter write = new StreamWriter(fullPath))
                {
                    write.WriteLine(i + 1);
                }
            }
        }
    }
}
