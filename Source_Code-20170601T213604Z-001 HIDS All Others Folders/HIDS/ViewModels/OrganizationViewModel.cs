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
    public class OrganizationViewModel
    {

     
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationSubName { get; set; }
        public string MailingAddress { get; set; }
        public string CityName { get; set; }
        public int StateBinID { get; set; }
        public string StateBinName { get; set; }
        public int ZipCode { get; set; }
        public bool Active { get; set; } 
        [AllowHtml]
        public string OrgPic { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }





    }
}