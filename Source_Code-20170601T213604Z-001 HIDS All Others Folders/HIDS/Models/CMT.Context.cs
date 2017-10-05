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
    
    public partial class PortalRequestEntitiesEntities : DbContext
    {
        public PortalRequestEntitiesEntities()
            : base("name=PortalRequestEntitiesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<ApplicationDivision> ApplicationDivisions { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<EdmMetadata> EdmMetadatas { get; set; }
        public virtual DbSet<GanttDependency> GanttDependencies { get; set; }
        public virtual DbSet<GanttResourceAssignment> GanttResourceAssignments { get; set; }
        public virtual DbSet<GanttResource> GanttResources { get; set; }
        public virtual DbSet<GanttTask> GanttTasks { get; set; }
        public virtual DbSet<ModuleApplication> ModuleApplications { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<ProjectApplication> ProjectApplications { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectSubApplication> ProjectSubApplications { get; set; }
        public virtual DbSet<ProjectType> ProjectTypes { get; set; }
        public virtual DbSet<QANote> QANotes { get; set; }
        public virtual DbSet<QANotesQualityAssurance> QANotesQualityAssurances { get; set; }
        public virtual DbSet<QualityAssuranceRequest> QualityAssuranceRequests { get; set; }
        public virtual DbSet<QualityAssurance> QualityAssurances { get; set; }
        public virtual DbSet<ReleaseApplication> ReleaseApplications { get; set; }
        public virtual DbSet<Release> Releases { get; set; }
        public virtual DbSet<RequestAction> RequestActions { get; set; }
        public virtual DbSet<RequestActionType> RequestActionTypes { get; set; }
        public virtual DbSet<RequestClient> RequestClients { get; set; }
        public virtual DbSet<RequestMode> RequestModes { get; set; }
        public virtual DbSet<RequestPerson> RequestPersons { get; set; }
        public virtual DbSet<RequestProgram> RequestPrograms { get; set; }
        public virtual DbSet<RequestProject> RequestProjects { get; set; }
        public virtual DbSet<RequestReleas> RequestReleases { get; set; }
        public virtual DbSet<RequestReport> RequestReports { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestScreening> RequestScreenings { get; set; }
        public virtual DbSet<RequestsDUMP> RequestsDUMPs { get; set; }
        public virtual DbSet<RequestTracker> RequestTrackers { get; set; }
        public virtual DbSet<RequestTypeApplication> RequestTypeApplications { get; set; }
        public virtual DbSet<RequestTypeNotification> RequestTypeNotifications { get; set; }
        public virtual DbSet<RequestType> RequestTypes { get; set; }
        public virtual DbSet<RequestTypeSubApplication> RequestTypeSubApplications { get; set; }
        public virtual DbSet<SiteDivision> SiteDivisions { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<SubApplicationApplication> SubApplicationApplications { get; set; }
        public virtual DbSet<SubApplication> SubApplications { get; set; }
        public virtual DbSet<UserIdApplication> UserIdApplications { get; set; }
        public virtual DbSet<UserIdRequest> UserIdRequests { get; set; }
        public virtual DbSet<UserIdSubApplication> UserIdSubApplications { get; set; }
        public virtual DbSet<UserOption> UserOptions { get; set; }
    }
}
