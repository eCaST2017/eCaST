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
    public class PostViewModel
    {


        public int PostID { get; set; }
        public int? PostTypeID { get; set; }
        public string PostTypeName { get; set; }
        public string PostTitle { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string PostContent { get; set; }
        public string FollowNotes { get; set; }
        public bool Active { get; set; }
        public DateTime OpenDate { get; set; }
        public string OrgPic { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? OpportunityOpenDate { get; set; }
        public DateTime? OpportunityExpirationDate { get; set; }
        public string AwardedId { get; set; }
        public string OutsideAwardedName { get; set; }
        public int? PostFollowersCount { get; set; }
        public int? IsFollowed { get; set; }
        public string MeetingTimes { get; set; }
        public string MeetingFormat { get; set; }
        public string ExpectedDeliveries { get; set; }
        public string Compensation { get; set; }
        public string FocusAreas { get; set; }
        [Display(Name = "Women")]
        public string Women { get; set; }
        [Display(Name = "Youth")]
        public string Youth { get; set; }
        [Display(Name = "YoungChildren")]
        public string YoungChildren { get; set; }
        public string Skills { get; set; }
        public string Knowledge { get; set; }
        public string Abilities { get; set; }
        public string Counties { get; set; }
        public int? OrganizationID { get; set; }
        public string OrganizationName { get; set; }

        // User Info
        //public string Id { get; set; }
        //public string UserName { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string PhoneNumber { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public int? StateID { get; set; }
        //public string StateBinName { get; set; }
        //public string ZipCode { get; set; }
        //public string Fax { get; set; }


        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }


    }
}