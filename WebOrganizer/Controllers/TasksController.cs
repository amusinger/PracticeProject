using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebOrganizer.Models;

namespace WebOrganizer.Controllers
{
    public class TasksController : Controller
    {
        private UserDbEntities db = new UserDbEntities();

        // GET: Tasks
        public ActionResult Index()
        {
            var uID = Session["LogedUserID"].ToString();
            int LogedID = Int32.Parse(Session["LogedUserID"].ToString());
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index");
            }

            if (uID != null)
            {
                var tasks = db.Tasks.Where(x => x.UserID == LogedID).ToList();

                tasks.OrderBy(x => x.StartDate);
                tasks.OrderBy(x => x.TaskDescription);

                var s = tasks.FirstOrDefault();
                if(s == null)
                {
                    ViewBag.Message = null;
                }
               
                if(s != null)
                {
                    ViewBag.Message = "tasks";
                }
                
                return View(tasks);
            }
            
            else
            {
                return RedirectToAction("Login", "User");
            }
            
        }

        //// GET: Tasks/Details/5 not used
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Task task = db.Tasks.Find(id);
        //    if (task == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(task);
        //}

        // GET: Tasks/Create
        public ActionResult Create()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index");
            }
           
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskID,TaskDescription,StartDate,FinishDate,Category,UserID")] Task task)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index");
            }
            
            if (ModelState.IsValid)
            {
                task.UserID = Int32.Parse(Session["LogedUserID"].ToString());
                
                
                db.Tasks.Add(task);
                db.SaveChanges();
                ViewBag.Message = "tasks";
                return RedirectToAction("Index");
            }

           // ViewBag.UserID = Int32.Parse(Session["LogedUserID"].ToString());
            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", task.UserID);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskID,TaskDescription,StartDate,FinishDate,Category,UserID")] Task task)
        {
            if (ModelState.IsValid)
            {
                task.UserID = Int32.Parse(Session["LogedUserID"].ToString());
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           // ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", task.UserID);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
           
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
           // return View(task);
        }

        // POST: Tasks/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Task task = db.Tasks.Find(id);
        //    db.Tasks.Remove(task);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}



        public ActionResult DoneTasks()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var uID = Session["LogedUserID"].ToString();
            int LogedID = Int32.Parse(Session["LogedUserID"].ToString());

            if (uID != null)
            {
                var tasks = db.FinishedTasks.Where(x => x.UserID == LogedID).ToList();

                var s = tasks.FirstOrDefault();

                if (s == null)
                {
                    ViewBag.Message = null;
                }

                if (s != null)
                {
                    ViewBag.Message = "tasks";
                }
                
                
                return View(tasks);
            }
            else
            {
                return RedirectToAction("Index", "Tasks");
            }
        }

      
        public ActionResult Done(int? id)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Task task = db.Tasks.Find(id);
            FinishedTask ft = new FinishedTask();
            ft.TaskID = task.TaskID;
            ft.TaskDescription = task.TaskDescription;
            ft.StartDate = task.StartDate;
            ft.FinishDate = task.FinishDate;
            ft.Category = task.Category;
            ft.UserID = task.UserID;
            db.FinishedTasks.Add(ft);

            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult DoneDelete(int? DoneID)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (DoneID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinishedTask task = db.FinishedTasks.Find(DoneID);
          
           
            if (task == null)
            {
                return HttpNotFound();
            }

            db.FinishedTasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("DoneTasks");

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
