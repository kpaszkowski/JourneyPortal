namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCreationDateField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "CreationDate");
        }
    }
}
