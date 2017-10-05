using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;


namespace CTL.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? StateBinID { get; set; }
        public string ZipCode { get; set; }
        [AllowHtml]
        public string ProfilePic { get; set; }
        public string Fax { get; set; }
        public int? RoleBinID { get; set; }
        public bool? PrivacyAgreement { get; set; }
        public int? OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public bool? firstTimeLoggedIn { get; set; }
        public bool? CurrentlyLoggedIn { get; set; }
        public string TelephoneNumber { get; set; }
        public bool? Status { get; set; }
        public bool? Active { get; set; }
        public string ImpersonationId { get; set; }
        public string ImpersonatorId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string LegacyId { get; set; }
        public string SiteAccess { get; set; }
        public string LegacyUserName { get; set; }
        public string ProgramAccess { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public int? CurrentApplicationID { get; set; }
        public bool? Dashboard { get; set; }
        public string ExternalID { get; set; }

        public async Task<ClaimsIdentity> GenerateIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var Identity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return Identity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<CTL.Models.ApplicationUser> AspNetUsers { get; set; }
        public DbSet<CTL.Models.ApplicationRolePrograms> ApplicationRolePrograms { get; set; }
        //public DbSet<CTL.Models.SiteAgencySites> SiteAgencySites { get; set; }
        //public DbSet<CTL.Models.ProgramSites> ProgramSites { get; set; }
        public DbSet<CTL.Models.SiteRoleProgramUserProfiles> SiteRoleProgramUserProfiles { get; set; }
        public DbSet<CTL.Models.RoleProgramUserProfiles> RoleProgramUserProfiles { get; set; }
        public DbSet<CTL.Models.AgencySiteProgramSites> AgencySiteProgramSites { get; set; }
        //public DbSet<CTL.Models.RoleApplications> RoleApplications { get; set; }
        public DbSet<CTL.Models.Dashboard> Dashboards { get; set; }
        public DbSet<CTL.Models.Application> Applications { get; set; }
        public DbSet<CTL.Models.Program> Programs { get; set; }
        public DbSet<CTL.Models.Site> Sites { get; set; }
        public DbSet<CTL.Models.RoleBin> RoleBins { get; set; }
        //public DbSet<CTL.Models.DDStateBin> DDStateBin { get; set; }
        //public DbSet<CTL.Models.DDCityBin> DDCityBin { get; set; }
        //public DbSet<CTL.Models.DDCountyBin> DDCountyBin { get; set; }
        public DbSet<CTL.Models.Report> Reports { get; set; }
        
        public DbSet<CTL.Models.AgencySite> AgencySites { get; set; }
        public DbSet<CTL.Models.AgencyContact> AgencyContacts { get; set; }
        //public DbSet<CTL.Models.DDAgencyRoleBin> DDAgencyRoleBin { get; set; }
      //  public DbSet<CTL.Models.DDAgencyRoleTypeBin> DDAgencyRoleTypeBin { get; set; }
        public DbSet<CTL.Models.Provider> Providers { get; set; }
        public DbSet<CTL.Models.ProviderProfile> ProviderProfile { get; set; }
        public DbSet<CTL.Models.SubContractor> SubContractors { get; set; }

        public DbSet<CTL.Models.ProviderSubContractors> ProviderSubContractors { get; set; }
        public DbSet<CTL.Models.ProviderProfileProviders> ProviderProfileProviders { get; set; }
        public DbSet<CTL.Models.AgencyContactClinics> AgencyContactClinics { get; set; }
        public DbSet<CTL.Models.SubContractorClinics> SubContractorClinics { get; set; }
        public DbSet<CTL.Models.ContractTypeRoles> ContractTypeRoles { get; set; }

        public DbSet<CTL.Models.Notice> Notices { get; set; }
        public DbSet<CTL.Models.DDNoticeTypeBin> DDNoticeTypeBins { get; set; }

        public ApplicationDbContext()
            : base("HIDSConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}