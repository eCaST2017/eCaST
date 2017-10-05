namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AwardedPostChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "AwardedId", c => c.String());
            AddColumn("dbo.Posts", "OutsideAwardedName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "OutsideAwardedName");
            DropColumn("dbo.Posts", "AwardedId");
        }
    }
}
