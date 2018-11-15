namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascadedelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OffersComments", "Offers_Id", "dbo.Offers");
            AddColumn("dbo.OffersComments", "Offers_Id1", c => c.Int());
            CreateIndex("dbo.OffersComments", "Offers_Id1");
            AddForeignKey("dbo.OffersComments", "Offers_Id1", "dbo.Offers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OffersComments", "Offers_Id1", "dbo.Offers");
            DropIndex("dbo.OffersComments", new[] { "Offers_Id1" });
            DropColumn("dbo.OffersComments", "Offers_Id1");
            AddForeignKey("dbo.OffersComments", "Offers_Id", "dbo.Offers", "Id");
        }
    }
}
