namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImageToDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Avatar", c => c.Binary(storeType: "image"));
            AddColumn("dbo.Offers", "Avatar", c => c.Binary(storeType: "image"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "Avatar");
            DropColumn("dbo.AspNetUsers", "Avatar");
        }
    }
}
