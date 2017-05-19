using System;

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

            if (result)
            {
                this.lblMessage.Text = "Kerkesa u rregjistrua me sukses!";
                this.lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                this.lblMessage.Text = "Ndodhi nje gabim gjate dergimit te kerkeses, ju lutem provoni perseri me vone.";
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}