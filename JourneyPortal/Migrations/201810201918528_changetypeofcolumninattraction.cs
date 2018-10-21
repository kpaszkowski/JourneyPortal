namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetypeofcolumninattraction : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Atractions", "TimeOfSightseeing", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Atractions", "TimeOfSightseeing", c => c.DateTime());
        }
    }
}
