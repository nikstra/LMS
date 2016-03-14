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
using Microsoft.AspNet.Identity;

namespace LMS.Controllers
{
    [Authorize(Roles = LMSConstants.RoleTeacher + "," + LMSConstants.RoleStudent)]
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }

        // GET: Groups

        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        // GET: Groups/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }

            var rd = ControllerContext.RouteData;
            var currentAction = rd.GetRequiredString("action");
            var currentController = rd.GetRequiredString("controller");

            TempData["ParentId"] = group.Id;
            TempData["DocumentParent"] = LMSConstants.Group;
            TempData["ReturnPath"] = Request.FilePath;
            TempData["GroupName"] = group.Name;
            TempData["GroupId"] = group.Id;

            ViewBag.DocumentModel = db.Documents
                .Where(d => d.GroupId == id)
                .ToList();

            // Students should be allowed to upload documents.
            TempData["IsAdministator"] = true;

            if (User.IsInRole(LMSConstants.RoleTeacher))
            {
                ViewBag.IsAdministrator = true;
                return View(group);
            }
            else
                return View(group);
        }

        // GET: Groups/Create
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Create()
        {
            Group group = new Group();
            group.StartDate = DateTime.Now;
            group.EndDate = DateTime.Now;
            return View(group);
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,ApplicationUserId")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: Groups/Edit/5
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,ApplicationUserId")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Groups/Delete/5
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = LMSConstants.RoleTeacher)]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);

            if (group.Users.Any(u => u.GroupId == group.Id) && (group.Courses.Any(c => c.GroupId == group.Id)))
            {
                ViewBag.Error = "Kan inte radera - Både studenter och kurser finns i grupp";
                return View(group);
            }
            else if (group.Courses.Any(c => c.GroupId == group.Id))
            {
                ViewBag.Error = "Kan inte radera - kurser finns i grupp";
                return View(group);
            }
            else if (group.Users.Any(u => u.GroupId == group.Id))
            {
                ViewBag.Error = "Kan inte radera - studenter finns i grupp";
                return View(group);
            }
            else
            {
                db.Groups.Remove(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        //GET: Groups/UsersInGroup/3
        [Authorize(Roles = LMSConstants.RoleTeacher + "," + LMSConstants.RoleStudent)]
        public ActionResult UsersInGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var usersInGroup = db.Users
                .Where(u => u.GroupId == id) ?? null;

            if (usersInGroup.Count() > 0)
            {
                ViewBag.GroupName = db.Groups
                    .Where(g => g.Id == id)
                    .FirstOrDefault()
                    .Name;


                if (User.IsInRole(LMSConstants.RoleTeacher))
                    ViewBag.IsAdministrator = true;

                return View(usersInGroup);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
