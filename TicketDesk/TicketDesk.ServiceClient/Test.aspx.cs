using System;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.ServiceClient
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EmailHelper sendEmail = new EmailHelper();

            string kaDifekt = "Po";

            string body = "Nje kerkese e re u rregjistrua ne sistemin e ticket desk nga faqja www.arfanet.al<br/><br/>" +
                "Emri: " + "<strong>Eduart</strong>" + "<br/>Email: " + "fff" + "<br/>Tel: " + "fff" +
                "<br/>Adresa: " + "gggg" + "<br/>Tipi kerkeses: " + ";;;;;;;;" + "<br/>Kategoria: " + "ggggggg" +
                "<br/>Emri kerkeses: " + "fffffffff" + "<br/>Priotiteti: " + "ggg" + "<br/>Ka difekt: " + kaDifekt +
                "<br/>Detajet e kerkeses: " + "gdmbmxbmb";

            sendEmail.SendEmail("eduart.lushka@pragmatic.al", "Nje kerkese e re", body);
        }
    }
}