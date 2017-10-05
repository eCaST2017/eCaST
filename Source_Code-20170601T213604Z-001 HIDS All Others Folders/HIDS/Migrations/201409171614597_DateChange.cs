namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateChange : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.AttributeUsers");
            //DropPrimaryKey("dbo.CountyBinUsers");
            //AlterColumn("dbo.AttributeUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            //AlterColumn("dbo.CountyBinUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Posts", "OpenDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Posts", "ExpirationDate", c => c.DateTime(nullable: false));
            //AddPrimaryKey("dbo.AttributeUsers", new[] { "AttributeID", "Id" });
            //AddPrimaryKey("dbo.CountyBinUsers", new[] { "CountyBinID", "Id" });
        }
        
        public override void Down()
        {
            //DropPrimaryKey("dbo.CountyBinUsers");
            //DropPrimaryKey("dbo.AttributeUsers");
            AlterColumn("dbo.Posts", "ExpirationDate", c => c.DateTime());
            AlterColumn("dbo.Posts", "OpenDate", c => c.DateTime());
            //AlterColumn("dbo.CountyBinUsers", "Id", c => c.Int(nullable: false));
            //AlterColumn("dbo.AttributeUsers", "Id", c => c.Int(nullable: false));
            //AddPrimaryKey("dbo.CountyBinUsers", new[] { "CountyBinID", "Id" });
            //AddPrimaryKey("dbo.AttributeUsers", new[] { "AttributeID", "Id" });
        }
    }
}
