using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class SiteViewModel
    {

        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteCode { get; set; }
        public int? AgencySiteID { get; set; }
        public string Address { get; set; }
        public string SecondaryAddress { get; set; }
        public int? CityBinID { get; set; }
        public int? CountyCodeBinID { get; set; }
        public int? StateBinID { get; set; }
        public int? ZipCodeBinID { get; set; }
        public int? ZipCode { get; set; }
        public string ReferralPhone { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string ProgramName { get; set; }
        public string ProgramList { get; set; }
        public string ProgramNameE { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public List<string> RoleAList { get; set; }
        public List<string> ServiceAList { get; set; }
        public string ServiceList { get; set; }
        public string ServiceName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}