using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using System.IO;
using LMS.Constants;

namespace LMS.Controllers
{
    [Authorize(Roles = LMSConstants.RoleTeacher + "," + LMSConstants.RoleStudent)]
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private DirectoryInfo _uploadDirInfo;

        public object RouteTables { get; private set; }

        public DocumentsController()
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data");
            _uploadDirInfo = Directory.CreateDirectory(Path.Combine(path, "uploads"));
        }

        // GET: Documents
        //[ChildActionOnly]
        public ActionResult Index()
        {
            var documents = db.Documents.Include(d => d.Activity)/*.Include(d => d.ApplicationUser)*/.Include(d => d.Course).Include(d => d.Group);
            if (User.IsInRole(LMSConstants.RoleTeacher))
                TempData["IsAdministrator"] = true;

            var activeUser = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            TempData["UserId"] = activeUser.Id;
            
            return View(documents.ToList());
        }

        // GET: Documents/Details/5
        public ActionResult Details(string type, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);

            var activeUser = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            if (User.IsInRole(LMSConstants.RoleTeacher) ||
                document.ApplicationUserId == activeUser.Id)
            {
                TempData["CanViewFeedback"] = true;
            }

            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult Upload()
        {
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name");
            //ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "UserName");
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload([Bind(Include = "Id,Name,Description,Feedback,TimeStamp,LocalPath,ActivityId," +/*ApplicationUserId,*/"CourseId,GroupId")] Document document, HttpPostedFileBase upload, string type, int? id)
        {
            if (ModelState.IsValid)
            {
                //Document uploadDocument;
                // Upload limit is set in web.config <system.web>
                // <httpRuntime maxRequestLength="1048576" executionTimeout="3600" />
                // and also needs to be set under <security><requestFiltering>
                // <requestLimits maxAllowedContentLength="1073741824" />

                if (upload != null && upload.ContentLength > 0)
                {
                    String downloadDirectory = _uploadDirInfo.FullName; //Server.MapPath("~/App_Data/uploads");
                    var activeUser = db.Users
                        .Where(u => u.UserName == User.Identity.Name)
                        .FirstOrDefault();


                    string fileName = Guid.NewGuid().ToString() + "_" + System.IO.Path.GetFileName(upload.FileName);

                    string localPath = Path.Combine(_uploadDirInfo.FullName, fileName);

                    var uploadDocument = new Document
                    {
                        ApplicationUserId = activeUser.Id,
                        Description = document.Description,
                        /*Feedback = "",*/
                        Name = System.IO.Path.GetFileName(upload.FileName),
                        TimeStamp = DateTime.Now,
                        LocalPath = @"~/App_Data/uploads/" + fileName
                    };

                    //Request.UrlReferrer.LocalPath == "/Home/Student";
                    if (type == LMSConstants.Activity)
                    {
                        uploadDocument.ActivityId = id;
                    }
                    else if (User.IsInRole(LMSConstants.RoleStudent))
                    {
                        uploadDocument.GroupId = id;
                    }
                    else if (type == LMSConstants.Course)
                    {
                        uploadDocument.CourseId = id;
                    }
                    else if (type == LMSConstants.Group)
                    {
                        uploadDocument.GroupId = id;
                    }

                    upload.SaveAs(localPath);

                    db.Documents.Add(uploadDocument);
                    db.SaveChanges();
                }

                return RedirectToAction("Details", type, new { id = id });
            }

            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", document.ActivityId);
            //ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "UserName", document.ApplicationUserId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", document.CourseId);
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", document.GroupId);
            return View(document);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(string type, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }

            var activeUser = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            if (User.IsInRole(LMSConstants.RoleTeacher))
                TempData["CanEditFeedback"] = true;

            if (User.IsInRole(LMSConstants.RoleTeacher) ||
                document.ApplicationUserId == activeUser.Id)
            {
                TempData["CanViewFeedback"] = true;
            }
            else
            {
                //TempData["CanViewFeedback"] = null;
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", document.ActivityId);
            //ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "UserName", document.ApplicationUserId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", document.CourseId);
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", document.GroupId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Feedback,TimeStamp,LocalPath,ActivityId," + /*ApplicationUserId,*/"CourseId,GroupId")] Document document, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var documentToBeModified = db.Documents.First(d => d.Id == document.Id);
                documentToBeModified.Name = document.Name;
                documentToBeModified.Description = document.Description;
                documentToBeModified.Feedback = document.Feedback;

                var formMove = form["move"];
                int value = 0;

                if (formMove == LMSConstants.Activity)
                {
                    var a = form["ActivityId"];
                    if (a != null)
                    {
                        if (Int32.TryParse(a, out value))
                        {
                            documentToBeModified.ActivityId = value;
                            documentToBeModified.CourseId = null;
                            documentToBeModified.GroupId = null;
                        }
                    }
                }
                else if (formMove == LMSConstants.Course)
                {
                    var c = form["CourseId"];
                    if (c != null)
                    {
                        if (Int32.TryParse(c, out value))
                        {
                            documentToBeModified.ActivityId = null;
                            documentToBeModified.CourseId = value;
                            documentToBeModified.GroupId = null;
                        }
                    }
                }
                else if (formMove == LMSConstants.Group)
                {
                    var g = form["GroupId"];
                    if (g != null)
                    {
                        if (Int32.TryParse(g, out value))
                        {
                            documentToBeModified.ActivityId = null;
                            documentToBeModified.CourseId = null;
                            documentToBeModified.GroupId = value;
                        }
                    }
                }
                //db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                if (TempData.Peek("ReturnPath") != null)
                    return Redirect((string)TempData["ReturnPath"]);
                else
                    return RedirectToAction("Index");
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", document.ActivityId);
            //ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "UserName", document.ApplicationUserId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", document.CourseId);
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", document.GroupId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(string type, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);

            var activeUser = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            if (activeUser.Id != document.ApplicationUserId && !User.IsInRole(LMSConstants.RoleTeacher))
            {
                if (TempData["ReturnPath"] != null)
                    return Content(@"Behörighet saknas <a href='" + TempData["ReturnPath"] + "'>Tillbaka</a>");
                else
                    return Content(@"Behörighet saknas <a href='/'>Hem</a>");
            }

            string fullPath = System.Web.HttpContext.Current.Server.MapPath(document.LocalPath);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            db.Documents.Remove(document);
            db.SaveChanges();
            if (TempData.Peek("ReturnPath") != null)
                return Redirect((string)TempData["ReturnPath"]);
            else
                return RedirectToAction("Index");
        }

        public ActionResult Download(string type, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);

            string path = Server.MapPath(document.LocalPath);

            if (System.IO.File.Exists(path))
            {
                return File(path, MimeMapping.GetMimeMapping(document.Name), document.Name);
            }
            return HttpNotFound();
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
