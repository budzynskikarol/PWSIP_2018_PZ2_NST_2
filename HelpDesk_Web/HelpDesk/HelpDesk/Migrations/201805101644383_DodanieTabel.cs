namespace HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieTabel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Statusies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Zgloszenias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(),
                        Opis = c.String(),
                        Komentarz = c.String(),
                        StatusyId = c.Int(nullable: false),
                        KategorieId = c.Int(nullable: false),
                        Uzytkownik = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kategories", t => t.KategorieId, cascadeDelete: true)
                .ForeignKey("dbo.Statusies", t => t.StatusyId, cascadeDelete: true)
                .Index(t => t.StatusyId)
                .Index(t => t.KategorieId);
            
            AddColumn("dbo.AspNetUsers", "Imie", c => c.String());
            AddColumn("dbo.AspNetUsers", "Nazwisko", c => c.String());
            AddColumn("dbo.AspNetUsers", "KategorieId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "KategorieId");
            AddForeignKey("dbo.AspNetUsers", "KategorieId", "dbo.Kategories", "Id", cascadeDelete: true);

            Sql("Insert into Statusies(Nazwa) values ('Nowy')");
            Sql("Insert into Statusies(Nazwa) values ('Przyjêty do realizacji')");
            Sql("Insert into Statusies(Nazwa) values ('Zakoñczony')");
            Sql("Insert into Statusies(Nazwa) values ('Anulowany')");
            Sql("Insert into Kategories(Nazwa) values ('IT')");
            Sql("Insert into Kategories(Nazwa) values ('Ksiêgowoœæ')");
            Sql("Insert into Kategories(Nazwa) values ('Kadry')");
            Sql("Insert into Kategories(Nazwa) values ('Zamówienia Publiczne')");
            Sql("Insert into Kategories(Nazwa) values ('Infastruktura')");
            Sql("Insert into Kategories(Nazwa) values ('Administracja')");
            Sql("Insert into Kategories(Nazwa) values ('Inne')");
            Sql("Insert into Kategories(Nazwa) values ('U¿ytkownik')");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Zgloszenias", "StatusyId", "dbo.Statusies");
            DropForeignKey("dbo.Zgloszenias", "KategorieId", "dbo.Kategories");
            DropForeignKey("dbo.AspNetUsers", "KategorieId", "dbo.Kategories");
            DropIndex("dbo.Zgloszenias", new[] { "KategorieId" });
            DropIndex("dbo.Zgloszenias", new[] { "StatusyId" });
            DropIndex("dbo.AspNetUsers", new[] { "KategorieId" });
            DropColumn("dbo.AspNetUsers", "KategorieId");
            DropColumn("dbo.AspNetUsers", "Nazwisko");
            DropColumn("dbo.AspNetUsers", "Imie");
            DropTable("dbo.Zgloszenias");
            DropTable("dbo.Statusies");
            DropTable("dbo.Kategories");
        }
    }
}
