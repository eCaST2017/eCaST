namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhoneChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TelephoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TelephoneNumber");
        }
    }
}
