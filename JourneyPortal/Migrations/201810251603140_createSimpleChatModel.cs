namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createSimpleChatModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstUserId = c.String(maxLength: 128),
                        SecondUserId = c.String(maxLength: 128),
                        Text = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FirstUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.SecondUserId)
                .Index(t => t.FirstUserId)
                .Index(t => t.SecondUserId);
            
            CreateTable(
                "dbo.GlobalMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        AuthorName = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatMessages", "SecondUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatMessages", "FirstUserId", "dbo.AspNetUsers");
            DropIndex("dbo.ChatMessages", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ChatMessages", new[] { "SecondUserId" });
            DropIndex("dbo.ChatMessages", new[] { "FirstUserId" });
            DropTable("dbo.GlobalMessages");
            DropTable("dbo.ChatMessages");
        }
    }
}
