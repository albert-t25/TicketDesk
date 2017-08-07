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

using System.ComponentModel.DataAnnotations;
using TicketDesk.Localization;
using TicketDesk.Localization.Models;

namespace TicketDesk.Web.Client.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation_sq))]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Validation_sq))]
        [Display(Name = "Email", ResourceType = typeof(Strings_sq))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation_sq))]
        [StringLength(100, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation_sq))]
        [Display(Name = "DisplayName", ResourceType = typeof(Strings_sq))]
        public string DisplayName { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation_sq))]
        [StringLength(100, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation_sq))]
        [Display(Name = "Phone", ResourceType = typeof(Strings_sq))]
        public string Phone { get; set; }
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation_sq))]
        [StringLength(100, ErrorMessageResourceName = "FieldMinimumLength", ErrorMessageResourceType = typeof(Validation_sq), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Strings_sq))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Strings_sq))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmationDoNotMatch", ErrorMessageResourceType = typeof(Strings_sq))]
        public string ConfirmPassword { get; set; }
    }

    public class UserSignInViewModel
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation_sq))]
        [Display(Name = "Email", ResourceType = typeof(Strings_sq))]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Validation_sq))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation_sq))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Strings_sq))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Strings_sq))]
        public bool RememberMe { get; set; }
    }
}