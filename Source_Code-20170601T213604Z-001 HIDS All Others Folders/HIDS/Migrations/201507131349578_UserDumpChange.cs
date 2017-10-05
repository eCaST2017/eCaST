namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDumpChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.USER_DUMP", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.USER_DUMP", "Gender");
        }
    }
}
