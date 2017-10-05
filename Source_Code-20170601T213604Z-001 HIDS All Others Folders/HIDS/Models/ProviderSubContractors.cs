using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace CTL.Models
{
    public class ProviderSubContractors
    {


        [Key, Column(Order = 0)]
        public int ProviderID { get; set; }

        [Key, Column(Order = 1)]
        public int SubContractorID { get; set; }

        [Key, Column(Order = 2)]
        public int ProgramID { get; set; } 



    }
}