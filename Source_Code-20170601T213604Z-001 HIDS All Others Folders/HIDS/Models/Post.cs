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
    public class Post
    {

        public int PostID { get; set; }
        public int? PostTypeID { get; set; }
        public string PostTitle { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string PostContent { get; set; }
        public bool Active { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? OpportunityOpenDate { get; set; }
        public DateTime? OpportunityExpirationDate { get; set; }
        public string AwardedId { get; set; }
        public string OutsideAwardedName { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }


    }
}