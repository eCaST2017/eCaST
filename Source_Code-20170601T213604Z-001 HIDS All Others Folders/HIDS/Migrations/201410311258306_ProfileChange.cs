namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.USER_DUMP",
                c => new
                    {
                        UserDumpID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Graduated = c.String(),
                    })
                .PrimaryKey(t => t.UserDumpID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.USER_DUMP");
        }
    }
}
