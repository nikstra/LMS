namespace LMS.Migrations
{
    using LMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "LMS.Models.ApplicationDbContext";
        }

        protected override void Seed(LMS.Models.ApplicationDbContext context)
        {
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

            //context.Groups.AddOrUpdate(
            //    g => g.Name,
            //    new Group { Name = ".NET September 2015", StartDate = new DateTime(2015,09,23), EndDate = new DateTime(2016,03,18), Description = "Systemutvecklarkurs...." }
            //    );

            //if (!context.Users.Any(u => u.UserName == "sune@gmail.com"))
            //{
            //    //    var roleStore = new RoleStore<IdentityRole>(context);
            //    //    var roleManager = new RoleManager<IdentityRole>(roleStore);

            //    var store = new UserStore<ApplicationUser>(context);
            //    var manager = new UserManager<ApplicationUser>(store);
            //    var user = new ApplicationUser { UserName = "sune@gmail.com" , GroupId = 4};



            //    manager.Create(user, "password");
            //    //    roleManager.Create(new IdentityRole { Name = "admin" });
            //    //    manager.AddToRole(user.Id, "admin");
            //}
            
        }
    }
}
