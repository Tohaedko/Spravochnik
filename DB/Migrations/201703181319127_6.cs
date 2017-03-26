namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceXServices", "Service_Id", c => c.Long());
            CreateIndex("dbo.ServiceXServices", "Service_Id");
            AddForeignKey("dbo.ServiceXServices", "Service_Id", "dbo.Services", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceXServices", "Service_Id", "dbo.Services");
            DropIndex("dbo.ServiceXServices", new[] { "Service_Id" });
            DropColumn("dbo.ServiceXServices", "Service_Id");
        }
    }
}
