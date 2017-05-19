using System;
using System.Web.Services;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace TicketDesk.WebService
{
    /// <summary>
    /// Summary description for SubmitTicket
    /// </summary>
    [WebService(Namespace = "http://pragmatic.al/webservices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SubmitTicket : System.Web.Services.WebService
    {
        [WebMethod]
        public bool AddNewTicket(string clientName, string clientEmail, string clientPhone, string clientAddress, string ticketType, string category, string title, string details, string priority, bool affectedCostumer)
        {
            TdDomainContext context = new TdDomainContext();
            int projectId = AddNewProject(clientName, clientEmail, clientPhone, clientAddress);

            Ticket newTicket = new Ticket()
            {
                TicketType = ticketType,
                Category = category,
                Title = title,
                Details = details,
                IsHtml = true,
                CreatedBy = "5e12f43a-c18e-4c8c-99be-e2ec714c6136",
                CreatedDate = DateTime.UtcNow,
                Owner = "5e12f43a-c18e-4c8c-99be-e2ec714c6136",
                AssignedTo = "5e12f43a-c18e-4c8c-99be-e2ec714c6136",
                TicketStatus = TicketStatus.Active,
                CurrentStatusDate = DateTime.UtcNow,
                CurrentStatusSetBy = "5e12f43a-c18e-4c8c-99be-e2ec714c6136",
                LastUpdateBy = "5e12f43a-c18e-4c8c-99be-e2ec714c6136",
                LastUpdateDate = DateTime.UtcNow,
                Priority = priority,
                AffectsCustomer = affectedCostumer,
                ProjectId = projectId,
                WorkingHours = 0
            };

            context.Tickets.Add(newTicket);
            context.SaveChanges();

            return true;
        }

        private int AddNewProject(string clientName, string clientEmail, string clientPhone, string clientAddress)
        {
            TdDomainContext context = new TdDomainContext();

            Project newTempProject = new Project()
            {
                ProjectName = clientName + "-" + " Klient temporar",
                ProjectDescription = "Ky eshte nje klient temporar i krijuar nga site i arfanet.al",
                Email = clientEmail,
                Phone = clientPhone,
                Address = clientAddress,
            };

            context.Projects.Add(newTempProject);
            context.SaveChanges();

            return newTempProject.ProjectId;
        }
    }
}
