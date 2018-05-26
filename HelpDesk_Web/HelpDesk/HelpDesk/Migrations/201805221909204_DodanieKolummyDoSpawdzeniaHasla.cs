namespace HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieKolummyDoSpawdzeniaHasla : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ChangedPassword", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ChangedPassword");
        }
    }
}
