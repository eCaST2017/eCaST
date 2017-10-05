using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class ContentList
    {

        public int ContentListID { get; set; }
        public int ContentOID { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Content> Contents { get; set; }
    }
}