using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Web.Identity.Model
{
    public class TranslateHelper
    {
        public static string Priority(string priority = "")
        {
            switch (priority)
            {
                case "Larte":
                    return "I lartë";

                case "Mesem":
                    return "I mesëm";

                case "Ulet":
                    return "I ulët";

                default:
                    return priority;

            }
        }

        public static string Status(string status = "")
        {
            switch (status)
            {
                case "Active":
                    return "Aktive";

                case "MoreInfo":
                    return "Më shumë info";

                case "Resolved":
                    return "E zgjidhur";

                case "Closed":
                    return "E mbyllur";

                default:
                    return status;

            }
        }

        public static string PriorityLocal(string priority = "")
        {
            switch (priority)
            {
                case "High":
                    return "I lartë";

                case "Medium":
                    return "I mesëm";

                case "Low":
                    return "I ulët";

                default:
                    return priority;

            }
        }
    }
}
