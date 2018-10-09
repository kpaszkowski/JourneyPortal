namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createSampleForumModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                        LastActivity = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LastActivity = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        IsDisabled = c.Boolean(nullable: false),
                        Views = c.Int(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        Category_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Like = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Topic_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Topics", t => t.Topic_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Topic_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "Topic_Id", "dbo.Topics");
            DropForeignKey("dbo.Topics", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Topics", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "Topic_Id" });
            DropIndex("dbo.Posts", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Topics", new[] { "Category_Id" });
            DropIndex("dbo.Topics", new[] { "Author_Id" });
            DropTable("dbo.Posts");
            DropTable("dbo.Topics");
            DropTable("dbo.Categories");
        }
    }
}
