namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLinkBetweenOffersAndCOmments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OffersComments", "Rate", c => c.Int(nullable: false));
            AddColumn("dbo.OffersComments", "Offers_Id", c => c.Int());
            CreateIndex("dbo.OffersComments", "Offers_Id");
            AddForeignKey("dbo.OffersComments", "Offers_Id", "dbo.Offers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OffersComments", "Offers_Id", "dbo.Offers");
            DropIndex("dbo.OffersComments", new[] { "Offers_Id" });
            DropColumn("dbo.OffersComments", "Offers_Id");
            DropColumn("dbo.OffersComments", "Rate");
        }
    }
}
