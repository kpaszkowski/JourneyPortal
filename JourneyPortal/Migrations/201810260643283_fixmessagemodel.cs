namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixmessagemodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Author_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Messages", "Author_Id");
            AddForeignKey("dbo.Messages", "Author_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Messages", "AuthorName");
            DropColumn("dbo.Messages", "AuthorImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "AuthorImage", c => c.String());
            AddColumn("dbo.Messages", "AuthorName", c => c.String());
            DropForeignKey("dbo.Messages", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "Author_Id" });
            DropColumn("dbo.Messages", "Author_Id");
        }
    }
}
