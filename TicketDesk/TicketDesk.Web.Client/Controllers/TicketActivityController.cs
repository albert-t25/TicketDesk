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

using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.IO;
using TicketDesk.Localization.Controllers;
using TicketDesk.Web.Client.Models;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;
using TicketDesk.Web.Identity.Properties;
using System.Net;
using System.Net.Mail;
using log4net;
using TicketDesk.Web.Client.Infrastructure.Helpers;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("ticket-activity")]
    [Route("{action}")]
    [TdAuthorize(Roles = "TdInternalUsers,TdHelpDeskUsers,TdAdministrators")]
    [ValidateInput(false)]
    public class TicketActivityController : BaseController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TicketActivityController));

        private TdDomainContext Context { get; set; }
        public TicketActivityController(TdDomainContext context)
        {
            Context = context;
        }

        [Route("load-activity")]
        public async Task<ActionResult> LoadActivity(TicketActivity activity, int ticketId, Guid? tempId)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            Context.TicketActions.IsTicketActivityValid(ticket, activity);
            ViewBag.CommentRequired = activity.IsCommentRequired();
            ViewBag.Activity = activity;
            ViewBag.TempId = tempId ?? Guid.NewGuid();
            ViewBag.IsEditorDefaultHtml = Context.TicketDeskSettings.ClientSettings.GetDefaultTextEditorType() == "summernote";
            if (activity == TicketActivity.EditTicketInfo)
            {
                await SetProjectInfoForModelAsync(ticket);
            }
            return PartialView("_ActivityForm", ticket);
        }

        private async Task SetProjectInfoForModelAsync(Ticket ticket)
        {
            var projectCount = await Context.Projects.CountAsync();
            var isMulti = (projectCount > 1);
            ViewBag.IsMultiProject = isMulti;
        }

        [Route("activity-buttons")]
        public ActionResult ActivityButtons(int ticketId)
        {
            //WARNING! This is also used as a child action and cannot be made async in MVC 5
            var ticket = Context.Tickets.Find(ticketId);
            var activities = Context.TicketActions.GetValidTicketActivities(ticket);
            return PartialView("_ActivityButtons", activities);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add-comment")]
        public async Task<ActionResult> AddComment(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment)
        {
            var activityFn = Context.TicketActions.AddComment(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.AddComment, comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("assign")]
        public async Task<ActionResult> Assign(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment, string assignedTo, string priority)
        {
            var activityFn = Context.TicketActions.Assign(comment, assignedTo, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.Assign, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("cancel-more-info")]
        public async Task<ActionResult> CancelMoreInfo(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment)
        {
            var activityFn = Context.TicketActions.CancelMoreInfo(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.CancelMoreInfo, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("close")]
        public async Task<ActionResult> Close(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment)
        {
            var activityFn = Context.TicketActions.Close(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.Close, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit-ticket-info")]
        public async Task<ActionResult> EditTicketInfo(
            int ticketId,
            int projectId,
            [ModelBinder(typeof(SummernoteModelBinder))] string comment,
            string title,
            string details,
            string priority,
            string ticketType,
            string category,
            string owner,
            string tagList,
            bool affectsCustomer,
            bool onlineSupport)
        {
            details = details.StripHtmlWhenEmpty();
            var projectName = await Context.Projects.Where(p => p.ProjectId == projectId).Select(s => s.ProjectName).FirstOrDefaultAsync();
            var activityFn = Context.TicketActions.EditTicketInfo(comment, projectId, projectName, title, details, priority, ticketType, category, owner, tagList, Context.TicketDeskSettings, affectsCustomer, onlineSupport);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.EditTicketInfo, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("force-close")]
        public async Task<ActionResult> ForceClose(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment)
        {
            var activityFn = Context.TicketActions.ForceClose(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ForceClose, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("give-up")]
        public async Task<ActionResult> GiveUp(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment)
        {
            var activityFn = Context.TicketActions.GiveUp(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.GiveUp, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("modify-attachments")]
        public async Task<ActionResult> ModifyAttachments(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment, Guid tempId, string deleteFiles)
        {
            var demoMode = (ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"] ?? "false").Equals("true", StringComparison.InvariantCultureIgnoreCase);
            if (demoMode)
            {
                return new EmptyResult();
            }
            //most of this action is performed directly against the storage provider, outside the business domain's control. 
            //  All the business domain has to do is record the activity log and comments
            Action<Ticket> activityFn = ticket =>
           {
               //TODO: it might make sense to move the string building part of this over to the TicketDeskFileStore too?
               var sb = new StringBuilder(comment);
               if (!string.IsNullOrEmpty(deleteFiles))
               {
                   sb.AppendLine();
                   sb.AppendLine("<dl><dt>");
                   sb.AppendLine(Strings_sq.RemovedFiles);
                   sb.AppendLine("</dt>");


                   var files = deleteFiles.Split(',');
                   foreach (var file in files)
                   {
                       TicketDeskFileStore.DeleteAttachment(file, ticketId.ToString(CultureInfo.InvariantCulture), false);
                       sb.AppendLine(string.Format("<dd>    {0}</dd>", file));
                   }
                   sb.AppendLine("</dl>");
               }
               var filesAdded = ticket.CommitPendingAttachments(tempId).ToArray();
               if (filesAdded.Any())
               {
                   sb.AppendLine();
                   sb.AppendLine("<dl><dt>");
                   sb.AppendLine(Strings_sq.NewFiles);
                   sb.AppendLine("</dt>");

                   foreach (var file in filesAdded)
                   {
                       sb.AppendLine(string.Format("<dd>    {0}</dd>", file));
                   }
                   sb.AppendLine("</dl>");
               }
               comment = sb.ToString();

               //perform the simple business domain functions
               var domainActivityFn = Context.TicketActions.ModifyAttachments(comment);
               domainActivityFn(ticket);
           };

            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ModifyAttachments, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("pass")]
        public async Task<ActionResult> Pass(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment, string assignedTo, string priority)
        {
            var activityFn = Context.TicketActions.Pass(comment, assignedTo, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.Pass, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("reassign")]
        public async Task<ActionResult> ReAssign(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment, string assignedTo, string priority)
        {
            var activityFn = Context.TicketActions.ReAssign(comment, assignedTo, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ReAssign, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("request-more-info")]
        public async Task<ActionResult> RequestMoreInfo(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment)
        {
            var activityFn = Context.TicketActions.RequestMoreInfo(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.RequestMoreInfo, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("reopen")]
        public async Task<ActionResult> ReOpen(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment, bool assignToMe = false)
        {
            var activityFn = Context.TicketActions.ReOpen(comment, assignToMe);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ReOpen, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("resolve")]
        public async Task<ActionResult> Resolve(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment)
        {
            var activityFn = Context.TicketActions.Resolve(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ReOpen, comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("supply-more-info")]
        public async Task<ActionResult> SupplyMoreInfo(int ticketId, [ModelBinder(typeof(SummernoteModelBinder))] string comment, bool reactivate = false)
        {
            var activityFn = Context.TicketActions.SupplyMoreInfo(comment, reactivate);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.SupplyMoreInfo, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("take-over")]
        public async Task<ActionResult> TakeOver(
            int ticketId,
            [ModelBinder(typeof(SummernoteModelBinder))] string comment,
            string priority)
        {
            var activityFn = Context.TicketActions.TakeOver(comment, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.TakeOver, "");
        }


        private async Task<ActionResult> PerformTicketAction(int ticketId, Action<Ticket> activityFn, TicketActivity activity, string comment)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            TryValidateModel(ticket);
            if (ModelState.IsValid)
            {
                try
                {
                    ticket.PerformAction(activityFn);
                    //send email
                    SendEmail(ticket:ticket, activity:activity, comment:comment);
                }
                catch (SecurityException ex)
                {
                    Log.Error("Could not perform ticket activity!", ex);
                    ModelState.AddModelError("Security", ex);
                }
                var result = await Context.SaveChangesAsync(); //save changes catches lastupdatedby and date automatically
                if (result > 0)
                {
                    return new EmptyResult();//standard success case
                }
            }
            //fail case, return the view and let the client/view sort out the errors
            ViewBag.CommentRequired = activity.IsCommentRequired();
            ViewBag.Activity = activity;
            ViewBag.IsEditorDefaultHtml = Context.TicketDeskSettings.ClientSettings.GetDefaultTextEditorType() == "summernote";
            return PartialView("_ActivityForm", ticket);
        }

        /// <summary>
        /// Send email based in the activity
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="activity"></param>
        /// <param name="comment"></param>
        private void SendEmail(Ticket ticket, TicketActivity activity, string comment)
        {
            if (activity.ToString().ToLower() == "Resolved".ToLower())
            {
                //ER: add logic to send email to client when a ticket is resolved
                PrepareEmailForResolved(ticket, comment);
            }

            if (activity.ToString().ToLower() == "AddComment".ToLower())
            {
                //send email if when a new comment is added
                PrepareEmailAddComment(ticket, comment);
            }

            var activities = Enum.GetNames(typeof(EmailActivities)).ToList();
            if (activities.Contains(activity.ToString()) && ticket.IsAssigned)
            {
                Log.Info($"Sending email to technician. Comment {comment}");

                //send email to the person that the ticket is assigned
                UserDisplayInfo userInfo = ticket.GetAssignedToInfo();

                var root = Context.TicketDeskSettings.ClientSettings.GetDefaultSiteRootUrl();
                //ticket.Priority = TranslateHelper.Priority(ticket.Priority);
                string body = this.RenderViewToString(ControllerContext, "~/Views/Emails/Ticket.Html.cshtml", new TicketEmail()
                {
                    Ticket = ticket,
                    SiteRootUrl = root,
                    IsMultiProject = false
                });
                //send email
                try
                {
                    EmailHelper.SendEmail(userInfo.Email, "Një detyrë e re për ju.", body);
                }
                catch (Exception ex)
                {
                    Log.Error($"Could not send email to technician!{ex}");
                }
                //send sms
                if (!string.IsNullOrWhiteSpace(userInfo.Phone))
                {
                    try
                    {
                        var projectName = "";
                        var project = Context.Projects.Find(ticket.ProjectId);
                        if (project != null)
                            projectName = project.ProjectName;

                        //send sms to the technician that the ticket is assigned to
                        SmsHelper sendSms = new SmsHelper();
                        sendSms.SendSms(userInfo.Phone, projectName);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Could not send sms to technician! {ex}");
                    }

                }
                else
                {
                    Log.Info("Could not send sms to technician! Technician has no phone number!");
                }
                
            }
        }

        /// <summary>
        /// Sends mail to ArfaNet when a new comment is added to an existing ticket. 
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="comment"></param>
        private void PrepareEmailAddComment(Ticket ticket, string comment)
        {
            if (!String.IsNullOrWhiteSpace(comment))
            {
                Log.Info($"Sending email to Arfa manager. Comment {comment}");

                var user = ticket.GetLastUpdatedByInfo();
                string body = "Përshëndetje,"
                       + "<br/>Një koment është shtuar në kërkesen:  \"<b>" + ticket.Title + "</b>\""
                       + "<br/><br/>Përmbajtja e komentit: <br/>" + HtmlHelperExtensions.HtmlToPlainText(comment).Trim()
                       + "<br/><br/>Komenti u shtua nga: " + user.DisplayName + "(" + user.Email + ")";

                //send mail to Arfa
                try
                {
                    EmailHelper.SendEmail("Koment i ri në kërkesen: \"" + ticket.Title + "\"", body);
                }

                catch (Exception ex)
                {
                    Log.Error("Could not send email to Arfa manager!", ex);
                }
            }

        }

        /// <summary>
        /// Sends mail to Client when ticket is marked as Resolved.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="comment"></param>
        private void PrepareEmailForResolved(Ticket ticket, string comment)
        {
            if (!String.IsNullOrWhiteSpace(ticket.Project.Email))
            {
                Log.Info($"Sending email to client {ticket.Project.Email}");

                var assignedToInfo = ticket.GetAssignedToInfo();
                var support = ticket.OnlineSupport ? "Online" : "Hardware Support";
                var body = "I nderuar Klient."
                               + "<br/>Kërkesa e krijuar nga ju për " + "<b>" + ticket.Project.ProjectName + "</b>" + " është mbyllur."
                               + "<br/><br/>Subjekti: " + ticket.Title
                               + "<br/>Përshkrimi i problemit: " + HtmlHelperExtensions.HtmlToPlainText(ticket.Details)
                               + "<br/><br/>Specialisti që asistoi: " + assignedToInfo.DisplayName + "(" + assignedToInfo.Email + ")"
                               + "<br/>Lloji i asistencës: " + support.ToString()
                               + "<br/>Përshkrimi i shërbimit të kryer: " + HtmlHelperExtensions.HtmlToPlainText(comment).Trim();

                //send mail to client
                try
                {
                    EmailHelper.SendEmail(ticket.Project.Email, "Kërkesa juaj: " + ticket.Title + " është zgjidhur.", body);
                }

                catch (Exception ex)
                {
                    Log.Error($"Could not send email to client {ticket.Project.Email}", ex);
                }
            }

        }
    }
}