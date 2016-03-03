namespace LMS.Migrations
{
    using LMS.Constants;
    using LMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RandomLib;
    using System;
    using System.Collections.Generic;
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

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!roleManager.RoleExists(LMSConstants.RoleTeacher))
                roleManager.Create(new IdentityRole { Name = LMSConstants.RoleTeacher });

            if (!roleManager.RoleExists(LMSConstants.RoleStudent))
                roleManager.Create(new IdentityRole { Name = LMSConstants.RoleStudent });

            // Make sure to call ToList() to get a copy of users in memory, otherwise you'll get the following exception:
            //      System.InvalidOperationException: There is already an open DataReader associated with this Command which must be closed first.
            // Setting "MultipleActiveResultSets=true" in the connection string is not a proper solution for this issue.
            // http://devproconnections.com/development/solving-net-scalability-problem
            foreach (var user in context.Users.ToList())
            {
                // Make all users that are not teachers, students.
                if(!userManager.IsInRole(user.Id, LMSConstants.RoleTeacher))
                    userManager.AddToRole(user.Id, LMSConstants.RoleStudent);
            }

            RandomGenerator randGen = new RandomGenerator();
            AddStudents(userManager, randGen.People(100), context.Users.Count());

            AddOrUpdateUser(
                userManager,
                new ApplicationUser {
                    /*UserName = "Seymore",*/
                    Email = "principal.skinner@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "Seymore",
                    LastName = "Skinner" },
                "Abc_123",
                LMSConstants.RoleTeacher
                );

            AddOrUpdateUser(
                userManager,
                new ApplicationUser {
                    /*UserName = "Edna",*/
                    Email = "edna.krabappel@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "Edna",
                    LastName = "Krabappel" },
                "Abc_123",
                LMSConstants.RoleTeacher
                );

            AddOrUpdateUser(
                userManager,
                new ApplicationUser {
                    /*UserName = "Gary",*/
                    Email = "superintendent.chalmers@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "Gary",
                    LastName = "Chalmers" },
                "Abc_123",
                LMSConstants.RoleTeacher
                );

            AddOrUpdateUser(
                userManager,
                new ApplicationUser {
                    /*UserName = "Elizabeth",*/
                    Email = "elizabeth.hoover@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "Elizabeth",
                    LastName = "Hoover" },
                "Abc_123",
                LMSConstants.RoleTeacher
                );

            AddOrUpdateUser(
                userManager,
                new ApplicationUser {
                    /*UserName = "William",*/
                    Email = "groundskeeper.willie@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "William",
                    LastName = "MacDougal" },
                "Abc_123",
                LMSConstants.RoleTeacher
                );
        }

        /// <summary>
        /// Adds students.
        /// </summary>
        /// <param name="userManager">An instance of UserManager.</param>
        /// <param name="studentNames">A List of first name / last name tuples.</param>
        /// <param name="maxUserCount">Maxumum number of users you want in the database.</param>
        private static void AddStudents(UserManager<ApplicationUser> userManager, List<Tuple<string, string>> studentNames, int maxUserCount)
        {
            // Don't seed if we already have users.
            if(studentNames.Count() < maxUserCount)
                return;

            foreach (Tuple<string, string> studentName in studentNames)
            {
                AddOrUpdateUser(
                    userManager,
                    new ApplicationUser
                    {
                        /*UserName = studentName.Item1,*/
                        Email = studentName.Item1 + "." + studentName.Item2 + "@springfieldelementary.edu",
                        PhoneNumber = "070-123456",
                        FirstName = studentName.Item1,
                        LastName = studentName.Item2
                    },
                    "Abc_123",
                    LMSConstants.RoleStudent
                    );
            }
        }

        /// <summary>
        /// Add an ApplicationUser unless a user with the same email address already exist.
        /// </summary>
        /// <param name="userManager">An instance of a UserManager.</param>
        /// <param name="newUser">An instance of the ApplicationUser to be created.</param>
        /// <param name="password">A plain text password.</param>
        /// <param name="role">The role for the new user.</param>
        /// <returns></returns>
        private static void AddOrUpdateUser(UserManager<ApplicationUser> userManager, ApplicationUser newUser, string password, string role)
        {
            // Set UserName to Email to get the same behaviour as in the AccountController.Register() method.
            newUser.UserName = newUser.Email;

            ApplicationUser appUser = userManager.FindByEmail(newUser.Email);

            if (appUser == null)
            {
                IdentityResult result = userManager.Create(newUser, password);
                if(result.Succeeded)
                    userManager.AddToRole(newUser.Id, role);
            }
        }
    }
}
