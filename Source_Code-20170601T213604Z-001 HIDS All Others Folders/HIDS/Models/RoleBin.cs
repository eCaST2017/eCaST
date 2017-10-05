using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class RoleBin
    {


        public int RoleBinID { get; set; }
        public string RoleBinName { get; set; }
        public bool Active { get; set; }
        public System.Guid LegacyRoleID { get; set; }
        public string LegacyRoleName { get; set; }
        public Nullable<bool> IsPortalUser { get; set; }
        public Nullable<int> AgencyTypeRoleBinID { get; set; }
        public Nullable<int> LegacyAgencyRoleID { get; set; }



    }
}