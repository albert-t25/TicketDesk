// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Web.Client.Models;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;
using ClosedXML.Excel;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("tickets")]
    [Route("{action=index}")]
    [TdAuthorize(Roles = "TdInternalUsers,TdHelpDeskUsers,TdAdministrators")]
    public class TicketCenterController : Controller
    {
        private TdDomainContext Context { get; set; }
        public TicketCenterController(TdDomainContext context)
        {
            Context = context;
        }
        private string getUserName(string id)
        {
            var context = new TdIdentityContext();
            foreach (var user in context.Users)
            {
                if (user.Id == id)
                    return user.Email + "--" + user.DisplayName;
            }
            return "noemail";
        }
        [Route("reset-user-lists")]
        public async Task<ActionResult> ResetUserLists()
        {
            var uId = Context.SecurityProvider.CurrentUserId;
            await Context.UserSettingsManager.ResetAllListSettingsForUserAsync(uId);
            var x = await Context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        // GET: TicketCenter
        [Route("{listName?}/{page:int?}")]
        public async Task<ActionResult> Index(int? page, string listName)
        {
            listName = listName ?? (Context.SecurityProvider.IsTdHelpDeskUser ? "unassigned" : "mytickets");
            var pageNumber = page ?? 1;

            var viewModel = await TicketCenterListViewModel.GetViewModelAsync(pageNumber, listName, Context, Context.SecurityProvider.CurrentUserId);//new TicketCenterListViewModel(listName, model, Context, User.Identity.GetUserId());

            return View(viewModel);
        }

        private List<IGrouping<string, Ticket>> getTicketForReport(string filters)
        {
            List<IGrouping<string, Ticket>> tickets = new List<IGrouping<string, Ticket>>();
            if (filters != null)
            {
                string[] formats = { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy",
                    "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy"};
                DateTime dateValue1;
                DateTime dateValue2;
                string[] param = filters.Split(';');
                string filter = param[0];
                if (DateTime.TryParse(param[1], out dateValue1) && DateTime.TryParse(param[2],
                              out dateValue2))
                {
                    DateTime dt1 = Convert.ToDateTime(param[1]);
                    DateTime dt2 = Convert.ToDateTime(param[2]);
                    DateTimeOffset from = dt1;
                    DateTimeOffset to = dt2;

                    tickets = Context.Tickets.ToList().Where(i => i.TicketStatus.ToString() == filter && i.CreatedDate > from && i.CreatedDate < to).GroupBy(i => i.AssignedTo).ToList();
                }
                else
                {
                    tickets = Context.Tickets.ToList().Where(i => i.TicketStatus.ToString() == filter).GroupBy(i => i.AssignedTo).ToList();
                }
            }

            else
            {
                tickets = Context.Tickets.ToList().GroupBy(i => i.AssignedTo).ToList();

            }
            return tickets;
        }
        // GET: TicketCenter
        // [Route("{listName?}/{page:int?}")]
        public ActionResult Summary(int? page, string listName, string filters)//string from, string to)
        {
            
          
            List<IGrouping<string, Ticket>> tickets = getTicketForReport(filters);
           
            List<SummaryTicket> model = new List<SummaryTicket>();
            foreach (var group in tickets)
            {
                SummaryTicket summaryTicket = new SummaryTicket();
                summaryTicket.TotalWorkingHours = 0;
                summaryTicket.TicketsNumber = 0;
                foreach (Ticket item in group)
                {
                    summaryTicket.AssignedTo = item.GetAssignedToInfo().DisplayName;
                    summaryTicket.TotalWorkingHours += item.WorkingHours;
                    summaryTicket.TicketsNumber++;
                    summaryTicket.Status = item.TicketStatus.ToString();
                }
                model.Add(summaryTicket);
            }
            if (Request.IsAjaxRequest())
            {

                return PartialView("SummaryTicketList", model);
            }
                return View(model);
        }
        private List<SummaryTechnical> getSummaryTechnicalModel(List<IGrouping<string, Ticket>> tickets)
        {
            List<SummaryTechnical> model = new List<SummaryTechnical>();
            foreach (var group in tickets)
            {
                SummaryTechnical summaryTicket = new SummaryTechnical();
                summaryTicket.TotalWorkingHours = 0;
                summaryTicket.TotalWorkingDays = 0;

                summaryTicket.TicketsNumber = 0;
                foreach (Ticket item in group)
                {
                    summaryTicket.AssignedTo = item.GetAssignedToInfo().DisplayName;
                    summaryTicket.TotalWorkingHours += item.WorkingHours;
                    summaryTicket.TotalWorkingDays += item.WorkingDays;

                    summaryTicket.LastOwner = getUserName(item.Owner.ToString());
                    summaryTicket.LastWorkDate = item.LastUpdateDate;
                    summaryTicket.WithSupport = item.WithSupport;
                    summaryTicket.WithPersonalAuto = item.WithPersonalAuto;
                    summaryTicket.WithArfaNetAuto = item.WithArfaNetAuto;
                    summaryTicket.TicketsNumber++;

                }
                model.Add(summaryTicket);
            }
            return model;
        }
        public ActionResult SummaryForTechnicals() { 


            List<IGrouping<string, Ticket>> tickets = getTicketForReport(null);



            List<SummaryTechnical> model = getSummaryTechnicalModel(tickets);
           
            if (Request.IsAjaxRequest())
            {

                return PartialView("SummaryTechnicalList", model);
            }
            return View(model);
        }
        private void Download(string fileName)
        {
            try
            {
                Response.Clear();

                Response.ClearHeaders();

                Response.ClearContent();
                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

                Response.Flush();

                Response.TransmitFile(Server.MapPath("~/upload/" + fileName));

                Response.End();
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
        }
        public ActionResult ExcelReportTechnical()
        {

            List<IGrouping<string, Ticket>> tickets = getTicketForReport(null);



            List<SummaryTechnical> model = getSummaryTechnicalModel(tickets);

            string fileName = $"{Guid.NewGuid().ToString()}.xlsx";
            string filePath = $"~/upload/{fileName}";

            XLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("Raporti_per_teknikun_e_jashtem");
            int index = 2; ;
            int positionOfCol = 1;
            foreach (SummaryTechnical item in model)
            {
                ws.Cell(index, positionOfCol).Value = item.TicketsNumber.ToString();
                positionOfCol++;
                ws.Cell(index, positionOfCol).Value = item.LastWorkDate.ToString();
                positionOfCol++;
                ws.Cell(index, positionOfCol).Value = item.AssignedTo==null?"Asnje": item.AssignedTo;
                positionOfCol++;
                ws.Cell(index, positionOfCol).Value = item.LastOwner.ToString();
                positionOfCol++;
                ws.Cell(index, positionOfCol).Value = item.TotalWorkingDays;
                positionOfCol++;
                ws.Cell(index, positionOfCol).Value = item.TotalWorkingHours;
                positionOfCol++;
                ws.Cell(index, positionOfCol).Value = item.WithSupport == true ? "Po" : "Jo";
                positionOfCol++;
                ws.Cell(index, positionOfCol).Value = item.WithPersonalAuto == true ? "Po" : "Jo";
                positionOfCol++;
                ws.Cell(index, positionOfCol).Value = item.WithArfaNetAuto == true ? "Po" : "Jo";
                positionOfCol++;
                positionOfCol = 1;
                index++;

            }
            int j = 1;
            ws.Cell(1, j).Value = "Nr";
            ws.Cell(1, j).Style.Font.Bold = true;
            j++;
            ws.Cell(1, j).Value = "Data";
            ws.Cell(1, j).Style.Font.Bold = true;
            j++;
            ws.Cell(1, j).Value = "Tekniku";
            ws.Cell(1, j).Style.Font.Bold = true;
            j++;
            ws.Cell(1, j).Value = "Klienti ku ka punuar";
            ws.Cell(1, j).Style.Font.Bold = true;
            j++;
            ws.Cell(1, j).Value = "Dite pune";
            ws.Cell(1, j).Style.Font.Bold = true;
            j++;
            ws.Cell(1, j).Value = "Ore pune";
            ws.Cell(1, j).Style.Font.Bold = true;
            j++;
            ws.Cell(1, j).Value = "Me ndihmes-teknik ose jo";
            ws.Cell(1, j).Style.Font.Bold = true;

            j++;
            ws.Cell(1, j).Value = "Me mjet personal";
            ws.Cell(1, j).Style.Font.Bold = true;
            j++;
            ws.Cell(1, j).Value = "Me mjetin e ArfaNet";
            ws.Cell(1, j).Style.Font.Bold = true;
            wb.SaveAs(Server.MapPath(filePath));
            Download(fileName);
            return null;

        }
        [Route("pageList/{listName=mytickets}/{page:int?}")]
        public async Task<ActionResult> PageList(int? page, string listName)
        {
            return await GetTicketListPartial(page, listName);
        }


        [Route("filterList/{listName=opentickets}/{page:int?}")]
        public async Task<PartialViewResult> FilterList(
            string listName,
            int pageSize,
            string ticketStatus,
            string owner,
            string assignedTo)
        {
            var uId = Context.SecurityProvider.CurrentUserId;
            var userSetting = await Context.UserSettingsManager.GetSettingsForUserAsync(uId);

            var currentListSetting = userSetting.GetUserListSettingByName(listName);

            currentListSetting.ModifyFilterSettings(pageSize, ticketStatus, owner, assignedTo);
            
            await Context.SaveChangesAsync();

            return await GetTicketListPartial(null, listName);

        }

        [Route("sortList/{listName=opentickets}/{page:int?}")]
        public async Task<PartialViewResult> SortList(
            int? page,
            string listName,
            string columnName,
            bool isMultiSort = false)
        {
            var uId = Context.SecurityProvider.CurrentUserId;
            var userSetting = await Context.UserSettingsManager.GetSettingsForUserAsync(uId);
            var currentListSetting = userSetting.GetUserListSettingByName(listName);

            var sortCol = currentListSetting.SortColumns.SingleOrDefault(sc => sc.ColumnName == columnName);

            if (isMultiSort)
            {
                if (sortCol != null)// column already in sort, remove from sort
                {
                    if (currentListSetting.SortColumns.Count > 1)//only remove if there are more than one sort
                    {
                        currentListSetting.SortColumns.Remove(sortCol);
                    }
                }
                else// column not in sort, add to sort
                {
                    currentListSetting.SortColumns.Add(new UserTicketListSortColumn(columnName, ColumnSortDirection.Ascending));
                }
            }
            else
            {
                if (sortCol != null)// column already in sort, just flip direction
                {
                    sortCol.SortDirection = (sortCol.SortDirection == ColumnSortDirection.Ascending) ? ColumnSortDirection.Descending : ColumnSortDirection.Ascending;
                }
                else // column not in sort, replace sort with new simple sort for column
                {
                    currentListSetting.SortColumns.Clear();
                    currentListSetting.SortColumns.Add(new UserTicketListSortColumn(columnName, ColumnSortDirection.Ascending));
                }
            }

            await Context.SaveChangesAsync();

            return await GetTicketListPartial(page, listName);
        }



        private async Task<PartialViewResult> GetTicketListPartial(int? page, string listName)
        {
            var pageNumber = page ?? 1;

            var viewModel = await TicketCenterListViewModel.GetViewModelAsync(pageNumber, listName, Context, Context.SecurityProvider.CurrentUserId);
            return PartialView("_TicketList", viewModel);

        }

       
    }
}
