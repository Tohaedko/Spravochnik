namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MedicalServiceGroups", "SiteCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MedicalServiceGroups", "SiteCode");
        }
    }
}
