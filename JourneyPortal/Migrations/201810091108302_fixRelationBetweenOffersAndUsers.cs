namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixRelationBetweenOffersAndUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OffersApplicationUsers", "Offers_Id", "dbo.Offers");
            DropForeignKey("dbo.OffersApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.OffersApplicationUsers", new[] { "Offers_Id" });
            DropIndex("dbo.OffersApplicationUsers", new[] { "ApplicationUser_Id" });
            DropTable("dbo.OffersApplicationUsers");
            CreateTable(
                "dbo.OffersApplicationUsers",
                c => new
                    {
                        OfferId = c.Int(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        BookingCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OfferId, t.ApplicationUserId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Offers", t => t.OfferId, cascadeDelete: true)
                .Index(t => t.OfferId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OffersApplicationUsers");
            CreateTable(
                "dbo.OffersApplicationUsers",
                c => new
                    {
                        Offers_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Offers_Id, t.ApplicationUser_Id });
            
            DropForeignKey("dbo.OffersApplicationUsers", "OfferId", "dbo.Offers");
            DropForeignKey("dbo.OffersApplicationUsers", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.OffersApplicationUsers", new[] { "ApplicationUserId" });
            DropIndex("dbo.OffersApplicationUsers", new[] { "OfferId" });
            CreateIndex("dbo.OffersApplicationUsers", "ApplicationUser_Id");
            CreateIndex("dbo.OffersApplicationUsers", "Offers_Id");
            AddForeignKey("dbo.OffersApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OffersApplicationUsers", "Offers_Id", "dbo.Offers", "Id", cascadeDelete: true);
        }
    }
}
