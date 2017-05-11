using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketDesk.Localization;
using TicketDesk.Localization.Domain;

namespace TicketDesk.Domain.Model
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [StringLength(100, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation_sq))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation_sq))]
        [Display(Name = "Project_Name", ResourceType = typeof(Strings_sq))]
        public string ProjectName { get; set; }

        [StringLength(500, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation_sq))]
        [Display(Name = "Project_Description", ResourceType = typeof(Strings_sq))]
        public string ProjectDescription { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Strings_sq))]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone", ResourceType = typeof(Strings_sq))]
        public string Phone { get; set; }

        [StringLength(100, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [Display(Name = "Address", ResourceType = typeof(Strings_sq))]
        public string Address { get; set; }
        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
