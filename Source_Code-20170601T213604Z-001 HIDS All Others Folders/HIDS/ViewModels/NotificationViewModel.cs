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


namespace CTL.ViewModels
{
    public class NotificationViewModel
    {

        public int NotificationID { get; set; }
        public int? PostID { get; set; }
        public string PostSubject { get; set; }
        public string From { get; set; }
        public string ToField { get; set; }
        public string CarbonCopy { get; set; }
        public string BlindCarbonCopy { get; set; }
        [AllowHtml]
        public string PostBody { get; set; }
        public string FocusAreas { get; set; }
        public string MeetingTimes { get; set; }
        public string MeetingFormat { get; set; }
        public string ExpectedDeliveries { get; set; }
        public string Compensation { get; set; }
        public string Skills { get; set; }
        public string Knowledge { get; set; }
        public string Abilities { get; set; }
        public string Counties { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }



    }
}