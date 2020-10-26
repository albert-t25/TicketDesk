using System;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.ServiceClient
{
    public partial class SubmitTicket : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            Ticket.SubmitTicket submitTicket = new Ticket.SubmitTicket();

            bool result = submitTicket.AddNewTicket(txtFullName.Text, txtClientEmail.Text, txtClientPhone.Text, txtAddress.Text, ddlTicketType.SelectedValue, ddlCategory.SelectedValue, txtTicketName.Text, txtTicketDetails.Text, ddlPriority.SelectedValue, chkAffCos.Checked);

            this.FormPanel.Visible = false;
            this.ResponsePanel.Visible = true;
            if (result)
            {
                this.lblMessage.Text = "Kerkesa u rregjistrua me sukses!";
                this.lblMessage.ForeColor = System.Drawing.Color.Green;

                //send to email the new ticket from afranet.al
                EmailHelper sendEmail = new EmailHelper();

                string kaDifekt = chkAffCos.Checked ? "Po" : "Jo";

                string body = "Nje kerkese e re u rregjistrua ne sistemin e ticket desk nga faqja www.arfanet.al<br/>" +
                    "Emri: <strong>" + txtFullName.Text +
                    "</strong><br/>Email: <strong>" + txtClientEmail.Text +
                    "</strong><br/>Tel: <strong>" + txtClientPhone.Text +
                    "</strong><br/>Adresa: <strong>" + txtAddress.Text +
                    "</strong><br/>Tipi kerkeses: <strong>" + ddlTicketType.SelectedItem.Text +
                    "</strong><br/>Kategoria: <strong>" + ddlCategory.SelectedItem.Text +
                    "</strong><br/>Emri kerkeses: <strong>" + txtTicketName.Text +
                    "</strong><br/>Priotiteti: <strong>" + ddlPriority.SelectedItem.Text +
                    "</strong><br/>Ka difekt: <strong>" + kaDifekt +
                    "</strong><br/>Detajet e kerkeses: <strong>" + txtTicketDetails.Text + "</strong>";

                try
                {
                    EmailHelper.SendEmail(Properties.Settings.Default.ReceiverEmail, "Nje kerkese e re", body);
                }
                catch (Exception ex)
                {
                    //
                }
                
            }
            else
            {
                this.lblMessage.Text = "Ndodhi nje gabim gjate dergimit te kerkeses, ju lutem provoni perseri me vone.";
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}