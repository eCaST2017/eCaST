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

namespace CTL.Models
{
    public class Organization
    {

        [Key]
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationSubName { get; set; }
        [Display(Name = "Mailing Address")]
        public string MailingAddress { get; set; }
        [Display(Name = "City")]
        public string CityName { get; set; }
        public int StateBinID { get; set; }
        [Display(Name = "ZipCode")]
        public int ZipCode { get; set; }
        [Display(Name = "Status")]
        public bool Active { get; set; }
        [AllowHtml]
        public string OrgPic { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }

        [Display(Name = "State")]
        public virtual StateBin StateBins { get; set; }

        //public virtual ICollection<UserProfile> AspNetUsers { get; set; }

    }


}