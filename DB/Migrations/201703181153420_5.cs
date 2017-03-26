namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "Location_Id", "dbo.Locations");
            DropIndex("dbo.Services", new[] { "Location_Id" });
            DropColumn("dbo.Services", "Location_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "Location_Id", c => c.Long());
            CreateIndex("dbo.Services", "Location_Id");
            AddForeignKey("dbo.Services", "Location_Id", "dbo.Locations", "Id");
        }
    }
}
