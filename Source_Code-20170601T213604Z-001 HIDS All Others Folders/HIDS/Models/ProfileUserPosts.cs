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
    public class ProfileUserPosts
    {

        [Key, Column(Order = 0)]
        public string Id { get; set; }

        [Key, Column(Order = 1)]
        public int PostID { get; set; }


    }
}