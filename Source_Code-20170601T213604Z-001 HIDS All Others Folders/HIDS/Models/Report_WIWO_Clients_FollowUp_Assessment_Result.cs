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
    
    public partial class Report_WIWO_Clients_FollowUp_Assessment_Result
    {
        public string AgencySiteName { get; set; }
        public string SiteName { get; set; }
        public int clientid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public int ScreeningID { get; set; }
        public Nullable<System.DateTime> CompletedSessions { get; set; }
        public Nullable<int> Type { get; set; }
        public int ClinicalCycleID { get; set; }
        public Nullable<int> HBSessionCompletionTypeBinID { get; set; }
        public string HBSessionCompletionTypeBinName { get; set; }
        public Nullable<int> DaysBetweenSessions { get; set; }
    }
}