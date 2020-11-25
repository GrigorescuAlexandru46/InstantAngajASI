using InstantAngaj.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            ViewBag.Age = DateTime.Today.Year - DateTime.ParseExact(candidate.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).Year;

            return View(candidate);
        }

        public ActionResult ShowOwnProfile()
        {
            return RedirectToAction("Show", "Candidate", new { id = GetCandidate().CandidateId });
        }

        [NonAction]
        public Candidate GetCandidate()
        {
            string ownUserId = User.Identity.GetUserId();

            var queryCandidates = from candidate in db.Candidates
                                  where candidate.UserId == ownUserId
                                  select candidate;

            Candidate ownCandidate = queryCandidates.FirstOrDefault();

            return ownCandidate;
        }
    }
}