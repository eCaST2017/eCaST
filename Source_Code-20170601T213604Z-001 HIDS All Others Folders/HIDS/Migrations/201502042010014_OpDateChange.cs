namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OpDateChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "OpportunityOpenDate", c => c.DateTime());
            AddColumn("dbo.Posts", "OpportunityExpirationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "OpportunityExpirationDate");
            DropColumn("dbo.Posts", "OpportunityOpenDate");
        }
    }
}
