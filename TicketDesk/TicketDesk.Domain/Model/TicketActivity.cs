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

using System;
using System.ComponentModel.DataAnnotations;
using TicketDesk.Domain.Infrastructure;
using TicketDesk.Localization.Domain;

namespace TicketDesk.Domain.Model
{

    [Flags]
    public enum TicketActivity
    {
        None = 0,

        [CommentRequired]
        [Display(Name = "TicketActivity_Comment", ResourceType = typeof(Strings_sq))]
        AddComment = 1,

        [CommentRequired]
        [Display(Name = "TicketActivity_ProvideInfo", ResourceType = typeof(Strings_sq))]
        SupplyMoreInfo = 2,

        [Display(Name = "TicketActivity_CancelMoreInfo", ResourceType = typeof(Strings_sq))]
        CancelMoreInfo = 4,

        [CommentRequired]
        [Display(Name = "TicketActivity_RequestMoreInfo", ResourceType = typeof(Strings_sq))]
        RequestMoreInfo = 8,

        [Display(Name = "TicketActivity_TakeOver", ResourceType = typeof(Strings_sq))]
        TakeOver = 16,

        [CommentRequired]
        [Display(Name = "TicketActivity_Resolve", ResourceType = typeof(Strings_sq))]
        Resolve = 32,

        [Display(Name = "TicketActivity_Assign", ResourceType = typeof(Strings_sq))]
        Assign = 64,

        [Display(Name = "TicketActivity_ReAssign", ResourceType = typeof(Strings_sq))]
        ReAssign = 128,

        [Display(Name = "TicketActivity_Pass", ResourceType = typeof(Strings_sq))]
        Pass = 256,

        [Display(Name = "TicketActivity_Close", ResourceType = typeof(Strings_sq))]
        Close = 512,

        [CommentRequired]
        [Display(Name = "TicketActivity_ReOpen", ResourceType = typeof(Strings_sq))]
        ReOpen = 1024,

        [Display(Name = "TicketActivity_GiveUp", ResourceType = typeof(Strings_sq))]
        [CommentRequired]
        GiveUp = 2048,

        [CommentRequired]
        [Display(Name = "TicketActivity_ForceClose", ResourceType = typeof(Strings_sq))]
        ForceClose = 4096,

        [Display(Name = "TicketActivity_EditAttachments", ResourceType = typeof(Strings_sq))]
        ModifyAttachments = 8192,

        [Display(Name = "TicketActivity_Edit", ResourceType = typeof(Strings_sq))]
        EditTicketInfo = 16384,

        [Display(Name = "TicketActivity_Create", ResourceType = typeof(Strings_sq))]
        Create = 32768,

        [Display(Name = "TicketActivity_CreateOnBehalfOf", ResourceType = typeof(Strings_sq))]
        CreateOnBehalfOf = 65536
    }


}
