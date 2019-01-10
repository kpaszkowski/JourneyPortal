namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeImageStorage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Atractions", "Image_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Image_Id", c => c.Int());
            AddColumn("dbo.Hotels", "Image_Id", c => c.Int());
            AddColumn("dbo.Offers", "Image_Id", c => c.Int());
            AddColumn("dbo.Images", "Binary", c => c.Binary());
            CreateIndex("dbo.Atractions", "Image_Id");
            CreateIndex("dbo.AspNetUsers", "Image_Id");
            CreateIndex("dbo.Hotels", "Image_Id");
            CreateIndex("dbo.Offers", "Image_Id");
            AddForeignKey("dbo.Atractions", "Image_Id", "dbo.Images", "Id");
            AddForeignKey("dbo.Hotels", "Image_Id", "dbo.Images", "Id");
            AddForeignKey("dbo.AspNetUsers", "Image_Id", "dbo.Images", "Id");
            AddForeignKey("dbo.Offers", "Image_Id", "dbo.Images", "Id");
            DropColumn("dbo.Atractions", "Image");
            DropColumn("dbo.AspNetUsers", "Avatar");
            DropColumn("dbo.Hotels", "Image");
            DropColumn("dbo.Offers", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offers", "Image", c => c.String());
            AddColumn("dbo.Hotels", "Image", c => c.String());
            AddColumn("dbo.AspNetUsers", "Avatar", c => c.String());
            AddColumn("dbo.Atractions", "Image", c => c.String());
            DropForeignKey("dbo.Offers", "Image_Id", "dbo.Images");
            DropForeignKey("dbo.AspNetUsers", "Image_Id", "dbo.Images");
            DropForeignKey("dbo.Hotels", "Image_Id", "dbo.Images");
            DropForeignKey("dbo.Atractions", "Image_Id", "dbo.Images");
            DropIndex("dbo.Offers", new[] { "Image_Id" });
            DropIndex("dbo.Hotels", new[] { "Image_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Image_Id" });
            DropIndex("dbo.Atractions", new[] { "Image_Id" });
            DropColumn("dbo.Images", "Binary");
            DropColumn("dbo.Offers", "Image_Id");
            DropColumn("dbo.Hotels", "Image_Id");
            DropColumn("dbo.AspNetUsers", "Image_Id");
            DropColumn("dbo.Atractions", "Image_Id");
        }
    }
}
