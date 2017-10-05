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
    public class AgencyContactViewModel
    {


        public int? AgencyContactID { get; set; }
        public Guid? UserId { get; set; }
        public int? AgencyRoleBinID { get; set; }
        public int? AgencyRoleTypeBinID { get; set; }
        public int? ProgramBinID { get; set; }
        public int? SiteID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public DateTime? MITrainingDate { get; set; }
        public string SiteName { get; set; }
        public string AgencyRoleName { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public bool Active { get; set; }



    }
}