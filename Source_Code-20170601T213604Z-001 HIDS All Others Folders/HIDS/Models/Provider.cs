using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class Provider
    {

        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
        public bool Active { get; set; }
        public Nullable<int> LegacyProfileType { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<int> LegacyColoCRCCPID { get; set; }
        public Nullable<int> LegacyColoCCSPID { get; set; }



    }
}