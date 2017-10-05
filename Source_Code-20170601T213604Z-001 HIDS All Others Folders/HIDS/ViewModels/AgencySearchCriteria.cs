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
    public class AgencySearchCriteria
    {

        public int AgencySiteID { get; set; }
        [Display(Name = "Agency Name")]
        public string AgencySiteName { get; set; }
        public bool? Active { get; set; }
        public List<string> RoleAList { get; set; }

    }
}