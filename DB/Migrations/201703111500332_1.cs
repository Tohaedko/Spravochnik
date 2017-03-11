namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Localizations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 2),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocationXLocalizations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LocationName = c.String(),
                        Localization_Id = c.Long(),
                        Location_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Localizations", t => t.Localization_Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Localization_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PriceLists",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Workingdays = c.Int(),
                        Location_Id = c.Long(),
                        Services_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .ForeignKey("dbo.Services", t => t.Services_Id)
                .Index(t => t.Location_Id)
                .Index(t => t.Services_Id);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        IsDiscountAllowed = c.Boolean(),
                        Location_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.ServiceXMedicalServiceGroups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MedicalServiceGroups_Id = c.Long(),
                        Service_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MedicalServiceGroups", t => t.MedicalServiceGroups_Id)
                .ForeignKey("dbo.Services", t => t.Service_Id)
                .Index(t => t.MedicalServiceGroups_Id)
                .Index(t => t.Service_Id);
            
            CreateTable(
                "dbo.MedicalServiceGroups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        ParentGroupId_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MedicalServiceGroups", t => t.ParentGroupId_Id)
                .Index(t => t.ParentGroupId_Id);
            
            CreateTable(
                "dbo.MedicalServiceGroupsXLocalizations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MedicalServiceGroupsName = c.String(),
                        Localization_Id = c.Long(),
                        MedicalServiceGroups_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Localizations", t => t.Localization_Id)
                .ForeignKey("dbo.MedicalServiceGroups", t => t.MedicalServiceGroups_Id)
                .Index(t => t.Localization_Id)
                .Index(t => t.MedicalServiceGroups_Id);
            
            CreateTable(
                "dbo.ServiceXLocalizations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ServiceName = c.String(),
                        Remark = c.String(),
                        Keywords = c.String(),
                        Description = c.String(storeType: "ntext"),
                        Localization_Id = c.Long(),
                        Services_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Localizations", t => t.Localization_Id)
                .ForeignKey("dbo.Services", t => t.Services_Id)
                .Index(t => t.Localization_Id)
                .Index(t => t.Services_Id);
            
            CreateTable(
                "dbo.ServiceXServices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ComponentServiceId_Id = c.Long(),
                        ParentServiceId_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.ComponentServiceId_Id)
                .ForeignKey("dbo.Services", t => t.ParentServiceId_Id)
                .Index(t => t.ComponentServiceId_Id)
                .Index(t => t.ParentServiceId_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceXServices", "ParentServiceId_Id", "dbo.Services");
            DropForeignKey("dbo.ServiceXServices", "ComponentServiceId_Id", "dbo.Services");
            DropForeignKey("dbo.ServiceXLocalizations", "Services_Id", "dbo.Services");
            DropForeignKey("dbo.ServiceXLocalizations", "Localization_Id", "dbo.Localizations");
            DropForeignKey("dbo.MedicalServiceGroupsXLocalizations", "MedicalServiceGroups_Id", "dbo.MedicalServiceGroups");
            DropForeignKey("dbo.MedicalServiceGroupsXLocalizations", "Localization_Id", "dbo.Localizations");
            DropForeignKey("dbo.ServiceXMedicalServiceGroups", "Service_Id", "dbo.Services");
            DropForeignKey("dbo.ServiceXMedicalServiceGroups", "MedicalServiceGroups_Id", "dbo.MedicalServiceGroups");
            DropForeignKey("dbo.MedicalServiceGroups", "ParentGroupId_Id", "dbo.MedicalServiceGroups");
            DropForeignKey("dbo.PriceLists", "Services_Id", "dbo.Services");
            DropForeignKey("dbo.Services", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.PriceLists", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.LocationXLocalizations", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.LocationXLocalizations", "Localization_Id", "dbo.Localizations");
            DropIndex("dbo.ServiceXServices", new[] { "ParentServiceId_Id" });
            DropIndex("dbo.ServiceXServices", new[] { "ComponentServiceId_Id" });
            DropIndex("dbo.ServiceXLocalizations", new[] { "Services_Id" });
            DropIndex("dbo.ServiceXLocalizations", new[] { "Localization_Id" });
            DropIndex("dbo.MedicalServiceGroupsXLocalizations", new[] { "MedicalServiceGroups_Id" });
            DropIndex("dbo.MedicalServiceGroupsXLocalizations", new[] { "Localization_Id" });
            DropIndex("dbo.MedicalServiceGroups", new[] { "ParentGroupId_Id" });
            DropIndex("dbo.ServiceXMedicalServiceGroups", new[] { "Service_Id" });
            DropIndex("dbo.ServiceXMedicalServiceGroups", new[] { "MedicalServiceGroups_Id" });
            DropIndex("dbo.Services", new[] { "Location_Id" });
            DropIndex("dbo.PriceLists", new[] { "Services_Id" });
            DropIndex("dbo.PriceLists", new[] { "Location_Id" });
            DropIndex("dbo.LocationXLocalizations", new[] { "Location_Id" });
            DropIndex("dbo.LocationXLocalizations", new[] { "Localization_Id" });
            DropTable("dbo.ServiceXServices");
            DropTable("dbo.ServiceXLocalizations");
            DropTable("dbo.MedicalServiceGroupsXLocalizations");
            DropTable("dbo.MedicalServiceGroups");
            DropTable("dbo.ServiceXMedicalServiceGroups");
            DropTable("dbo.Services");
            DropTable("dbo.PriceLists");
            DropTable("dbo.Locations");
            DropTable("dbo.LocationXLocalizations");
            DropTable("dbo.Localizations");
        }
    }
}
