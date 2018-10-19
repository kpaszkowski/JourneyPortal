namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeRateFieldOnOffers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Offers", "Rate", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Offers", "Rate", c => c.Int(nullable: false));
        }
    }
}
