using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class Program
    {


        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramContact { get; set; }
        public string ProgramPhone { get; set; }
        public string ProgramEmail { get; set; }
        public bool Active { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }




    }
}