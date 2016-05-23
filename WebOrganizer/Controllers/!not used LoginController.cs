using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOrganizer.Models;

namespace WebOrganizer.Controllers
{
    public class LoginController : Controller
    {
        UserDbEntities dc = new UserDbEntities();
        // GET: Login
        public ActionResult Login()
        {
            return View();
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
                return RedirectToAction("Login", "Login");
            }
        }
    }
}