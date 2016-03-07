using LMS.Constants;
using LMS.Models;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace LMS.Controllers
{
    /// <summary>
    /// A shared controller. Can be used with partial views.
    /// </summary>
    [Authorize(Roles = LMSConstants.RoleTeacher + "," + LMSConstants.RoleStudent)]
    public class SharedController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Call this action in the view to get the group name.
        /// @{Html.RenderAction("GroupName", "Shared");}
        /// </summary>
        /// <returns>
        /// ContentResult.Content() with either the group name or
        /// a string indicating that no group was found.
        /// </returns>
        public ActionResult GroupName()
        {
            db.Database.Log = (m => Debug.Print(m));

            var activeUser = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            if (User.IsInRole(LMSConstants.RoleTeacher))
                TempData["IsAdministrator"] = true;

            if (activeUser != null && activeUser.Group != null)
            {
                TempData["GroupId"] = activeUser.Group.Id;
                return Content(activeUser.Group.Name);
            }

            return Content("Ingen grupp");
        }

        /// <summary>
        /// Call this action in the view to get the current users first name.
        /// </summary>
        /// <returns>
        /// ContentResult.Content() with either the first name or
        /// a string indicating that no name was found.
        /// </returns>
        public ActionResult FirstName()
        {
            db.Database.Log = (m => Debug.Print(m));

            var activeUser = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            if (activeUser != null && activeUser.FirstName != null)
            {
                return Content(activeUser.FirstName);
            }

            return Content("Inget förnamn");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}