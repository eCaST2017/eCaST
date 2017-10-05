using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class CancerTypeViewModel
    {


        public int CancerTypeBinID { get; set; }
        public string CancerTypeBinName { get; set; }
        public string FacilityList { get; set; }
        public int? SiteID { get; set; }
        public int? ClinicID { get; set; }
        public int? ProgramID { get; set; }


    }
}