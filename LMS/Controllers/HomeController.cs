using LMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LMS.Constants;

namespace LMS.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public HomeController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        public ActionResult Index()
        {
            ViewBag.ReturnUrl = "/Home/Index";
            ViewBag.Error = TempData["Error"];

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(LMSConstants.RoleTeacher))
                    return RedirectToAction("Index", "Groups");
                else
                    return RedirectToAction("Student", "Home");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles = LMSConstants.RoleTeacher + "," + LMSConstants.RoleStudent)]
        public ActionResult Student()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());

            if(currentUser.GroupId != null)
            {
                var usersInGroup = db.Users
                    .Where(u => u.GroupId == currentUser.GroupId)
                    .OrderBy(u => u.FirstName)
                    ?? null;

                TempData["ApplicationUsers"] = usersInGroup;
            }
            else
            {
                TempData["ApplicationUsers"] = new List<ApplicationUser>();
            }

            var upcomingActivities = db.Activities
                .Where(a => a.EndDate >= DateTime.Now)
                .Where(a => a.Course.GroupId == currentUser.GroupId)
                .OrderBy(a => a.StartDate)
                .Take(10)
                .ToList();

            return View(upcomingActivities);
        }
    }
}