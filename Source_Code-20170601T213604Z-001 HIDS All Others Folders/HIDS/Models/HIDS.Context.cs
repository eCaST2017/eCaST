﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HIDSEntities : DbContext
    {
        public HIDSEntities()
            : base("name=HIDSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DDCityBin> DDCityBins { get; set; }
        public virtual DbSet<DDCountyBin> DDCountyBins { get; set; }
        public virtual DbSet<DDStateBin> DDStateBins { get; set; }
        public virtual DbSet<DDAgencyRoleBin> DDAgencyRoleBins { get; set; }
        public virtual DbSet<DDAgencyRoleTypeBin> DDAgencyRoleTypeBins { get; set; }
    }
}
