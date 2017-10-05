using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class SubContractor
    {

        public int SubContractorID { get; set; }
        public string SubContractorName { get; set; }
        public bool Active { get; set; }
        public string LegacySubContractorCodeCCSP { get; set; }
        public Nullable<int> LegacySubContractorCodeWWC { get; set; }
        public Nullable<int> LegacySubContractorCodeCRCCP { get; set; }
        public string Address { get; set; }
        public Nullable<int> CityBinID { get; set; }
        public Nullable<int> StateBinID { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public Nullable<int> CountyBinID { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<int> DigitalMamIndicator { get; set; }
        public string DigitalTechCode { get; set; }
        public string ZipCode { get; set; }



    }
}