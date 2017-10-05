using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class OrganizationsViewModel
    {

        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationSubName { get; set; }
        public string MailingAddress { get; set; }
        public string CityName { get; set; }
        public int ZipCode { get; set; }
        public bool Active { get; set; }
        public int MemberCount { get; set; }
        public int RequestCount { get; set; }


    }
}