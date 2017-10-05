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
    
    public partial class V_PatientInfoTBI
    {
        public int Program_ID { get; set; }
        public string Site_Name { get; set; }
        public string County { get; set; }
        public int Patient_ID { get; set; }
        public string Program_Code { get; set; }
        public Nullable<int> HCP_Region_ID { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public Nullable<System.DateTime> Date_of_Birth { get; set; }
        public Nullable<int> Gender_ID { get; set; }
        public string HH_Last_Name { get; set; }
        public string HH_First_Name { get; set; }
        public string HH_MiddleName { get; set; }
        public string HH_Addressee { get; set; }
        public string HH_Mailing_Addr_Line1 { get; set; }
        public string HH_Mailing_Addr_Line2 { get; set; }
        public string HH_Mailing_Addr_City { get; set; }
        public string HH_Mailing_State_Cd { get; set; }
        public string HH_Mailing_Addr_Zip_Cd { get; set; }
        public string HH_Phone_Number { get; set; }
        public string HH_Email { get; set; }
        public Nullable<bool> HH_English_Ind { get; set; }
        public string County_Of_Residence { get; set; }
        public string Language { get; set; }
        public string LanguageWritten { get; set; }
        public string SSN { get; set; }
        public Nullable<int> Status_ID { get; set; }
        public Nullable<int> StatusReasonID { get; set; }
        public Nullable<System.DateTime> Status_date { get; set; }
        public string RelationshipTypeCode { get; set; }
        public string Relationship { get; set; }
    }
}
