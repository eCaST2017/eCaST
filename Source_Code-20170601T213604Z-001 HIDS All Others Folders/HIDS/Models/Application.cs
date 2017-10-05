using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class Application
    {

        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationDescription { get; set; }
        public string HTTP { get; set; }
        public bool Active { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }




    }
}