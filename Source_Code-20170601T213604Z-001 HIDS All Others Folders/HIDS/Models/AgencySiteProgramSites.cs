using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;


namespace CTL.Models
{
    public class AgencySiteProgramSites
    {


        //[Key, Column(Order = 0)]
        public int? LegacySiteID { get; set; }

        [Key, Column(Order = 1)]
        public int ProgramID { get; set; }

        [Key, Column(Order = 2)]
        public int SiteID { get; set; }

        public string SiteCode { get; set; }

        public int? LegacyClinicID { get; set; }
        public int? AgencySiteID { get; set; }
        public int? LegacyAgencySiteID { get; set; }
        public string LegacyClinicName { get; set; }
    }
}