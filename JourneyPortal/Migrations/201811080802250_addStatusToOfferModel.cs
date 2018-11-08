namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatusToOfferModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OffersApplicationUsers", "Status", c => c.String(nullable:false,defaultValue:"Niezaakceptowany"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OffersApplicationUsers", "Status");
        }
    }
}
