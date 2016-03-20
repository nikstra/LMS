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
    using System.Globalization;
    using System.Linq;
    using Extensions;

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

            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            // http://stackoverflow.com/questions/25702693/how-do-i-delete-all-data-in-the-seed-method
            // Deletes all data, from all tables, except for __MigrationHistory
            //context.Database.ExecuteSqlCommand("sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            //context.Database.ExecuteSqlCommand("sp_MSForEachTable 'IF OBJECT_ID(''?'') NOT IN (ISNULL(OBJECT_ID(''[dbo].[__MigrationHistory]''),0)) DELETE FROM ?'");
            //context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");


            // groupStartDate should not be using DateTime.Now....
            DateTime groupStartDate = new DateTime(2016, 3, 16).StartOfNextWeek(DayOfWeek.Monday).Date;

            Group newGroup = AddOrUpdateGroup(context, groupStartDate, 60, ".Net", "Påbyggnadskurs ASP.NET och C#");

            Activity newActivity;
            Course newCourse;
            newCourse = AppendOrUpdateCourse(context, newGroup, groupStartDate, 5, "C# grunder", "Grunderna i programmeringsspråket C#");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Lecture, groupStartDate, 2, "Föreläsning C#", "Föreläsning och codealong där vi lär oss grunderna i Visual Studio och C#");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Elearning, newActivity.EndDate.AddWorkDays(1), 2, "C# Fundamentals", "E-learning som går igenom variabeldeklarationer och flödeskontroll i C#");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Codealong, newActivity.EndDate.AddWorkDays(1), 1, "C# The Next Step", "Mer om programmering i C#");

            newCourse = AppendOrUpdateCourse(context, newGroup, newActivity.EndDate.AddWorkDays(1), 5, "Databas, grund", "Vi lär oss Entity Framework och Linq");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Elearning, newCourse.StartDate, 3, "Entity Framework 6", "Hantering av EF och code first");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Elearning, newActivity.EndDate.AddWorkDays(1), 2, "Linq", "Databasfrågor med Linq");

            newCourse = AppendOrUpdateCourse(context, newGroup, newActivity.EndDate.AddWorkDays(1), 5, "Webb frontend", "Html, Javascript och css");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Lecture, newCourse.StartDate, 3, "HTML 5", "Skapa webbsidor med den senaste HTML versionen");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Excercise, newActivity.EndDate.AddWorkDays(1), 2, "Javascript och AngularJS", "Skapa interaktivitet med olika skripttekniker");

            newCourse = AppendOrUpdateCourse(context, newGroup, newActivity.EndDate.AddWorkDays(1), 5, "Webb frontend 2", "Avancerade frontendlösningar");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Elearning, newCourse.StartDate, 2, "Bootstrap 3", "Designa webbsidor med Bootstrap");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Lecture, newActivity.EndDate.AddWorkDays(1), 3, "JQuery", "Manipulera DOM med JQuery");

            newCourse = AppendOrUpdateCourse(context, newGroup, newActivity.EndDate.AddWorkDays(1), 10, "C# fortsättning", "Fortsättning på C#-kursen");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Lecture, newCourse.StartDate, 2, "C#, använda SOLID", "Föreläsning om SOLID-principen och C#");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Excercise, newActivity.EndDate.AddWorkDays(1), 5, "Garage", "Skriv ett program.");

            // We want to be in the .Net group ;)
            int ourDevTeamGroupId = newGroup.Id;


            ///////////////////////////////
            groupStartDate = new DateTime(2016, 4, 10).StartOfNextWeek(DayOfWeek.Monday).Date;

            newGroup = AddOrUpdateGroup(context, groupStartDate, 60, "Java", "Påbyggnadskurs webbutveckling i Java");

            newCourse = AppendOrUpdateCourse(context, newGroup, groupStartDate, 5, "Java grunder", "Grunderna i programmeringsspråket Java");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Lecture, groupStartDate, 2, "Föreläsning Java", loremIpsum[0]);
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Elearning, newActivity.EndDate.AddWorkDays(1), 2, "Java Fundamentals", "E-learning som går igenom variabeldeklarationer och flödeskontroll i Java");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Codealong, newActivity.EndDate.AddWorkDays(1), 1, "Java The Next Step", "Mer om programmering i Java");

            newCourse = AppendOrUpdateCourse(context, newGroup, newActivity.EndDate.AddWorkDays(1), 5, "Databas, grund", loremIpsum[0]);
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Elearning, newCourse.StartDate, 3, "Spring Framework", loremIpsum[0]);
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Elearning, newActivity.EndDate.AddWorkDays(1), 2, "SQL", "Databasfrågor med SQL");

            newCourse = AppendOrUpdateCourse(context, newGroup, newActivity.EndDate.AddWorkDays(1), 5, "Webb frontend", "Html, Javascript och css");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Lecture, newCourse.StartDate, 3, "HTML 5", "Skapa webbsidor med den senaste HTML versionen");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Excercise, newActivity.EndDate.AddWorkDays(1), 2, "Javascript och AngularJS", "Skapa interaktivitet med olika skripttekniker");

            newCourse = AppendOrUpdateCourse(context, newGroup, newActivity.EndDate.AddWorkDays(1), 5, "Webb frontend 2", "Avancerade frontendlösningar");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Elearning, newCourse.StartDate, 2, "Bootstrap 3", "Designa webbsidor med Bootstrap");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Lecture, newActivity.EndDate.AddWorkDays(1), 3, "JQuery", "Manipulera DOM med JQuery");

            newCourse = AppendOrUpdateCourse(context, newGroup, newActivity.EndDate.AddWorkDays(1), 10, "Java fortsättning", "Fortsättning på Java-kursen");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Lecture, newCourse.StartDate, 2, "Java, använda SOLID", "Föreläsning om SOLID-principen och Java");
            newActivity = AppendOrUpdateActivity(context, newCourse, ActivityType.Excercise, newActivity.EndDate.AddWorkDays(1), 5, "Garage", "Skriv ett program.");
            //////////////////////////////

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
                if (!userManager.IsInRole(user.Id, LMSConstants.RoleTeacher))
                    userManager.AddToRole(user.Id, LMSConstants.RoleStudent);
            }

            RandomGenerator randGen = new RandomGenerator();

            List<int> groupIds = context.Groups
                .Select(g => g.Id)
                .Distinct()
                .ToList();

            AddStudents(userManager, randGen.People(50), groupIds, context.Users.Count());

            AddOrUpdateUser(
                userManager,
                new ApplicationUser
                {
                    /*UserName = "Robert",*/
                    Email = "robert@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "Robert",
                    LastName = "Dahlin",
                    GroupId = ourDevTeamGroupId
                },
                "Abc_123",
                LMSConstants.RoleStudent
                );

            AddOrUpdateUser(
                userManager,
                new ApplicationUser
                {
                    /*UserName = "Oscar",*/
                    Email = "oscar@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "Oscar",
                    LastName = "Urbina",
                    GroupId = ourDevTeamGroupId
                },
                "Abc_123",
                LMSConstants.RoleStudent
                );

            AddOrUpdateUser(
                userManager,
                new ApplicationUser
                {
                    /*UserName = "Niklas",*/
                    Email = "niklas@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "Niklas",
                    LastName = "Strand",
                    GroupId = ourDevTeamGroupId
                },
                "Abc_123",
                LMSConstants.RoleStudent
                );

            // Some teachers
            AddOrUpdateUser(
                userManager,
                new ApplicationUser
                {
                    /*UserName = "Seymore",*/
                    Email = "principal.skinner@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "Seymore",
                    LastName = "Skinner"
                },
                "Abc_123",
                LMSConstants.RoleTeacher
                );

            AddOrUpdateUser(
                userManager,
                new ApplicationUser
                {
                    /*UserName = "Edna",*/
                    Email = "edna.krabappel@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "Edna",
                    LastName = "Krabappel"
                },
                "Abc_123",
                LMSConstants.RoleTeacher
                );

            AddOrUpdateUser(
                userManager,
                new ApplicationUser
                {
                    /*UserName = "Gary",*/
                    Email = "superintendent.chalmers@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "Gary",
                    LastName = "Chalmers"
                },
                "Abc_123",
                LMSConstants.RoleTeacher
                );

            AddOrUpdateUser(
                userManager,
                new ApplicationUser
                {
                    /*UserName = "Elizabeth",*/
                    Email = "elizabeth.hoover@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "Elizabeth",
                    LastName = "Hoover"
                },
                "Abc_123",
                LMSConstants.RoleTeacher
                );

            AddOrUpdateUser(
                userManager,
                new ApplicationUser
                {
                    /*UserName = "William",*/
                    Email = "groundskeeper.willie@springfieldelementary.edu",
                    PhoneNumber = "070-123456",
                    FirstName = "William",
                    LastName = "MacDougal"
                },
                "Abc_123",
                LMSConstants.RoleTeacher
                );

            context.SaveChanges();
        }

        /// <summary>
        /// Add or update a Group (class).
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="startDate">The date when the group starts.</param>
        /// <param name="duration">The length of the Group in number of days.</param>
        /// <param name="groupName">The name of the group without month and year (this will be calculated based on the start date).</param>
        /// <param name="description">A description of the group.</param>
        /// <param name="cultureName">The name of the culture (used when generating month names).</param>
        /// <returns>The newly added Group.</returns>
        private static Group AddOrUpdateGroup(ApplicationDbContext context, DateTime startDate, int duration,
                                            string groupName, string description, string cultureName = "sv-SE")
        {
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo(cultureName);
            string monthName = DateTimeFormatInfo.GetInstance(cultureInfo).GetMonthName(startDate.Month);
            monthName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(monthName);
            //monthName = char.ToUpper(monthName[0]) + monthName.Substring(1);

            Group newGroup = new Group
            {
                Name = string.Format("{0} {1} {2}", groupName, monthName, startDate.Year),
                Description = description,
                StartDate = startDate,
                EndDate = startDate.AddWorkDays(duration - 1)
            };

            context.Groups.AddOrUpdate(g => g.Name, newGroup);

            context.SaveChanges();
            return newGroup;
        }

        /// <summary>
        /// Add or update a Course.
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="group">The Group object to which this Course belongs.</param>
        /// <param name="courseStartDate">The date when the course starts.</param>
        /// <param name="duration">The length of the course in number of days.</param>
        /// <param name="courseName">The name of the course.</param>
        /// <param name="description">A description of the course.</param>
        /// <returns>The newly created Course.</returns>
        private Course AppendOrUpdateCourse(ApplicationDbContext context, Group group,
                                            DateTime courseStartDate, int duration,
                                            string courseName, string description)
        {
            Course newCourse = new Course
            {
                Name = courseName,
                Description = description,
                StartDate = courseStartDate,
                EndDate = courseStartDate.AddWorkDays(duration - 1),
                GroupId = group.Id
            };

            context.Courses.AddOrUpdate(c => new { c.Name, c.StartDate }, newCourse);

            context.SaveChanges();
            return newCourse;
        }

        /// <summary>
        /// Add or update an Activity.
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="course">The Course object to which this Activity belongs.</param>
        /// <param name="activityStartDate">The date when the activity starts.</param>
        /// <param name="duration">The length of the activity in days.</param>
        /// <param name="type">The type of the activity.</param>
        /// <param name="name">The name of the activity.</param>
        /// <param name="description">A description of the activity.</param>
        /// <returns>The newly created Activity</returns>
        private Activity AppendOrUpdateActivity(ApplicationDbContext context, Course course,
                                                ActivityType type, DateTime activityStartDate, int duration,
                                                string name, string description)
        {
            Activity newActivity = new Activity
            {
                Name = name,
                Description = description,
                Type = type,
                StartDate = activityStartDate,
                EndDate = activityStartDate.AddWorkDays(duration - 1),
                CourseId = course.Id
            };

            context.Activities.AddOrUpdate(a => new { a.Name, a.StartDate }, newActivity);

            context.SaveChanges();
            return newActivity;
        }

        /// <summary>
        /// Add or update an ApplicationUser unless a user with the same email address already exist.
        /// </summary>
        /// <param name="userManager">An instance of a UserManager.</param>
        /// <param name="newUser">An instance of the ApplicationUser to be created.</param>
        /// <param name="password">A plain text password.</param>
        /// <param name="role">The role for the new user.</param>
        /// <returns></returns>
        private static void AddOrUpdateUser(UserManager<ApplicationUser> userManager, ApplicationUser newUser, string password, string role)
        {
            newUser.Email = newUser.Email.RemoveDiacritics();

            //System.IO.File.AppendAllLines(@"C:\temp\LMSout.txt", new List<string> { newUser.FirstName + " " + newUser.LastName + " " + newUser.GroupId.ToString() + " " + newUser.Email });
            // Set UserName to Email to get the same behaviour as in the AccountController.Register() method.
            newUser.UserName = newUser.Email;

            ApplicationUser appUser = userManager.FindByEmail(newUser.Email);

            if (appUser == null)
            {
                IdentityResult result = userManager.Create(newUser, password);
                if (result.Succeeded)
                    userManager.AddToRole(newUser.Id, role);
                //else
                //    System.IO.File.AppendAllLines(@"C:\temp\LMSout.txt", result.Errors);
            }
            //else
            //{
            //    throw new Exception("Duplicate user: " + appUser.UserName);
            //}
        }

        /// <summary>
        /// Adds students from a List.
        /// </summary>
        /// <param name="userManager">An instance of UserManager.</param>
        /// <param name="studentNames">A List of first name / last name tuples.</param>
        /// <param name="maxUserCount">Maxumum number of users you want in the database.</param>
        private static void AddStudents(UserManager<ApplicationUser> userManager,
                                            List<Tuple<string, string>> studentNames, List<int> groupIds, int maxUserCount)
        {
            // Don't seed if we already have users.
            if (studentNames.Count() < maxUserCount)
                return;

            int maxNumberOfGroupMembers = 14;

            Stack<int> availableGroupIds = new Stack<int>();
            foreach (int id in groupIds)
            {
                int groupMemberCount = userManager.Users
                    .Where(u => u.GroupId == id)
                    .Count();

                if (groupMemberCount < maxNumberOfGroupMembers)
                    availableGroupIds.Push(id);
            }

            int i = 0;
            int? groupId = null;

            foreach (Tuple<string, string> studentName in studentNames)
            {
                if (i % maxNumberOfGroupMembers == 0)
                {
                    if (availableGroupIds.Count() > 0)
                        groupId = availableGroupIds.Pop();
                    else
                        groupId = null;
                }

                i++;
                AddOrUpdateUser(
                    userManager,
                    new ApplicationUser
                    {
                        /*UserName = studentName.Item1,*/
                        Email = studentName.Item1 + "." + studentName.Item2 + "@springfieldelementary.edu",
                        PhoneNumber = "070-123456",
                        FirstName = studentName.Item1,
                        LastName = studentName.Item2,
                        GroupId = groupId // (i++ % 14 == 0) ? groupIds[groupIdIndex++] : groupIds[groupIdIndex]
                    },
                    "Abc_123",
                    LMSConstants.RoleStudent
                    );
            }
        }


        //internal static string RemoveDiacritics(string input)
        //{
        //    if (String.IsNullOrEmpty(input))
        //        return String.Empty;

        //    string stFormD = input.Normalize(NormalizationForm.FormD);
        //    StringBuilder sb = new StringBuilder();

        //    for (int ich = 0; ich < stFormD.Length; ich++)
        //    {
        //        UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
        //        if (uc != UnicodeCategory.NonSpacingMark)
        //        {
        //            sb.Append(stFormD[ich]);
        //        }
        //    }

        //    return (sb.ToString().Normalize(NormalizationForm.FormC));
        //}

        #region SeedData

        string[] loremIpsum = {
                "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            };

        //courses "C# grunder", "Grunderna i programmeringsspråket C#");

        //newActivity = AppendOrUpdateActivity(context, newCourse, "Föreläsning C#", "Föreläsning och codealong där vi lär oss grunderna i Visual Studio och C#", groupStartDate, 2);
        //newActivity = AppendOrUpdateActivity(context, newCourse, "C# Fundamentals", "E-learning som går igenom variabeldeklarationer och flödeskontroll i C#", newActivity.EndDate.AddWorkDays(1), 2);
        //newActivity = AppendOrUpdateActivity(context, newCourse, "C# The Next Step", "Mer om programmering i C#", newActivity.EndDate.AddWorkDays(1), 1);

        //newCourse = AppendOrUpdateCourse(context, newGroup, "Databas, grund", "Vi lär oss Entity Framework och Linq", newActivity.EndDate.AddWorkDays(1), 5);
        //newActivity = AppendOrUpdateActivity(context, newCourse, "Entity Framework 6", "Hantering av EF och code first", newCourse.StartDate, 3);
        //newActivity = AppendOrUpdateActivity(context, newCourse, "Linq", "Databasfrågor med Linq", newCourse.EndDate.AddWorkDays(1), 2);

        #endregion
    }
}
