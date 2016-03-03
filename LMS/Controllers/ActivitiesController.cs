using LMS.Constants;
using LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    [Authorize(Roles = LMSConstants.RoleTeacher + "," + LMSConstants.RoleStudent)]
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activities
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Index()
        {
            Debug.Print("Is in role teacher: " + User.IsInRole(LMSConstants.RoleTeacher));

            var activities = db.Activities.Include(a => a.Course);
            return View(activities.ToList());
        }

        // GET: Activities/Details/5
        [Authorize(Roles = LMSConstants.RoleTeacher + "," + LMSConstants.RoleStudent)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if(User.IsInRole(LMSConstants.RoleTeacher))
                ViewBag.IsAdministrator = true;

            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Create(int CourseId)
        {
            Activity activity = new Activity();
            activity.CourseId = CourseId;
            activity.StartDate = DateTime.Now;
            activity.EndDate = DateTime.Now;
            //ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
            return View(activity);
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Details", "Courses", new { id = activity.CourseId });
            }

            //ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", activity.CourseId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", activity.CourseId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", activity.CourseId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Details", "Courses", new { id = activity.CourseId });
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
