using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace CTL.Models
{
    public class SupportTicket
    {

        public int SupportTicketID { get; set; }
        [Display(Name = "Email")]
        public string SupportTicketEmail { get; set; }
        [Display(Name = "Subject")]
        public string SupportTicketTitle { get; set; }
        [Display(Name = "Message"),UIHint("tinymce_jquery_full"), AllowHtml]
        public string SupportTicketContent { get; set; }
        public bool Active { get; set; }



    }
}