using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class SubContractorViewModel
    {

        public int? SubContractorID { get; set; }
        public string SubContractorName { get; set; }
        public bool Active { get; set; }
        public int? ProviderCount { get; set; }
        public int? SiteID { get; set; }
        public int? ClinicID { get; set; }
        public int? ProgramID { get; set; }
        public string ClinicName { get; set; }
        public string Address { get; set; }
        public string SecondaryAddress { get; set; }
        public int? CityBinID { get; set; }
        public int? CountyBinID { get; set; }
        public int? StateBinID { get; set; }
        public string ZipCode { get; set; }
        public string CancerTypeName { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }

    }
}