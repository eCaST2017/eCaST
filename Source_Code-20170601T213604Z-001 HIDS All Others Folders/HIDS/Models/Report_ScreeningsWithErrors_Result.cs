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
    
    public partial class Report_ScreeningsWithErrors_Result
    {
        public string AgencySiteName { get; set; }
        public string SiteName { get; set; }
        public int ClientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime DOB { get; set; }
        public int ClinicalCycleID { get; set; }
        public string MessageNameScreen { get; set; }
        public int ScreeningID { get; set; }
        public Nullable<int> CancerTypeBinID { get; set; }
        public string CancerTypeBinName { get; set; }
        public Nullable<System.DateTime> ProcedureDate { get; set; }
    }
}