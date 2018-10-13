namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offersRefactor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "Country", c => c.String());
            AlterColumn("dbo.Offers", "Name", c => c.String());
            AlterColumn("dbo.Offers", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Offers", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Offers", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Offers", "Country");
        }
    }
}
