namespace CTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfilePostsChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfileUserPosts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.PostID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProfileUserPosts");
        }
    }
}
