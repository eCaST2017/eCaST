namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DemographicsChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EthnicityBins",
                c => new
                    {
                        EthnicityBinID = c.Int(nullable: false, identity: true),
                        EthnicityBinName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EthnicityBinID);
            
            CreateTable(
                "dbo.FLTICommunityBins",
                c => new
                    {
                        FLTICommunityBinID = c.Int(nullable: false, identity: true),
                        FLTICommunityBinName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FLTICommunityBinID);
            
            CreateTable(
                "dbo.GenderBins",
                c => new
                    {
                        GenderBinID = c.Int(nullable: false, identity: true),
                        GenderBinName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GenderBinID);
            
            AddColumn("dbo.AspNetUsers", "GenderBinID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "EthnicityBinID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "FLTICommunityBinID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "FLTICommunityProjectTitle", c => c.String());
            AddColumn("dbo.AspNetUsers", "FLTICommunityProjectDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FLTICommunityProjectDescription");
            DropColumn("dbo.AspNetUsers", "FLTICommunityProjectTitle");
            DropColumn("dbo.AspNetUsers", "FLTICommunityBinID");
            DropColumn("dbo.AspNetUsers", "EthnicityBinID");
            DropColumn("dbo.AspNetUsers", "GenderBinID");
            DropTable("dbo.GenderBins");
            DropTable("dbo.FLTICommunityBins");
            DropTable("dbo.EthnicityBins");
        }
    }
}
