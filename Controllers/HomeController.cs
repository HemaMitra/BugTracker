using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        ApplicationUser au = new ApplicationUser();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AuthorizedIndex()
        {
            if(User.IsInRole("Admin"))
            {
                return View(db.Projects.ToList());
            }
            if(User.IsInRole("ProjectManager") || User.IsInRole("Developer") || User.IsInRole("Submitter"))
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                return View(user.Projects.ToList());
            }

            return View(db.Projects.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}