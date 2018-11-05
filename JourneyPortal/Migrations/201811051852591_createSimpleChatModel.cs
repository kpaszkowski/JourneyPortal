namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createSimpleChatModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GlobalMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GlobalMessages", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.GlobalMessages", new[] { "Author_Id" });
            DropTable("dbo.GlobalMessages");
        }
    }
}
