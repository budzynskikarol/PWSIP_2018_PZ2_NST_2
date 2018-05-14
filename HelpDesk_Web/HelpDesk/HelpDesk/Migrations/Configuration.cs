namespace HelpDesk.Migrations
{
    using HelpDesk.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HelpDesk.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HelpDesk.Models.ApplicationDbContext context)
        {
            SeedAccountAdmin(context, "admin@admin.pl", "Adam", "Szyszkowski", "!QAZ2wsx");
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }

        private void SeedAccountAdmin(ApplicationDbContext context, string email, string firstname, string lastname, string password)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any())
            {
                var Admin = new ApplicationUser
                {
                    Imie = firstname,
                    Nazwisko = lastname,
                    Email = email,
                    UserName = email,
                    //ChangePasswordDate = DateTime.Now.AddDays(-1),
                    KategorieId = 9
                };

                var wynik = userManager.Create(Admin, password);
            }
        }
    }
}
