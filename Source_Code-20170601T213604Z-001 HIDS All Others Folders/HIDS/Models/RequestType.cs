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
    
    public partial class RequestType
    {
        public int RequestTypeID { get; set; }
        public string RequestTypeName { get; set; }
        public int Active { get; set; }
        public Nullable<int> RequestModeID { get; set; }
        public Nullable<int> NotificationID { get; set; }
    }
}
