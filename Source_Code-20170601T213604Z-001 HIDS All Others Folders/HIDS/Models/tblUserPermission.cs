//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CTL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblUserPermission
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public Nullable<byte> EditUser { get; set; }
        public Nullable<byte> AccessRpts { get; set; }
        public Nullable<byte> SearchMrg { get; set; }
        public Nullable<byte> AddMrg { get; set; }
        public Nullable<byte> EditMrg { get; set; }
        public Nullable<byte> DelMrg { get; set; }
        public Nullable<byte> PrintMrg { get; set; }
        public Nullable<byte> ImportMrg { get; set; }
        public Nullable<byte> SearchDis { get; set; }
        public Nullable<byte> AddDis { get; set; }
        public Nullable<byte> EditDis { get; set; }
        public Nullable<byte> DelDis { get; set; }
        public Nullable<byte> PrintDis { get; set; }
        public Nullable<byte> ImportDis { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<byte> AccessFinRpts { get; set; }
        public Nullable<byte> FlagMrg { get; set; }
        public Nullable<byte> FlagDis { get; set; }
    }
}
