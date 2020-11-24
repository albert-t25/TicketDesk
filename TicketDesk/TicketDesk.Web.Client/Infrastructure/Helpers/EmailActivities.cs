using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketDesk.Web.Client.Infrastructure.Helpers
{
    /// <summary>
    /// List of ticket activities that we should send an email to technical
    /// </summary>
    public enum EmailActivities
    {
        Assign = 0,
        ReAssign = 1,
        Pass = 2,
        TakeOver = 3
    }
}