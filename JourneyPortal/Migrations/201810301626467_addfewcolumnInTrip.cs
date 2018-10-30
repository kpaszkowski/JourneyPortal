namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfewcolumnInTrip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "Duration", c => c.Int(nullable: false));
            AddColumn("dbo.Trips", "DurationTraffic", c => c.Int(nullable: false));
            DropColumn("dbo.Trips", "NumberOfDays");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trips", "NumberOfDays", c => c.Int(nullable: false));
            DropColumn("dbo.Trips", "DurationTraffic");
            DropColumn("dbo.Trips", "Duration");
        }
    }
}
