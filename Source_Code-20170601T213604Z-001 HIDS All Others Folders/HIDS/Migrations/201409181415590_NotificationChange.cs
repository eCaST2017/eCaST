namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationPosts",
                c => new
                    {
                        NotificationID = c.Int(nullable: false),
                        PostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NotificationID, t.PostID });
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationID = c.Int(nullable: false, identity: true),
                        PostID = c.Int(),
                        DateUpdated = c.DateTime(),
                        UpdatedBy = c.String(),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.NotificationID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notifications");
            DropTable("dbo.NotificationPosts");
        }
    }
}
