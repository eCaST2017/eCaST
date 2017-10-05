using System;
using System.Collections.Generic;
using System.Linq;
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

namespace CTL.ViewModels
{
    public class MessageViewModel
    {


        public int MessageID { get; set; }
        public string MessageTitle { get; set; }
        public string MessageContent { get; set; }
        public string SendList { get; set; }
        public bool Active { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }



    }
}