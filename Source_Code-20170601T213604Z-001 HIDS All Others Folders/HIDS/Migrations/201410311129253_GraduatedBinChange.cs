namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GraduatedBinChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GraduatedBins",
                c => new
                    {
                        GraduatedBinID = c.Int(nullable: false, identity: true),
                        GraduatedBinName = c.String(),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GraduatedBinID);
            
            AddColumn("dbo.AspNetUsers", "GraduatedBinID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "GraduatedBinID");
            DropTable("dbo.GraduatedBins");
        }
    }
}
