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
    
    public partial class WMDE_Errors
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public Nullable<int> SiteID { get; set; }
        public Nullable<System.DateTime> BPDate { get; set; }
        public Nullable<int> RowNumber { get; set; }
        public string FieldName { get; set; }
        public string ErrorType { get; set; }
        public string Error { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> BPDate1 { get; set; }
    }
}
