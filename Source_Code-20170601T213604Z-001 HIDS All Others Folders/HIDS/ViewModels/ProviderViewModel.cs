using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class ProviderViewModel
    {


        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string ProviderSpecialty { get; set; }
        public bool Active { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }



    }
}