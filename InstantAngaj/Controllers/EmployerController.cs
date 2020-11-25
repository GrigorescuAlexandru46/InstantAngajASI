using InstantAngaj.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstantAngaj.Controllers
{
    public class EmployerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Show(int id)
        {
            Employer employer = db.Employers.Find(id);

            return View(employer);
        }

        public ActionResult ShowOwnProfile()
        {
            return RedirectToAction("Show", "Employer", new { id = GetEmployer().EmployerId });
        }

        [NonAction]
        public Employer GetEmployer()
        {
            string ownUserId = User.Identity.GetUserId();

            var queryEmployers = from employer in db.Employers
                                 where employer.UserId == ownUserId
                                 select employer;

            Employer ownEmployer = queryEmployers.FirstOrDefault();

            return ownEmployer;
        }
    }
}