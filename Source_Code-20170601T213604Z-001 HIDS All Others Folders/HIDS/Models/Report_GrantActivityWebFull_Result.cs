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
    
    public partial class Report_GrantActivityWebFull_Result
    {
        public System.DateTime BillingDate { get; set; }
        public string ContractType { get; set; }
        public string AgencySiteName { get; set; }
        public string AgencyLegalName { get; set; }
        public string FEIN { get; set; }
        public string MailAddress { get; set; }
        public string Address { get; set; }
        public string CityBinName { get; set; }
        public string StateCode { get; set; }
        public Nullable<int> ZipCode { get; set; }
        public Nullable<int> MailZipCode { get; set; }
        public string FiscalContactFirst { get; set; }
        public string FiscalContactLast { get; set; }
        public string FiscalPhone { get; set; }
        public string FiscalEmail { get; set; }
        public Nullable<System.DateTime> ContactStart { get; set; }
        public Nullable<System.DateTime> ContractEnd { get; set; }
        public string ContractID { get; set; }
        public string PayableObjectCode { get; set; }
        public int InvoiceNumber { get; set; }
        public Nullable<System.DateTime> InvoicePosted { get; set; }
        public Nullable<int> ClientID { get; set; }
        public string ClientLastName { get; set; }
        public string ClientFirstName { get; set; }
        public Nullable<System.DateTime> ClientDOB { get; set; }
        public Nullable<System.DateTime> DateLastProcedure { get; set; }
        public Nullable<int> BillingLevel { get; set; }
        public Nullable<int> CancerTypeID { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public string COFERSFederal { get; set; }
        public string COFERSState { get; set; }
        public Nullable<decimal> TotalGrantPayment { get; set; }
        public Nullable<decimal> FederalTotal { get; set; }
        public Nullable<decimal> StateTotal { get; set; }
        public string FederalAccount { get; set; }
        public string StateAccount { get; set; }
    }
}
