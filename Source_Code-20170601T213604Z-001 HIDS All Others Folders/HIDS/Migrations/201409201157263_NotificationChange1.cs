namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationChange1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "PostSubject", c => c.String());
            AddColumn("dbo.Notifications", "From", c => c.String());
            AddColumn("dbo.Notifications", "To", c => c.String());
            AddColumn("dbo.Notifications", "CarbonCopy", c => c.String());
            AddColumn("dbo.Notifications", "BlindCarbonCopy", c => c.String());
            AddColumn("dbo.Notifications", "PostBody", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "PostBody");
            DropColumn("dbo.Notifications", "BlindCarbonCopy");
            DropColumn("dbo.Notifications", "CarbonCopy");
            DropColumn("dbo.Notifications", "To");
            DropColumn("dbo.Notifications", "From");
            DropColumn("dbo.Notifications", "PostSubject");
        }
    }
}
