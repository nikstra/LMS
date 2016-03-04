using LMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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
            // Temporarly using the about page as start page for authenticated users.
            ViewBag.ReturnUrl = "/Home/About";

            // If user already authenticated, redirect to start page.
            if (User.Identity.IsAuthenticated)
                return Redirect(ViewBag.ReturnUrl);

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

        public ActionResult Student()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());

            if(currentUser.GroupId != null)
            {
                var usersInGroup = db.Users
                    .Where(u => u.GroupId == currentUser.GroupId) ?? null;

                TempData["ApplicationUsers"] = usersInGroup;
            }
            else
            {
                TempData["ApplicationUsers"] = new List<ApplicationUser>();
            }

            var upcomingActivities = db.Activities
                .Where(a => a.StartDate >= DateTime.Now)
                .Where(a => a.Course.GroupId == currentUser.GroupId)
                .OrderBy(a => a.StartDate)
                .Take(10)
                .ToList();

            return View(upcomingActivities);
            //return View(db.Activities.ToList());
        }
    }
}