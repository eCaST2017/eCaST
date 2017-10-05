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
    
    public partial class Report
    {
        public int ReportID { get; set; }
        public Nullable<bool> Active { get; set; }
        public string Title { get; set; }
        public Nullable<int> ProgramID { get; set; }
        public Nullable<bool> Communication { get; set; }
        public Nullable<bool> DateRange { get; set; }
        public Nullable<bool> Month { get; set; }
        public Nullable<bool> Provider { get; set; }
        public Nullable<bool> ProviderType { get; set; }
        public Nullable<bool> Hospital { get; set; }
        public Nullable<bool> Person { get; set; }
        public Nullable<int> Usage { get; set; }
        public bool StateOnly { get; set; }
        public bool SiteLevel { get; set; }
        public string RptDescription { get; set; }
        public string SiteAccess { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<int> ReportTypeID { get; set; }
    }
}
