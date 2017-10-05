namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BigChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgencySites",
                c => new
                    {
                        AgencySiteID = c.Int(nullable: false, identity: true),
                        AgencySiteName = c.String(),
                        Address = c.String(),
                        SecondaryAddress = c.String(),
                        CityBinID = c.Int(),
                        StateBinID = c.Int(),
                        ZipCodeBinID = c.Int(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Active = c.Boolean(nullable: false),
                        DateUpdated = c.DateTime(),
                        UpdatedBy = c.String(),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.AgencySiteID);
            
            CreateTable(
                "dbo.ApplicationPrograms",
                c => new
                    {
                        ApplicationID = c.Int(nullable: false),
                        ProgramID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationID, t.ProgramID });
            
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationID = c.Int(nullable: false, identity: true),
                        ApplicationName = c.String(),
                        ApplicationDescription = c.String(),
                        HTTP = c.String(),
                        Active = c.Boolean(nullable: false),
                        DateUpdated = c.DateTime(),
                        UpdatedBy = c.String(),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.ApplicationID);
            
            CreateTable(
                "dbo.Programs",
                c => new
                    {
                        ProgramID = c.Int(nullable: false, identity: true),
                        ProgramName = c.String(),
                        ProgramCode = c.String(),
                        ProgramContact = c.String(),
                        ProgramPhone = c.String(),
                        ProgramEmail = c.String(),
                        Active = c.Boolean(nullable: false),
                        DateUpdated = c.DateTime(),
                        UpdatedBy = c.String(),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.ProgramID);
            
            CreateTable(
                "dbo.ProgramSites",
                c => new
                    {
                        ProgramID = c.Int(nullable: false),
                        SiteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProgramID, t.SiteID });
            
            CreateTable(
                "dbo.RoleAgencySites",
                c => new
                    {
                        RoleID = c.Int(nullable: false),
                        AgencySiteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleID, t.AgencySiteID });
            
            CreateTable(
                "dbo.RoleProgramUserProfiles",
                c => new
                    {
                        RoleID = c.Int(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Training = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleID, t.ProgramID, t.Id });
            
            CreateTable(
                "dbo.SiteAgencySites",
                c => new
                    {
                        SiteID = c.Int(nullable: false),
                        AgencySiteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SiteID, t.AgencySiteID });
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        SiteID = c.Int(nullable: false, identity: true),
                        SiteName = c.String(),
                        SiteCode = c.String(),
                        Address = c.String(),
                        CityBinID = c.Int(),
                        StateBinID = c.Int(),
                        ZipCodeBinID = c.Int(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Active = c.Boolean(nullable: false),
                        DateUpdated = c.DateTime(),
                        UpdatedBy = c.String(),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.SiteID);
            
            CreateTable(
                "dbo.UserProfileSites",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SiteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.SiteID });
            
            DropColumn("dbo.AspNetUsers", "GraduatedBinID");
            DropColumn("dbo.AspNetUsers", "GenderBinID");
            DropColumn("dbo.AspNetUsers", "EthnicityBinID");
            DropColumn("dbo.AspNetUsers", "FLTICommunityBinID");
            DropColumn("dbo.AspNetUsers", "FLTICommunityProjectTitle");
            DropColumn("dbo.AspNetUsers", "FLTICommunityProjectDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "FLTICommunityProjectDescription", c => c.String());
            AddColumn("dbo.AspNetUsers", "FLTICommunityProjectTitle", c => c.String());
            AddColumn("dbo.AspNetUsers", "FLTICommunityBinID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "EthnicityBinID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "GenderBinID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "GraduatedBinID", c => c.Int());
            DropTable("dbo.UserProfileSites");
            DropTable("dbo.Sites");
            DropTable("dbo.SiteAgencySites");
            DropTable("dbo.RoleProgramUserProfiles");
            DropTable("dbo.RoleAgencySites");
            DropTable("dbo.ProgramSites");
            DropTable("dbo.Programs");
            DropTable("dbo.Applications");
            DropTable("dbo.ApplicationPrograms");
            DropTable("dbo.AgencySites");
        }
    }
}
