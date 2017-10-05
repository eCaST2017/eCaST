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
    public class Message
    {

        public int MessageID { get; set; }
        [Display(Name = "Email")]
        public string MessageEmail { get; set; }
        [Display(Name = "Subject")]
        public string MessageTitle { get; set; }
        [Display(Name = "Message"), UIHint("tinymce_jquery_full"), AllowHtml]
        public string MessageContent { get; set; }
        public bool Active { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }

    }
}