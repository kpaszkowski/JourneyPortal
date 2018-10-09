namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeImage : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Offers", "Avatar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offers", "Avatar", c => c.Binary(storeType: "image"));
        }
    }
}
