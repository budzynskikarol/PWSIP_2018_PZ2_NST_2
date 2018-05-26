using HelpDesk.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HelpDesk.Controllers.ManageController;

namespace HelpDesk.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                   message == ManageMessageId.ChangePasswordSuccess ? "Zmieniono hasło."
                   : "";
            //ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            //if (user.ChangedPassword == 0)
            //{
            //    return RedirectToAction("ChangePassword", "Manage");
            //}
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.test = db.Users;
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}