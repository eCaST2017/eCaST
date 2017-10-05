using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class Notice
    {

        public int NoticeID { get; set; }
        public string NoticeName { get; set; }
        public bool Active { get; set; }
        public string NoticeDescription { get; set; }
        public Nullable<int> ProgramID { get; set; }
        public Nullable<int> NoticeTypeBinID { get; set; }
        public bool Default { get; set; }
        public bool Expires { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        


    }
}