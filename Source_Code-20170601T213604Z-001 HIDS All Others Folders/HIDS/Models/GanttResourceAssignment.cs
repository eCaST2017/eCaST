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
    
    public partial class GanttResourceAssignment
    {
        public int ID { get; set; }
        public int TaskID { get; set; }
        public Nullable<int> ResourceID { get; set; }
        public Nullable<decimal> Units { get; set; }
    }
}
