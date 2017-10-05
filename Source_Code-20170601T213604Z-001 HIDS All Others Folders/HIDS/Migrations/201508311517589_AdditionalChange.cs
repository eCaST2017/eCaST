namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContentContentLists", "Content_ContentID", "dbo.Contents");
            DropForeignKey("dbo.ContentContentLists", "ContentList_ContentListID", "dbo.ContentLists");
            DropForeignKey("dbo.Organizations", "StateBinID", "dbo.StateBins");
            DropIndex("dbo.Organizations", new[] { "StateBinID" });
            DropIndex("dbo.ContentContentLists", new[] { "Content_ContentID" });
            DropIndex("dbo.ContentContentLists", new[] { "ContentList_ContentListID" });
            DropTable("dbo.Assets");
            DropTable("dbo.ContentContents");
            DropTable("dbo.ContentLists");
            DropTable("dbo.Contents");
            DropTable("dbo.ContentTypes");
            DropTable("dbo.MessageAspNetUsers");
            DropTable("dbo.Messages");
            DropTable("dbo.Notifications");
            DropTable("dbo.Organizations");
            DropTable("dbo.ParentContentContents");
            DropTable("dbo.SupportTickets");
            DropTable("dbo.ContentContentLists");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ContentContentLists",
                c => new
                    {
                        Content_ContentID = c.Int(nullable: false),
                        ContentList_ContentListID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Content_ContentID, t.ContentList_ContentListID });
            
            CreateTable(
                "dbo.SupportTickets",
                c => new
                    {
                        SupportTicketID = c.Int(nullable: false, identity: true),
                        SupportTicketEmail = c.String(),
                        SupportTicketTitle = c.String(),
                        SupportTicketContent = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SupportTicketID);
            
            CreateTable(
                "dbo.ParentContentContents",
                c => new
                    {
                        ParentContentID = c.Int(nullable: false),
                        ContentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentContentID, t.ContentID });
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        OrganizationID = c.Int(nullable: false, identity: true),
                        OrganizationName = c.String(),
                        OrganizationSubName = c.String(),
                        MailingAddress = c.String(),
                        CityName = c.String(),
                        StateBinID = c.Int(nullable: false),
                        ZipCode = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        OrgPic = c.String(),
                        DateUpdated = c.DateTime(),
                        UpdatedBy = c.String(),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.OrganizationID);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationID = c.Int(nullable: false, identity: true),
                        PostID = c.Int(),
                        PostSubject = c.String(),
                        From = c.String(),
                        To = c.String(),
                        CarbonCopy = c.String(),
                        BlindCarbonCopy = c.String(),
                        PostBody = c.String(),
                        DateUpdated = c.DateTime(),
                        UpdatedBy = c.String(),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.NotificationID);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        MessageEmail = c.String(),
                        MessageTitle = c.String(),
                        MessageContent = c.String(),
                        Active = c.Boolean(nullable: false),
                        DateUpdated = c.DateTime(),
                        UpdatedBy = c.String(),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.MessageID);
            
            CreateTable(
                "dbo.MessageAspNetUsers",
                c => new
                    {
                        MessageID = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MessageID, t.Id });
            
            CreateTable(
                "dbo.ContentTypes",
                c => new
                    {
                        ContentTypeID = c.Int(nullable: false, identity: true),
                        ContentTypeName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ContentTypeID);
            
            CreateTable(
                "dbo.Contents",
                c => new
                    {
                        ContentID = c.Int(nullable: false, identity: true),
                        ContentTypeID = c.Int(),
                        ContentTitle = c.String(),
                        ContentBody = c.String(),
                        ContentFooter = c.String(),
                        Active = c.Boolean(nullable: false),
                        IsMainPage = c.Boolean(nullable: false),
                        IsSecure = c.Boolean(nullable: false),
                        pageOrder = c.Int(),
                        DateUpdated = c.DateTime(),
                        UpdatedBy = c.String(),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.ContentID);
            
            CreateTable(
                "dbo.ContentLists",
                c => new
                    {
                        ContentListID = c.Int(nullable: false, identity: true),
                        ContentOID = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ContentListID);
            
            CreateTable(
                "dbo.ContentContents",
                c => new
                    {
                        ContentListID = c.Int(nullable: false),
                        ContentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ContentListID, t.ContentID });
            
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        AssetID = c.Int(nullable: false, identity: true),
                        AssetName = c.String(),
                        Active = c.Boolean(nullable: false),
                        ContentBody = c.String(),
                        ContentFooter = c.String(),
                    })
                .PrimaryKey(t => t.AssetID);
            
            CreateIndex("dbo.ContentContentLists", "ContentList_ContentListID");
            CreateIndex("dbo.ContentContentLists", "Content_ContentID");
            CreateIndex("dbo.Organizations", "StateBinID");
            AddForeignKey("dbo.Organizations", "StateBinID", "dbo.StateBins", "StateBinID", cascadeDelete: true);
            AddForeignKey("dbo.ContentContentLists", "ContentList_ContentListID", "dbo.ContentLists", "ContentListID", cascadeDelete: true);
            AddForeignKey("dbo.ContentContentLists", "Content_ContentID", "dbo.Contents", "ContentID", cascadeDelete: true);
        }
    }
}
