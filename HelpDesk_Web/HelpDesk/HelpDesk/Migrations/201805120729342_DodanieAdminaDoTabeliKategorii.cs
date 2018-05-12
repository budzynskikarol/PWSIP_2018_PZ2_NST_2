namespace HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieAdminaDoTabeliKategorii : DbMigration
    {
        public override void Up()
        {
            Sql("Insert into Kategories(Nazwa) values ('admin')");

        }

        public override void Down()
        {
        }
    }
}
