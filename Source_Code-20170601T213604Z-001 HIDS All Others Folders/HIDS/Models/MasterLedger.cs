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
    
    public partial class MasterLedger
    {
        public int ID { get; set; }
        public Nullable<int> ContractID { get; set; }
        public Nullable<int> ContractTypeID { get; set; }
        public Nullable<int> LedgerTypeID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<int> FiscalYear { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}
