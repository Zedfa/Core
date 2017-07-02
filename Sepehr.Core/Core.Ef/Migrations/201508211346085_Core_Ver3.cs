namespace Core.Ef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Core_Ver3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "core.ConstantCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "core.Constants",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false),
                        Value = c.String(nullable: false),
                        ConstantCategoryID = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("core.ConstantCategories", t => t.ConstantCategoryID, cascadeDelete: true)
                .Index(t => t.ConstantCategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("core.Constants", "ConstantCategoryID", "core.ConstantCategories");
            DropIndex("core.Constants", new[] { "ConstantCategoryID" });
            DropTable("core.Constants");
            DropTable("core.ConstantCategories");
        }
    }
}
