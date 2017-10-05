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
    public class UserProfileViewModel
    {
        
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string TelephoneNumber { get; set; }
        public int? OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string OrgPic { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? StateID { get; set; }
        public int? GraduatedBinID { get; set; }
        public string GraduatedBinName { get; set; }
        public string StateBinName { get; set; }
        public string PartnerTypeName { get; set; }
        public string ZipCode { get; set; }
        public string Fax { get; set; }
        public string RoleBinName { get; set; }
        public bool? Status {get; set;}
        public bool PrivacyAgreement { get; set; }
        public DateTime? LastUpdated { get; set; }
        [AllowHtml]
        public string profilePic { get; set; }
        public bool? firstTimeLoggedIn { get; set; }
        public bool? CurrentlyLoggedIn { get; set; }
        public int? RoleBinID { get; set; }
        public bool? Active { get; set; }
        public bool? IsLoggedIn { get; set; }

        public int? AgencySiteID { get; set; }
        public string AgencySiteName { get; set; }
        public bool? Coordinator { get; set; }
        public int? CurrentApplicationID { get; set; }
        public bool? Dashboard { get; set; }
        public string ExternalID { get; set; }

        public string Code { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Focus Areas")]
        public string FocusAreas { get; set; }

        [Display(Name = "Skills")]
        public string Skills { get; set; }

        [Display(Name = "Knowledge")]
        public string Knowledge { get; set; }

        [Display(Name = "Abilities")]
        public string Abilities { get; set; }

        [Display(Name = "Languages")]
        public string Languages { get; set; }

        [Display(Name = "Whom")]
        public string Whom { get; set; }

        [Display(Name = "Counties")]
        public string Counties { get; set; }

        [Display(Name = "Women")]
        public string Women { get; set; }

        [Display(Name = "Youth")]
        public string Youth { get; set; }

        [Display(Name = "YoungChildren")]
        public string YoungChildren { get; set; }

        public List<string> RoleAList { get; set; }
        public List<string> AgencyRoleAList { get; set; }
        public List<string> SiteAList { get; set; }

        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }

    }
}