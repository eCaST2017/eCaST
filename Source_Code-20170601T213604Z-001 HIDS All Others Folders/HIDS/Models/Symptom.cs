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
    
    public partial class Symptom
    {
        public Symptom()
        {
            this.Screenings = new HashSet<Screening>();
        }
    
        public int SymptomID { get; set; }
        public string SymptomName { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<Screening> Screenings { get; set; }
    }
}
