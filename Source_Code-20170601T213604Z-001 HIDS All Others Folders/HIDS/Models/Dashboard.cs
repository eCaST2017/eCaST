using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class Dashboard
    {

        public int DashboardID { get; set; }
        public string DashboardName { get; set; }
        public string Theme { get; set; }
        public bool Active { get; set; }


    }
}