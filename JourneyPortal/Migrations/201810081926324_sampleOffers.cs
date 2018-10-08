namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sampleOffers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        NuberOfBooking = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TravelAgencyOwnerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.TravelAgencyOwnerId)
                .Index(t => t.TravelAgencyOwnerId);
            
            CreateTable(
                "dbo.OffersApplicationUsers",
                c => new
                    {
                        Offers_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Offers_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Offers", t => t.Offers_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Offers_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "TravelAgencyOwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OffersApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OffersApplicationUsers", "Offers_Id", "dbo.Offers");
            DropIndex("dbo.OffersApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OffersApplicationUsers", new[] { "Offers_Id" });
            DropIndex("dbo.Offers", new[] { "TravelAgencyOwnerId" });
            DropTable("dbo.OffersApplicationUsers");
            DropTable("dbo.Offers");
        }
    }
}
