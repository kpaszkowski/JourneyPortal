namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTripModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Atractions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.String(),
                    Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TimeOfSightseeing = c.Int(nullable: false),
                    Name = c.String(),
                    Description = c.String(),
                    Rate = c.Double(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                    X = c.Double(nullable: false),
                    Y = c.Double(nullable: false),
                    OwnerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Hotels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CostPerNight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                        Description = c.String(),
                        Rate = c.Double(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        OwnerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BaseHotelId = c.Int(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                        TotalDistance = c.Double(nullable: false),
                        NumberOfDays = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hotels", t => t.BaseHotelId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.BaseHotelId)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.AtractionTrips",
                c => new
                    {
                        Atraction_Id = c.Int(nullable: false),
                        Trip_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Atraction_Id, t.Trip_Id })
                .ForeignKey("dbo.Atractions", t => t.Atraction_Id, cascadeDelete: true)
                .ForeignKey("dbo.Trips", t => t.Trip_Id, cascadeDelete: true)
                .Index(t => t.Atraction_Id)
                .Index(t => t.Trip_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AtractionTrips", "Trip_Id", "dbo.Trips");
            DropForeignKey("dbo.AtractionTrips", "Atraction_Id", "dbo.Atractions");
            DropForeignKey("dbo.Trips", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trips", "BaseHotelId", "dbo.Hotels");
            DropForeignKey("dbo.Hotels", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Atractions", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.AtractionTrips", new[] { "Trip_Id" });
            DropIndex("dbo.AtractionTrips", new[] { "Atraction_Id" });
            DropIndex("dbo.Trips", new[] { "CreatedById" });
            DropIndex("dbo.Trips", new[] { "BaseHotelId" });
            DropIndex("dbo.Hotels", new[] { "OwnerId" });
            DropIndex("dbo.Atractions", new[] { "OwnerId" });
            DropTable("dbo.AtractionTrips");
            DropTable("dbo.Trips");
            DropTable("dbo.Hotels");
            DropTable("dbo.Atractions");
        }
    }
}
