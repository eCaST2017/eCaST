using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class Site
    {

        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public bool Active { get; set; }
        public string Address { get; set; }
        public string SecondaryAddress { get; set; }
        public Nullable<int> CityBinID { get; set; }
        public Nullable<int> StateBinID { get; set; }
        public Nullable<int> ZipCode { get; set; }
        public string ZipCode4 { get; set; }
        public Nullable<int> CountyCodeBinID { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ReferralPhone { get; set; }
        public Nullable<int> NPI { get; set; }
        public Nullable<int> BCCP { get; set; }
        public Nullable<bool> HCPClinic { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string ContactName { get; set; }
        public string Extension { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

    }
}