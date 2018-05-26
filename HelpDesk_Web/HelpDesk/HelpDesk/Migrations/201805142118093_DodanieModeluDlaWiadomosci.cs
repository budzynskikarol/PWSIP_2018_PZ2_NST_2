namespace HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieModeluDlaWiadomosci : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Wiadomoscis",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Tresc = c.String(nullable: false),
                        Nadawca = c.String(),
                        ZgloszeniaId = c.Int(nullable: false),
                        DataDodania = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Zgloszenias", t => t.ZgloszeniaId, cascadeDelete: true)
                .Index(t => t.ZgloszeniaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Wiadomoscis", "ZgloszeniaId", "dbo.Zgloszenias");
            DropIndex("dbo.Wiadomoscis", new[] { "ZgloszeniaId" });
            DropTable("dbo.Wiadomoscis");
        }
    }
}
