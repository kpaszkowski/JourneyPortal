namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLikedPostsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostsUsers",
                c => new
                    {
                        PostId = c.Int(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PostId, t.ApplicationUserId })
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.ApplicationUserId);
            
            AddColumn("dbo.Offers", "IsActive", c => c.Boolean(nullable: false , defaultValue : true));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostsUsers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostsUsers", "PostId", "dbo.Posts");
            DropIndex("dbo.PostsUsers", new[] { "ApplicationUserId" });
            DropIndex("dbo.PostsUsers", new[] { "PostId" });
            DropColumn("dbo.Offers", "IsActive");
            DropTable("dbo.PostsUsers");
        }
    }
}
