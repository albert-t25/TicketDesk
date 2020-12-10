using log4net;
using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace TicketDesk.Web.Identity.Model
{
    public class SmsHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void SendSms(string toNumber, string projectName)
        {
            toNumber = Properties.Settings.Default.ToNumber;
            bool connected;

            TcpClient smsServer = null;

            try
            {
                smsServer = OpenConnection(Properties.Settings.Default.IpTCP, Properties.Settings.Default.PortTCP, out connected);
                if (connected)
                {
                    Log.Info("Connected -> " + connected + "->" + smsServer.Client.AddressFamily.ToString());
                }
                else
                {
                    Log.Error("Connected -> " + connected + "->" + smsServer.Client.AddressFamily.ToString());
                }
                if (connected)
                {
                    string sms = String.Format(Properties.Settings.Default.SmsTemplateForNewTask, projectName.Replace('ë', 'e'));

                    SendSmsToClient(smsServer, Properties.Settings.Default.FromNumber, toNumber, sms);

                }
            }
            finally
            {
                CloseConnection(smsServer);
            }
        }

        protected static TcpClient OpenConnection(string ip, int port, out bool connected)
        {

            string response = string.Empty;
            string message = string.Empty;

            TcpClient tcpClient = new TcpClient();

            try
            {
                ASCIIEncoding ascEn = new ASCIIEncoding();

                tcpClient.Connect(ip, port);

                Stream stream = tcpClient.GetStream();

                byte[] bb = new byte[100];
                stream.Read(bb, 0, 100);

                string connect = ascEn.GetString(bb);

                if (!String.IsNullOrEmpty(connect))
                {
                    //authenticating to smsServer
                    string str = String.Format("action: login\r\nusername: {0}\r\nsecret: {1}\r\n\r\n", Properties.Settings.Default.SmsUsername, Properties.Settings.Default.SmsPassword);

                    byte[] ba = ascEn.GetBytes(str);
                    stream.Write(ba, 0, ba.Length);
                    stream.Flush();

                    byte[] resp = new byte[100];
                    stream.Read(resp, 0, 100);
                    response = ascEn.GetString(resp);
                    stream.Read(resp, 0, 100);
                    message = ascEn.GetString(resp);

                    if (response.Contains("Success") && message.Contains("Authentication accepted"))
                    {
                        Console.WriteLine("Authenticated");
                        Log.Info("Authenticated");
                        stream.Flush();
                        connected = true;
                        return tcpClient;
                    }
                    else
                    {
                        Console.WriteLine("Credentials error! Can't Authenticate");
                        Log.Error("Credentials error! Can't Authenticate");
                        tcpClient.Close();
                        connected = false;
                        return tcpClient;
                    }
                }

                connected = false;
                return tcpClient;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Error opening connection with SMS Server", ex);
            }

            connected = false;
            return tcpClient;
        }

        protected static void CloseConnection(TcpClient client)
        {
            if (client == null)
            {
                return;
            }
            try
            {
                client.Close();
                Console.WriteLine("Connection Closed process terminated...");
            }
            catch (Exception ex)
            {
                Log.Error("Error closing connection", ex);
            }
        }


        protected static void SendSmsToClient(TcpClient client, string fromNumber, string toNumber, string smsBody)
        {
            string response = string.Empty;
            string message = string.Empty;
            string eventMsg = string.Empty;
            try
            {
                ASCIIEncoding asen = new ASCIIEncoding();
                Stream stm = client.GetStream();

                string smsSend = string.Format("action: smscommand\r\ncommand: gsm send sms {0} {1} \"{2}\" \r\n\r\n", fromNumber, toNumber, smsBody);

                byte[] smsCmd = asen.GetBytes(smsSend);

                stm.Write(smsCmd, 0, smsCmd.Length);
                stm.Flush();

                byte[] smsResp = new byte[1000];
                stm.Read(smsResp, 0, 1000);
                response = asen.GetString(smsResp);
                Log.Error("Response " + response);
                if (!String.IsNullOrEmpty(response))
                {
                    stm.Read(smsResp, 0, 1000);
                    message = asen.GetString(smsResp);
                    Log.Info("smsResp " + message);
                    if (!String.IsNullOrEmpty(message))
                    {
                        stm.Read(smsResp, 0, 1000);

                        eventMsg = asen.GetString(smsResp);
                        Log.Info("eventMsg " + eventMsg);
                        if (!String.IsNullOrEmpty(eventMsg))
                        {
                            String[] list = eventMsg.Split('\n');

                            foreach (string value in list)
                            {


                                if (value.StartsWith("--END"))
                                {
                                    stm.Flush();
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error on sending SMS: ", ex);
            }
        }


    }
}
