namespace Core.Ef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Core_Ver1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "core.ExceptionLogs",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        ExceptionType = c.String(),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Source = c.String(),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        InnerException_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("core.ExceptionLogs", t => t.InnerException_ID)
                .Index(t => t.InnerException_ID);
            
            CreateTable(
                "core.Logs",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        UserId = c.String(),
                        CustomMessage = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        InnerExceptionCount = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ExceptionLog_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("core.ExceptionLogs", t => t.ExceptionLog_ID)
                .Index(t => t.ExceptionLog_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("core.Logs", "ExceptionLog_ID", "core.ExceptionLogs");
            DropForeignKey("core.ExceptionLogs", "InnerException_ID", "core.ExceptionLogs");
            DropIndex("core.Logs", new[] { "ExceptionLog_ID" });
            DropIndex("core.ExceptionLogs", new[] { "InnerException_ID" });
            DropTable("core.Logs");
            DropTable("core.ExceptionLogs");
        }
    }
}
