using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class ContentViewModel
    {

        public int ContentID { get; set; }
        public int? ContentTypeID { get; set; }
        public string ContentTitle { get; set; }
        public string ContentBody { get; set; }
        public string ContentFooter { get; set; }
        public int? SubContentCount { get; set; }
        public bool Active { get; set; }
        public bool IsMainPage { get; set; }
        public bool IsSecure { get; set; }
        public int? pageOrder { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }



    }
}