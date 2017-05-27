using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Localization;
using TicketDesk.Localization.Controllers;

namespace TicketDesk.Domain.Model
{
    public class SummaryTicket
    {
        private string _assignedTo;
        [StringLength(256, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation_sq))]
        [Display(ResourceType = typeof(Strings_sq), Name = "TicketAssignedTo", ShortName = "TicketAssignedToShort")]
        public string AssignedTo
        {
            get
            {
                return _assignedTo;
            }
            set
            {

                _assignedTo = value;
            }
        }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation_sq))]

        [Display(ResourceType = typeof(Strings_sq), Name = "WorkingHours", ShortName = "WorkingHoursShort")]
        public int TotalWorkingHours { get; set; }


        [Display(Name = "Numri i kerkesave")]
        public int TicketsNumber { get; set; }

        [Display(Name = "Statusi i kerkesave")]
        public string Status { get; set; }

    }
}
