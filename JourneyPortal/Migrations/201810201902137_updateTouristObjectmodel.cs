namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTouristObjectmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Atractions", "Image", c => c.String());
            AddColumn("dbo.Hotels", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Hotels", "Image");
            DropColumn("dbo.Atractions", "Image");
        }
    }
}
