using LMS.Models;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace LMS.Controllers
{
    /// <summary>
    /// A shared controller. Can be used with partial views.
    /// </summary>
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

            if (activeUser != null && activeUser.Group != null)
            {
                TempData["GroupId"] = activeUser.Group.Id;
                return Content(activeUser.Group.Name);
            }

            return Content("Ingen grupp");
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