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
    
    public partial class DeIdentified_Clinic_Visit_2015
    {
        public Nullable<int> Agency_Code { get; set; }
        public Nullable<int> Site_Code { get; set; }
        public int Person_ID { get; set; }
        public int Visit_ID { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<System.DateTime> Visit_Date { get; set; }
        public string Visit_County { get; set; }
        public string Provider_Type { get; set; }
        public string Before_Contraceptive_Description { get; set; }
        public string After_Contraceptive_Description { get; set; }
        public string Percent_Of_Poverty { get; set; }
        public string Visit_Type { get; set; }
        public string New_Returning { get; set; }
    }
}
