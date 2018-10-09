namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createOfferComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OffersComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Like = c.Int(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
            AddColumn("dbo.Offers", "Rate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OffersComments", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.OffersComments", new[] { "Author_Id" });
            DropColumn("dbo.Offers", "Rate");
            DropTable("dbo.OffersComments");
        }
    }
}
