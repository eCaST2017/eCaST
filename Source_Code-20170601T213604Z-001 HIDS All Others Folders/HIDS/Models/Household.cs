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
    
    public partial class Household
    {
        public int HHID { get; set; }
        public Nullable<int> PersonID { get; set; }
        public Nullable<int> InsuranceID { get; set; }
        public Nullable<int> subInsuranceID { get; set; }
        public Nullable<int> countyResidenceID { get; set; }
        public Nullable<decimal> annualGrossIncome { get; set; }
        public Nullable<short> numberInFamily { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}
