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
    [Authorize(Roles=LMSConstants.RoleTeacher + "," + LMSConstants.RoleStudent)]
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private DirectoryInfo _uploadDirInfo;

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
                TempData["IsAdministator"] = true;

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
                //.Include(d => d.Group.Name).Where(d => d.Id == id).FirstOrDefault();//
            ////if (type == LMSConstants.Group)
            //    var dbdoc = db.Documents.Include(d => d.Group.Name)
            //        //.Where(d => d.Id == id).Find(id);
            //    //document.Include();

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

                    //@"~/App_Data/uploads"
                    //Path.Combine(path, "uploads")


                    string fileName = Guid.NewGuid().ToString() + "_" + System.IO.Path.GetFileName(upload.FileName);

                    string localPath = Path.Combine(_uploadDirInfo.FullName, fileName);

                    var uploadDocument = new Document
                    {
                        ApplicationUserId = activeUser.Id,
                        Description = document.Description,
                        /*Feedback = "",*/
                        Name = upload.FileName,
                        TimeStamp = DateTime.Now,
                        LocalPath = @"~/App_Data/uploads/" + fileName
                    };

                    string action = "Index";
                    //var t = type;
                    //Request.UrlReferrer.LocalPath == "/Home/Student";
                    if (type == LMSConstants.Activity)
                    {
                        action = LMSConstants.Activity;
                        uploadDocument.ActivityId = id; //(int?)TempData["ActivityId"]; 
                    }
                    else if (User.IsInRole(LMSConstants.RoleStudent))
                    {
                        action = "";
                        uploadDocument.GroupId = id; //activeUser.GroupId;
                    }
                    else if (type == LMSConstants.Course)
                    {
                        action = LMSConstants.Course;
                        uploadDocument.CourseId = id; //(int?)TempData["CourseId"];
                    }
                    else if (type == LMSConstants.Group)
                    {
                        action = LMSConstants.Group;
                        uploadDocument.GroupId = id; // (int?)TempData["GroupId"];
                    }
                    //var vc = new ControllerContext();
                    //var parentActionViewContext = ControllerContext.ParentActionViewContext;
                    ////Session.
                    //if ((string)TempData["DocumentParent"] == "course")
                    //    uploadDocument.CourseId = (int?)TempData["CourseId"];
                    //else if ((string)TempData["DocumentParent"] == "activity")
                    //    uploadDocument.ActivityId = (int?)TempData["ActivityId"];
                    //else if ((string)TempData["DocumentParent"] == "group")
                    //    uploadDocument.GroupId = (int?)TempData["GroupId"];

                    upload.SaveAs(localPath);
                    //instructor.FilePaths = new List<Document>();
                    //instructor.FilePaths.Add(photo);
                    db.Documents.Add(uploadDocument);
                    db.SaveChanges();
                }
                //db.SaveChanges();
                //return RedirectToAction("Index");
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
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Feedback,TimeStamp,LocalPath,ActivityId," + /*ApplicationUserId,*/"CourseId,GroupId")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
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

            // Delete file!
            string fullPath = System.Web.HttpContext.Current.Server.MapPath(document.LocalPath);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            db.Documents.Remove(document);
            db.SaveChanges();
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

        //protected override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    if (filterContext.Controller.ControllerContext.ParentActionViewContext != null)
        //    {
        //        //filterContext.Controller.ViewBag.Area = filterContext.Controller.ValueProvider.GetValue("area").RawValue;
        //        filterContext.Controller.ViewBag.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName; // filterContext.Controller.ValueProvider.GetValue("controller").RawValue;
        //        filterContext.Controller.ViewBag.Action = filterContext.ActionDescriptor.ActionName; // filterContext.Controller.ValueProvider.GetValue("action").RawValue;
        //        base.OnActionExecuted(filterContext);
        //    }
        //}

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
