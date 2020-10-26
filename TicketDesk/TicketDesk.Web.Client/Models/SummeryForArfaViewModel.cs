using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Models
{
    public class SummeryForArfaViewModel
    {
        /// <summary>
        /// List of tickets
        /// </summary>
        public IList<Ticket> TicketList { get; set; }
        /// <summary>
        /// Select list of all projects
        /// </summary>
        public SelectList ProjectList { get; set; }
        /// <summary>
        /// From Date
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// To Date
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Ticket client
        /// </summary>
        public int? Client { get; set; }
    }
}