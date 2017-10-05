namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleAppChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleApplications",
                c => new
                    {
                        RoleID = c.Int(nullable: false),
                        ApplicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleID, t.ApplicationID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RoleApplications");
        }
    }
}
