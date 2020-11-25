using InstantAngaj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstantAngaj.Controllers
{
    public class CandidateController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Show(int id)
        {
            Candidate candidate = db.Candidates.Find(id);

            return View(candidate);
        }
    }
}