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
    public class SiteRoleProgramUserProfiles
    {


        [Key, Column(Order = 0)]
        public int RoleID { get; set; }

        [Key, Column(Order = 1)]
        public int ProgramID { get; set; }

        [Key, Column(Order = 2)]
        public int SiteID { get; set; }

        [Key, Column(Order = 3)]
        public string Id { get; set; }

        public bool Training { get; set; }

        public int AgencySiteID { get; set; }

    }
}