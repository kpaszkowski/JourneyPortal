namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixmchatmodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChatMessages", "FirstUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatMessages", "SecondUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatMessages", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ChatMessages", new[] { "FirstUserId" });
            DropIndex("dbo.ChatMessages", new[] { "SecondUserId" });
            DropIndex("dbo.ChatMessages", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.Conversations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstUserId = c.String(maxLength: 128),
                    SecondUserId = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FirstUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.SecondUserId)
                .Index(t => t.FirstUserId)
                .Index(t => t.SecondUserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        AuthorName = c.String(),
                        AuthorImage = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Conversation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conversations", t => t.Conversation_Id)
                .Index(t => t.Conversation_Id);
            
            DropTable("dbo.ChatMessages");
        }
        
        public override void Down()
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
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Conversations", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversations", "SecondUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Conversation_Id", "dbo.Conversations");
            DropForeignKey("dbo.Conversations", "FirstUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "Conversation_Id" });
            DropIndex("dbo.Conversations", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Conversations", new[] { "SecondUserId" });
            DropIndex("dbo.Conversations", new[] { "FirstUserId" });
            DropTable("dbo.Messages");
            DropTable("dbo.Conversations");
            CreateIndex("dbo.ChatMessages", "ApplicationUser_Id");
            CreateIndex("dbo.ChatMessages", "SecondUserId");
            CreateIndex("dbo.ChatMessages", "FirstUserId");
            AddForeignKey("dbo.ChatMessages", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ChatMessages", "SecondUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ChatMessages", "FirstUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
