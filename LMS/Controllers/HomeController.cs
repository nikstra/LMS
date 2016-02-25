using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class HomeController : Controller
    {
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
    }
}