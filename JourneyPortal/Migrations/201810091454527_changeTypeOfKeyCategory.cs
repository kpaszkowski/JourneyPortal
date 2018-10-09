namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTypeOfKeyCategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Topics", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Topics", new[] { "Category_Id" });
            DropPrimaryKey("dbo.Categories");
            AlterColumn("dbo.Categories", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Topics", "Category_Id", c => c.Int());
            AddPrimaryKey("dbo.Categories", "Id");
            CreateIndex("dbo.Topics", "Category_Id");
            AddForeignKey("dbo.Topics", "Category_Id", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Topics", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Topics", new[] { "Category_Id" });
            DropPrimaryKey("dbo.Categories");
            AlterColumn("dbo.Topics", "Category_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Categories", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Categories", "Id");
            CreateIndex("dbo.Topics", "Category_Id");
            AddForeignKey("dbo.Topics", "Category_Id", "dbo.Categories", "Id");
        }
    }
}
