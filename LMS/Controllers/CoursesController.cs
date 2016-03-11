using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using LMS.Constants;

namespace LMS.Controllers
{
    [Authorize(Roles = LMSConstants.RoleTeacher + "," + LMSConstants.RoleStudent)]
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        // GET: Courses/Details/5
        [Authorize(Roles = LMSConstants.RoleTeacher + "," + LMSConstants.RoleStudent)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            TempData["CourseId"] = course.Id;
            TempData["DocumentParent"] = LMSConstants.Course;

            ViewBag.DocumentModel = db.Documents
                .Where(d => d.CourseId == id)
                .ToList();

            if (User.IsInRole(LMSConstants.RoleTeacher))
            {
                ViewBag.IsAdministrator = true;
                return View(course);
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Create(int GroupId)
        {
            Course course = new Course();
            course.GroupId = GroupId;
            course.StartDate = DateTime.Now;
            course.EndDate = DateTime.Now;
            return View(course);
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,GroupId,ActivityId")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Details", "Groups", new { id = course.GroupId });
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,GroupId,ActivityId")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Groups", new { id = course.GroupId });
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            if (course.Activities.Any(a => a.CourseId == course.Id))
            {
                ViewBag.Error = "Kan inte radera - aktiviteter finns i kurs";
                return View(course);
            }
            else
            {
                db.Courses.Remove(course);
                db.SaveChanges();
                return RedirectToAction("Details", "Groups", new { id = course.GroupId });
            }
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
