namespace HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieDatyDoZgloszen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Zgloszenias", "DataDodania", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Zgloszenias", "DataDodania");
        }
    }
}
