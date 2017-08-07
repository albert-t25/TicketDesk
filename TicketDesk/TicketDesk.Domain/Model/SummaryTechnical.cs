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
    public class SummaryTechnical
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

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation_sq))]

        [Display(ResourceType = typeof(Strings_sq), Name = "WorkingDays", ShortName = "WorkingDaysShort")]
        public int TotalWorkingDays { get; set; }


        [Display(Name = "Numri i kerkesave")]
        public int TicketsNumber { get; set; }
        [Display(Name = "Data")]
        public DateTimeOffset LastWorkDate { get; set; }
        [Display(Name = "Klienti ku ka punuar")]
        public string LastOwner { get; set; }
        public bool WithSupport { get; set; }
        public bool WithPersonalAuto { get; set; }
        public bool WithArfaNetAuto { get; set; }
    }
}
