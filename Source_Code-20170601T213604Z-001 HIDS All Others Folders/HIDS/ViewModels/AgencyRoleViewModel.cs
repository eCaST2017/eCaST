using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class AgencyRoleViewModel
    {


        public int? AgencyRoleBinID { get; set; }
        public string AgencyRoleBinName { get; set; }
        public string LegacyRoleName { get; set; }
        public string UserList { get; set; }
        public int? SiteID { get; set; }
        public int? ProgramID { get; set; }
        public int LegacyAgencyRoleID { get; set; }


    }
}