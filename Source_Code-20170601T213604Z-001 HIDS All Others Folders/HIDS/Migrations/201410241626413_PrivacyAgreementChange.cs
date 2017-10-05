namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrivacyAgreementChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PrivacyAgreement", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PrivacyAgreement");
        }
    }
}
