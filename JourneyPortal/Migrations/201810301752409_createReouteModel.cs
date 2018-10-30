namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createReouteModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TripId = c.Int(nullable: false),
                        StartX = c.Double(nullable: false),
                        EndX = c.Double(nullable: false),
                        StartY = c.Double(nullable: false),
                        EndY = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Routes", "TripId", "dbo.Trips");
            DropIndex("dbo.Routes", new[] { "TripId" });
            DropTable("dbo.Routes");
        }
    }
}
