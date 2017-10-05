using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class Attribute
    {

        public int AttributeID { get; set; }
        public int? AttributeParentID { get; set; }
        public string AttributeName { get; set; }
        public bool Active { get; set; }


    }
}