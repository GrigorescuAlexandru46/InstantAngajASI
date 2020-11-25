using InstantAngaj.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstantAngaj.Controllers
{
    public class JobController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            int ownEmployerId = GetEmployer().EmployerId;

            var jobList = (from job in db.Jobs
                           where job.EmployerId == ownEmployerId
                           select job
                          ).ToList();

            ViewBag.JobList = jobList;

            return View();
        }

        public ActionResult Show(int id)
        {
            Job job = db.Jobs.Find(id);

            if (User.IsInRole("Candidate"))
            {
                ViewBag.CandidateHasApplied = CandidateHasAppliedForJob(GetCandidate().CandidateId, job.JobId);
            }

            return View(job);
        }

        public ActionResult ShowAll()
        {
            var jobList = (from job in db.Jobs
                           select job
                          ).ToList();


            Dictionary<Job, bool> candidateJobApplications = new Dictionary<Job, bool>();
            Candidate candidate = GetCandidate();
            foreach (Job job in jobList)
            {
                candidateJobApplications.Add(job, CandidateHasAppliedForJob(candidate.CandidateId, job.JobId));
            }

            ViewBag.JobList = jobList;
            ViewBag.CandidateJobApplications = candidateJobApplications;

            return View();
        }

        public ActionResult New()
        {
            Job job = new Job();

            job.AllCities = GetAllCities();

            return View(job);
        }

        [HttpPost]
        public ActionResult New(Job job)
        {
            try
            {
                job.EmployerId = GetEmployer().EmployerId;

                db.Jobs.Add(job);
                db.SaveChanges();

                TempData["Message"] = "Jobul a fost creat cu succes!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                TempData["Message"] = "A aparut o eroare. Crearea a esuat";

                Console.Write(e);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult AddCandidate(int id)
        {
            try
            {
                Job job = db.Jobs.Find(id);
                Candidate candidate = GetCandidate();

                job.Candidates.Add(candidate);

                db.SaveChanges();

                TempData["Message"] = "Ai aplicat cu succes!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                TempData["Message"] = "A aparut o eroare. Aplicarea a esuat";

                Console.Write(e);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult RemoveCandidate(int id)
        {
            try
            {
                Job job = db.Jobs.Find(id);
                Candidate candidate = GetCandidate();

                job.Candidates.Remove(candidate);

                db.SaveChanges();

                TempData["Message"] = "Ai renuntat la aplicatie cu succes!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                TempData["Message"] = "A aparut o eroare. Renuntarea a esuat";

                Console.Write(e);
                return RedirectToAction("Index", "Home");
            }
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

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCities()
        {
            var selectList = new List<SelectListItem>();
            
            var cities = from city in db.Cities
                         select city;
            
            foreach (var city in cities)
            {
                selectList.Add(new SelectListItem
                {
                    Value = city.CityId.ToString(),
                    Text = city.Name.ToString()
                });
            }

            return selectList;
        }

        [NonAction]
        public bool CandidateHasAppliedForJob(int candidateId, int jobId)
        {
            Candidate candidate = db.Candidates.Find(candidateId);
            Job job = db.Jobs.Find(jobId);

            if (candidate.Jobs.Contains(job))
            {
                return true;
            }

            return false;
        }
    }
}