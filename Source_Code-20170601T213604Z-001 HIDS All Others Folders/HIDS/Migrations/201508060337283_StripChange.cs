namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StripChange : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.AttributeParents");
            DropTable("dbo.AttributePosts");
            DropTable("dbo.Attributes");
            DropTable("dbo.AttributeUsers");
            DropTable("dbo.CountyBinPosts");
            DropTable("dbo.CountyBins");
            DropTable("dbo.CountyBinUsers");
            DropTable("dbo.EthnicityBins");
            DropTable("dbo.FLTICommunityBins");
            DropTable("dbo.GenderBins");
            DropTable("dbo.GraduatedBins");
            DropTable("dbo.NotificationPosts");
            DropTable("dbo.PartnerAspNetUsers");
            DropTable("dbo.PartnerPartners");
            DropTable("dbo.PartnerTypeAspNetUsers");
            DropTable("dbo.PartnerTypeBins");
            DropTable("dbo.PostOrganizations");
            DropTable("dbo.Posts");
            DropTable("dbo.PostTypes");
            DropTable("dbo.ProfileUserPosts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProfileUserPosts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.PostID });
            
            CreateTable(
                "dbo.PostTypes",
                c => new
                    {
                        PostTypeID = c.Int(nullable: false, identity: true),
                        PostTypeName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PostTypeID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        PostTypeID = c.Int(),
                        PostTitle = c.String(),
                        PostContent = c.String(),
                        Active = c.Boolean(nullable: false),
                        OpenDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        OpportunityOpenDate = c.DateTime(),
                        OpportunityExpirationDate = c.DateTime(),
                        AwardedId = c.String(),
                        OutsideAwardedName = c.String(),
                        DateUpdated = c.DateTime(),
                        UpdatedBy = c.String(),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.PostID);
            
            CreateTable(
                "dbo.PostOrganizations",
                c => new
                    {
                        PostID = c.Int(nullable: false),
                        OrganizationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostID, t.OrganizationID });
            
            CreateTable(
                "dbo.PartnerTypeBins",
                c => new
                    {
                        PartnerTypeBinID = c.Int(nullable: false, identity: true),
                        PartnerTypeBinName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PartnerTypeBinID);
            
            CreateTable(
                "dbo.PartnerTypeAspNetUsers",
                c => new
                    {
                        PartnerTypeBinID = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PartnerTypeBinID, t.Id });
            
            CreateTable(
                "dbo.PartnerPartners",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PartnerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.PartnerID });
            
            CreateTable(
                "dbo.PartnerAspNetUsers",
                c => new
                    {
                        PartnerID = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PartnerID, t.Id });
            
            CreateTable(
                "dbo.NotificationPosts",
                c => new
                    {
                        NotificationID = c.Int(nullable: false),
                        PostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NotificationID, t.PostID });
            
            CreateTable(
                "dbo.GraduatedBins",
                c => new
                    {
                        GraduatedBinID = c.Int(nullable: false, identity: true),
                        GraduatedBinName = c.String(),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GraduatedBinID);
            
            CreateTable(
                "dbo.GenderBins",
                c => new
                    {
                        GenderBinID = c.Int(nullable: false, identity: true),
                        GenderBinName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GenderBinID);
            
            CreateTable(
                "dbo.FLTICommunityBins",
                c => new
                    {
                        FLTICommunityBinID = c.Int(nullable: false, identity: true),
                        FLTICommunityBinName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FLTICommunityBinID);
            
            CreateTable(
                "dbo.EthnicityBins",
                c => new
                    {
                        EthnicityBinID = c.Int(nullable: false, identity: true),
                        EthnicityBinName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EthnicityBinID);
            
            CreateTable(
                "dbo.CountyBinUsers",
                c => new
                    {
                        CountyBinID = c.Int(nullable: false),
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CountyBinID, t.Id });
            
            CreateTable(
                "dbo.CountyBins",
                c => new
                    {
                        CountyBinID = c.Int(nullable: false, identity: true),
                        CountyBinName = c.String(),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CountyBinID);
            
            CreateTable(
                "dbo.CountyBinPosts",
                c => new
                    {
                        CountyBinID = c.Int(nullable: false),
                        PostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CountyBinID, t.PostID });
            
            CreateTable(
                "dbo.AttributeUsers",
                c => new
                    {
                        AttributeID = c.Int(nullable: false),
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AttributeID, t.Id });
            
            CreateTable(
                "dbo.Attributes",
                c => new
                    {
                        AttributeID = c.Int(nullable: false, identity: true),
                        AttributeParentID = c.Int(),
                        AttributeName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AttributeID);
            
            CreateTable(
                "dbo.AttributePosts",
                c => new
                    {
                        AttributeID = c.Int(nullable: false),
                        PostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AttributeID, t.PostID });
            
            CreateTable(
                "dbo.AttributeParents",
                c => new
                    {
                        AttributeParentID = c.Int(nullable: false, identity: true),
                        AttributeParentName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AttributeParentID);
            
        }
    }
}
