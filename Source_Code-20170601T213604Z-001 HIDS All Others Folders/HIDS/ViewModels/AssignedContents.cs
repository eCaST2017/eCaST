using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.ViewModels
{
    public class AssignedContents
    {

        public int ContentID { get; set; }
        public string ContentName { get; set; }
        public bool Assigned { get; set; }
        public bool Active { get; set; }


    }
}