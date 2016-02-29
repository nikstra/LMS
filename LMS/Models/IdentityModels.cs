using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }
        [Display(Name = "Användarnamn")]
        public override string UserName { get { return base.UserName; } set { base.UserName = value; } }
        [Display(Name = "E-post")]
        public override string Email { get { return base.Email; } set { base.Email = value; } }
        [Display(Name = "Telefonnummer")]
        public override string PhoneNumber { get { return base.PhoneNumber; } set { base.PhoneNumber = value; } }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Activity> Activities { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // This line was added when scaffolding the view for Groups.UsersInGroup() based on ApplicationUsers. This causes the following exception to be thrown:
        // Multiple object sets per type are not supported. The object sets 'ApplicationUsers' and 'Users' can both contain instances of type 'LMS.Models.ApplicationUser'.
        //public System.Data.Entity.DbSet<LMS.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}