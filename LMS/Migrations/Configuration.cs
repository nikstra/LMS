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
            //AutomaticMigrationDataLossAllowed = true;
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
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            //    var user = new ApplicationUser { UserName = "sune@gmail.com" , GroupId = 4};



            //manager.Create(user, "password");
            string roleName = "teacher";
            if (!roleManager.RoleExists(roleName))
                roleManager.Create(new IdentityRole { Name = roleName });

            roleName = "student";
            if (!roleManager.RoleExists(roleName))
                roleManager.Create(new IdentityRole { Name = roleName });

            // Make sure to call ToList() to get a copy of users in memory, otherwise you'll get the following exception:
            //      System.InvalidOperationException: There is already an open DataReader associated with this Command which must be closed first.
            // Setting "MultipleActiveResultSets=true" in the connection string is not a proper solution for this issue.
            // http://devproconnections.com/development/solving-net-scalability-problem
            foreach (var user in context.Users.ToList())
            {
                if(!userManager.IsInRole(user.Id, "teacher"))
                    userManager.AddToRole(user.Id, "student");
            }

            ApplicationUser newUser;
            string userEmail = "principal.skinner@springfieldelementary.edu";
            if (userManager.FindByEmail(userEmail) == null)
            {
                newUser = new ApplicationUser { UserName = "Seymore", Email = userEmail, FirstName = "Seymore", LastName = "Skinner" };
                userManager.Create(newUser, "Abc_123");
                userManager.AddToRole(newUser.Id, "teacher");
            }

            userEmail = "edna.krabappel@springfieldelementary.edu";
            if (userManager.FindByEmail(userEmail) == null)
            {
                newUser = new ApplicationUser { UserName = "Edna", Email = userEmail, FirstName = "Edna", LastName = "Krabappel" };
                userManager.Create(newUser, "Abc_123");
                userManager.AddToRole(newUser.Id, "teacher");
            }

            userEmail = "superintendent.chalmers@springfieldelementary.edu";
            if (userManager.FindByEmail(userEmail) == null)
            {
                newUser = new ApplicationUser { UserName = "Gary", Email = userEmail, FirstName = "Gary", LastName = "Chalmers" };
                userManager.Create(newUser, "Abc_123");
                userManager.AddToRole(newUser.Id, "teacher");
            }

            userEmail = "elizabeth.hoover@springfieldelementary.edu";
            if (userManager.FindByEmail(userEmail) == null)
            {
                newUser = new ApplicationUser { UserName = "Elizabeth", Email = userEmail, FirstName = "Elizabeth", LastName = "Hoover" };
                userManager.Create(newUser, "Abc_123");
                userManager.AddToRole(newUser.Id, "teacher");
            }

            userEmail = "groundskeeper.willie@springfieldelementary.edu";
            if (userManager.FindByEmail(userEmail) == null)
            {
                newUser = new ApplicationUser { UserName = "William", Email = userEmail, FirstName = "William", LastName = "MacDougal" };
                userManager.Create(newUser, "Abc_123");
                userManager.AddToRole(newUser.Id, "teacher");
            }
            //}

        }
    }
}
