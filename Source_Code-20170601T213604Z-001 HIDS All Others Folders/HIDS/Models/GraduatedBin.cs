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

namespace CTL.Models
{
    public class GraduatedBin
    {



        [Key]
        public int GraduatedBinID { get; set; }
        public string GraduatedBinName { get; set; }
        public int Active { get; set; }



    }
}