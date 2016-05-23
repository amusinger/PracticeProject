using System;
using System.Collections.Generic;
using System.Linq;
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
            if (Session["LogedUserID"] != null)
            {
                return RedirectToAction("Index", "Tasks");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
    }
}