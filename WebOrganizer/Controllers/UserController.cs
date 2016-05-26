using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebOrganizer.Models;

namespace WebOrganizer.Controllers
{
    public class UserController : Controller
    {
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
                using (UserDbEntities db = new UserDbEntities())
                {
                    if (db.Users.Where(x => x.Username == U.Username).FirstOrDefault() == null)
                    {

                        db.Users.Add(U);
                        db.SaveChanges();
                        ModelState.Clear();
                        U = null;
                        ViewBag.Message = "Successfully registered";
                        return RedirectToAction("Login", "User");
                    }
                    ViewBag.Message = "User already registered, please choose enter username";
                }
            }
            return View();
        }


        UserDbEntities dc = new UserDbEntities();
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
            var s = Session["LogedUsername"].ToString();
            if (Session["LogedUserID"] != null && s.Equals("admin"))
            {
                return RedirectToAction("Users");
            }
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
            UserDbEntities db = new UserDbEntities();
            var users = db.Users;
            return View(users);

        }
        public ActionResult UserDetails(int? UsersID)
        {
            UserDbEntities db = new UserDbEntities();
                        
            if (UsersID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebOrganizer.Models.myModel.AllTasks model = new WebOrganizer.Models.myModel.AllTasks();
            model.Tasks = db.Tasks.Where(x => x.UserID == UsersID);
            model.FinishedTasks = db.FinishedTasks.Where(x => x.UserID == UsersID);
            if (model.Tasks == null)
            {
                return HttpNotFound();
            }
            return View(model);
        
        }

        
    }
}