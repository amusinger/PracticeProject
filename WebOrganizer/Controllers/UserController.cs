using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebOrganizer.Models;

namespace WebOrganizer.Controllers
{
    public class UserController : Controller
    {
        private UserDbEntities db = new UserDbEntities();
        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User U)
        {
            if (ModelState.IsValid)
            {
                 if (db.Users.Where(x => x.Username == U.Username).FirstOrDefault() == null)
                    {

                        db.Users.Add(U);
                        db.SaveChanges();
                       // ModelState.Clear();
                       // U = null;
                        ViewBag.Message = "Successfully registered";
                        return RedirectToAction("Login", "User");
                    }
                    ViewBag.Message = "User already registered, please choose enter username";
                }
            
            return View();
        }


        
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["LogedUserID"] = null;
            Session["LogedUsername"] = null;
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User u)
        {
            UserDbEntities dc = new UserDbEntities();

            var v = dc.Users.Where(a => a.Username.Equals(u.Username) && a.Password.Equals(u.Password)).FirstOrDefault();
            if (v != null)
            {
                Session["LogedUserID"] = v.UserID.ToString();
                Session["LogedUsername"] = v.Username.ToString();
                return RedirectToAction("AfterLogin");
            }


            return View(u);
        }

        public ActionResult AfterLogin()
        {
            //var s = Session["LogedUsername"].ToString();
            //if (Session["LogedUserID"] != null && s.Equals("admin"))
            //{
            //    return RedirectToAction("Users");
            //}
            if (Session["LogedUserID"] != null)
            {
                return RedirectToAction("Index", "Tasks");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult Users()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            var users = db.Users;
            return View(users);

        }
        public ActionResult UserDetails(int? UsersID)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            UserDbEntities db = new UserDbEntities();
                        
            if (UsersID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tasks = db.Tasks.Where(x => x.UserID == UsersID);
            var finishedTasks = db.FinishedTasks.Where(x => x.UserID == UsersID);

            var model = new WebOrganizer.Models.myModel.AllTasks()
            {
                Tasks = tasks.ToList(),
                FinishedTasks = finishedTasks.ToList()
            };
            
          
            if (model.Tasks == null)
            {
                return HttpNotFound();
            }
            return View(model);
        
        }

        public ActionResult UserDelete(int? UsersID)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
         
            if (UsersID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
                var u = db.Users.Where(x => x.UserID == UsersID).FirstOrDefault();
                var tasks = db.Tasks.Where(x => x.UserID == UsersID);
                var finishedTasks = db.FinishedTasks.Where(x => x.UserID == UsersID);
                if (u == null)
                {
                    return HttpNotFound();
                }
                if (tasks != null)
                {
                    foreach (var t in tasks)
                    {
                        db.Tasks.Remove(t);
                    }
                }
                if (finishedTasks != null)
                {
                    foreach (var t in finishedTasks)
                    {
                        db.FinishedTasks.Remove(t);
                    }
                }
                db.Users.Remove(u);
                
                db.SaveChanges();
            
            
            return RedirectToAction("Users");

        }
       

        [HttpPost]
        public ActionResult Search(string name)
        {
          
            System.Threading.Thread.Sleep(2000);
            
            var users = db.Users.Where(x => x.Username.Contains(name));
            return PartialView(users);
        }


        public ActionResult EditProfile(int? id)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
           
            if (Session["LogedUserID"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(id);
                //user.UserID = int.Parse(Session["LogedUserID"].ToString());
                //user.Username = Session["LogedUsername"].ToString();
                //user.ConfirmPassword = user.Password;
                //user.FinishedTasks = db.FinishedTasks.Where(x => x.UserID == user.UserID).ToList();
                //user.Tasks = db.Tasks.Where(x => x.UserID == user.UserID).ToList();
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            return RedirectToAction("Index");
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(User user)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //int usID = int.Parse(Session["LogedUserID"].ToString());
            //string oldPass = (from d in db.Users
            //              where d.UserID == usID
            //              select d.Password).ToString();

            user.UserID = int.Parse(Session["LogedUserID"].ToString());
            user.Username = Session["LogedUsername"].ToString();
            user.ConfirmPassword = user.Password;
            user.FinishedTasks = db.FinishedTasks.Where(x => x.UserID == user.UserID).ToList();
            user.Tasks = db.Tasks.Where(x => x.UserID == user.UserID).ToList();
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            //Session["LogedUserID"] = null;
            //Session["LogedUsername"] = null;
            return RedirectToAction("EditProfile", "User");
          
          //  return RedirectToAction("Index", "Tasks");
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