using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CTL.Models
{
    public class DDNoticeTypeBin
    {

        [Key]
        public int NoticeTypeBinID { get; set; }
        public string NoticeTypeBinName { get; set; }
        public string Icon { get; set; }
        public string Alert { get; set; }
        public bool Active { get; set; }



    }
}