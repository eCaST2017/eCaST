using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class NoticeViewModel
    {

        public int NoticeID { get; set; }
        public string NoticeName { get; set; }
        public string NoticeDescription { get; set; }
        public bool Active { get; set; }
        public int ProgramID { get; set; }
        public int NoticeTypeBinID { get; set; }
        public bool Default { get; set; }
        public bool Expires { get; set; }
        public DateTime? ExpirationDate { get; set; }
     

        public string NoticeTypeBinName { get; set; }
        public string Icon { get; set; }
        public string Alert { get; set; }


        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }





    }
}