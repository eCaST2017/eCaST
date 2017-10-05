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
    public class Content
    {

        public int ContentID { get; set; }
        public int? ContentTypeID { get; set; }
        [Display(Name = "Title")]
        public string ContentTitle { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        [Display(Name = "Main Content")]
        public string ContentBody { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string ContentFooter { get; set; }
        public bool Active { get; set; }
        public bool IsMainPage { get; set; }
        public bool IsSecure { get; set; }
        public int? pageOrder { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }

        public virtual ICollection<ContentList> ContentLists { get; set; }
        //public virtual ICollection<Asset> Assets { get; set; }
    }
}