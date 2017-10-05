using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class AgencySite
    {

        public int AgencySiteID { get; set; }
        public string AgencySiteName { get; set; }
        public bool Active { get; set; }
        public string AgencyLegalName { get; set; }
        public string Address { get; set; }
        public string SecondaryAddress { get; set; }
        public Nullable<int> CityBinID { get; set; }
        public Nullable<int> StateBinID { get; set; }
        public Nullable<int> ZipCode { get; set; }
        public Nullable<int> RegionBinID { get; set; }
        public Nullable<int> CountyCodeBinID { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string FEIN { get; set; }
        public string MailAddress { get; set; }
        public string SecondaryMailAddress { get; set; }
        public Nullable<int> MailCityBinID { get; set; }
        public Nullable<int> MailStateBinID { get; set; }
        public Nullable<int> MailZipCode { get; set; }
        public string Website { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }



    }
}