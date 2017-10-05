using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class ApplicationViewModel
    {


        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationDescription { get; set; }
        public string HTTP { get; set; }
        public int? RoleBinID { get; set; }
        public int? RoleListCount { get; set; }
        public bool Active { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public List<string> ProgramAList { get; set; }


    }
}