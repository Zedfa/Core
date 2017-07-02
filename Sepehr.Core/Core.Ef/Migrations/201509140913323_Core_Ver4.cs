namespace Core.Ef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Core_Ver4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("core.Constants", "ConstantCategoryID", "core.ConstantCategories");
            CreateTable(
                "core.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Title = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 12),
                        Address = c.String(maxLength: 100),
                        Code = c.String(maxLength: 10),
                        Family = c.String(maxLength: 50),
                        FatherName = c.String(maxLength: 50),
                        NationalId = c.String(maxLength: 10),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "core.CompanyRoles",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CurrentCompanyId = c.Int(),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.RoleId })
                .ForeignKey("core.Companies", t => t.CompanyId)
                .ForeignKey("core.Roles", t => t.RoleId)
                .Index(t => t.CompanyId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "core.Roles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsCompanyRole = c.Boolean(nullable: false),
                        CurrentCompanyId = c.Int(),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "core.CompanyChartRoles",
                c => new
                    {
                        CompanyChartId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.CompanyChartId, t.RoleId })
                .ForeignKey("core.CompanyCharts", t => t.CompanyChartId)
                .ForeignKey("core.Roles", t => t.RoleId)
                .Index(t => t.CompanyChartId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "core.CompanyCharts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        ParentId = c.Int(),
                        Level = c.Short(),
                        Lineage = c.String(),
                        Depth = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.CompanyCharts", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "core.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleID = c.Int(nullable: false),
                        CurrentCompanyId = c.Int(),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleID })
                .ForeignKey("core.Roles", t => t.RoleID, cascadeDelete: true)
                .ForeignKey("core.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleID);
            
            CreateTable(
                "core.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FName = c.String(nullable: false, maxLength: 50),
                        LName = c.String(maxLength: 50),
                        CompanyChartId = c.Int(),
                        Active = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 40),
                        Count = c.Int(nullable: false),
                        CurrentCompanyId = c.Int(),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.CompanyCharts", t => t.CompanyChartId)
                .Index(t => t.CompanyChartId);
            
            CreateTable(
                "core.UserConfigs",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ConfigKey = c.String(nullable: false, maxLength: 50),
                        ConfigValue = c.String(maxLength: 50),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.UserId, t.ConfigKey })
                .ForeignKey("core.Configs", t => t.ConfigKey)
                .ForeignKey("core.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ConfigKey);
            
            CreateTable(
                "core.Configs",
                c => new
                    {
                        ConfigKey = c.String(nullable: false, maxLength: 50),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ConfigKey);
            
            CreateTable(
                "core.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.Users", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            CreateTable(
                "core.ViewElementRoles",
                c => new
                    {
                        ViewElementId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.ViewElementId, t.RoleId })
                .ForeignKey("core.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("core.ViewElements", t => t.ViewElementId, cascadeDelete: true)
                .Index(t => t.ViewElementId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "core.ViewElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueName = c.String(nullable: false, maxLength: 200),
                        Title = c.String(nullable: false, maxLength: 100),
                        ElementType = c.Int(nullable: false),
                        XMLViewData = c.String(),
                        ParentId = c.Int(),
                        IsHidden = c.Boolean(nullable: false),
                        InVisible = c.Boolean(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.ViewElements", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "core.CompanyViewElements",
                c => new
                    {
                        ViewElementId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.ViewElementId, t.CompanyId })
                .ForeignKey("core.Companies", t => t.CompanyId)
                .ForeignKey("core.ViewElements", t => t.ViewElementId)
                .Index(t => t.ViewElementId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "core.TableInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Caption = c.String(),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "core.UserLoggeds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ip = c.String(),
                        TableNameId = c.Int(),
                        RecordId = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        LogType = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        TableInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.TableInfoes", t => t.TableInfo_Id)
                .ForeignKey("core.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TableInfo_Id);
            
            AddForeignKey("core.Constants", "ConstantCategoryID", "core.ConstantCategories", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("core.Constants", "ConstantCategoryID", "core.ConstantCategories");
            DropForeignKey("core.UserLoggeds", "UserId", "core.Users");
            DropForeignKey("core.UserLoggeds", "TableInfo_Id", "core.TableInfoes");
            DropForeignKey("core.ViewElementRoles", "ViewElementId", "core.ViewElements");
            DropForeignKey("core.CompanyViewElements", "ViewElementId", "core.ViewElements");
            DropForeignKey("core.CompanyViewElements", "CompanyId", "core.Companies");
            DropForeignKey("core.ViewElements", "ParentId", "core.ViewElements");
            DropForeignKey("core.ViewElementRoles", "RoleId", "core.Roles");
            DropForeignKey("core.UserRoles", "UserId", "core.Users");
            DropForeignKey("core.UserProfiles", "Id", "core.Users");
            DropForeignKey("core.UserConfigs", "UserId", "core.Users");
            DropForeignKey("core.UserConfigs", "ConfigKey", "core.Configs");
            DropForeignKey("core.Users", "CompanyChartId", "core.CompanyCharts");
            DropForeignKey("core.UserRoles", "RoleID", "core.Roles");
            DropForeignKey("core.CompanyRoles", "RoleId", "core.Roles");
            DropForeignKey("core.CompanyChartRoles", "RoleId", "core.Roles");
            DropForeignKey("core.CompanyChartRoles", "CompanyChartId", "core.CompanyCharts");
            DropForeignKey("core.CompanyCharts", "ParentId", "core.CompanyCharts");
            DropForeignKey("core.CompanyRoles", "CompanyId", "core.Companies");
            DropIndex("core.UserLoggeds", new[] { "TableInfo_Id" });
            DropIndex("core.UserLoggeds", new[] { "UserId" });
            DropIndex("core.CompanyViewElements", new[] { "CompanyId" });
            DropIndex("core.CompanyViewElements", new[] { "ViewElementId" });
            DropIndex("core.ViewElements", new[] { "ParentId" });
            DropIndex("core.ViewElementRoles", new[] { "RoleId" });
            DropIndex("core.ViewElementRoles", new[] { "ViewElementId" });
            DropIndex("core.UserProfiles", new[] { "Id" });
            DropIndex("core.UserConfigs", new[] { "ConfigKey" });
            DropIndex("core.UserConfigs", new[] { "UserId" });
            DropIndex("core.Users", new[] { "CompanyChartId" });
            DropIndex("core.UserRoles", new[] { "RoleID" });
            DropIndex("core.UserRoles", new[] { "UserId" });
            DropIndex("core.CompanyCharts", new[] { "ParentId" });
            DropIndex("core.CompanyChartRoles", new[] { "RoleId" });
            DropIndex("core.CompanyChartRoles", new[] { "CompanyChartId" });
            DropIndex("core.CompanyRoles", new[] { "RoleId" });
            DropIndex("core.CompanyRoles", new[] { "CompanyId" });
            DropTable("core.UserLoggeds");
            DropTable("core.TableInfoes");
            DropTable("core.CompanyViewElements");
            DropTable("core.ViewElements");
            DropTable("core.ViewElementRoles");
            DropTable("core.UserProfiles");
            DropTable("core.Configs");
            DropTable("core.UserConfigs");
            DropTable("core.Users");
            DropTable("core.UserRoles");
            DropTable("core.CompanyCharts");
            DropTable("core.CompanyChartRoles");
            DropTable("core.Roles");
            DropTable("core.CompanyRoles");
            DropTable("core.Companies");
            AddForeignKey("core.Constants", "ConstantCategoryID", "core.ConstantCategories", "ID", cascadeDelete: true);
        }
    }
}
