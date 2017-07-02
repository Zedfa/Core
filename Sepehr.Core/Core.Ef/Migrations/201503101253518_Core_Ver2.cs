namespace Core.Ef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Core_Ver2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("core.Logs", "LogType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("core.Logs", "LogType");
        }
    }
}
