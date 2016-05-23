using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebOrganizer.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (Session["LogedUserID"] != null)
            {
                return RedirectToAction("Index", "Tasks");
            }
            return View();
            
        }
    }
}