using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TicketDesk.Domain;
using TicketDesk.Web.Client.Controllers;
using TicketDesk.Web.Client.Models;

namespace ReportConsole
{
    class Program
    {
        static void Main()
        {
            //get all users
            var users = GetUsers();

            TdDomainContext context = new TdDomainContext();
            TicketCenterController ticketCenterController = new TicketCenterController(context);

            //send monthly report to ArfaNet
            ticketCenterController.SendMonthlyReportToArfaNet(users);
            //send monthly report to ArfaNet clients
            //ticketCenterController.SendMonthlyReportToArfaNetClients(users);
        }

        /// <summary>
        /// Get users in console application directly from database since we can not access users without initializing the start page
        /// </summary>
        /// <returns>Users list</returns>
        public static List<ReportConsoleUserModel> GetUsers()
        {
            List<ReportConsoleUserModel> userModels = new List<ReportConsoleUserModel>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TicketDesk"].ConnectionString;
                conn.Open();

                // use the connection here
                SqlCommand command = new SqlCommand("SELECT * FROM IdentityUsers", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        //Added this for testing purposes / write the data on to the screen
                        Console.WriteLine(String.Format("{0} \t | {1} ",
                            // call the objects from their index
                            reader[0], reader[1]));

                        ReportConsoleUserModel user = new ReportConsoleUserModel()
                        {
                            UserId = reader[0].ToString(),
                            DisplayName = reader[1].ToString()
                        };
                        userModels.Add(user);
                    }
                }
                conn.Close();
            }
            
            return userModels;
        }
    }
}
