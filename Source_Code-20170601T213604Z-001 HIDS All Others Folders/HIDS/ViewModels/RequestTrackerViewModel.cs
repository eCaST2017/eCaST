using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class RequestTrackerViewModel
    {

        public int RequestTrackerID { get; set; }
        public int? RequestID { get; set; }
        public int? ClientID { get; set; }
        public int? ScreeningID { get; set; }
        public int? ScreeningCheck { get; set; }
        public string Assignee { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Priority { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string RequestDesc { get; set; }
        public string DeveloperNotes { get; set; }




    }
}