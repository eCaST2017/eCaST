using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class ProgramViewModel
    {

        public string Id { get; set; }
        public int ProgramID { get; set; }
        public int? RoleBinID { get; set; }
        public int? SiteID { get; set; }
        public string RoleBinName { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramContact { get; set; }
        public string ProgramEmail { get; set; }
        public List<string> ApplicationAList { get; set; }
        public bool Active { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }

    }
}