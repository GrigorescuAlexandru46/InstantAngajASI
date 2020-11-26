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
            Candidate candidate = GetCandidate();

            var allJobList = (from job in db.Jobs
                              select job
                             ).ToList();

            List<Job> recommendedJobList = new List<Job>();
            List<Job> remainingJobList = new List<Job>();
            foreach (Job job in allJobList)
            {
                if (job.CityId == candidate.CityId && job.DomainId == candidate.DomainId && job.DegreeId == candidate.DegreeId)
                {
                    recommendedJobList.Add(job);
                }
                else
                {
                    remainingJobList.Add(job);
                }
            }

            Dictionary<Job, bool> candidateJobApplications = new Dictionary<Job, bool>();
            foreach (Job job in allJobList)
            {
                candidateJobApplications.Add(job, CandidateHasAppliedForJob(candidate.CandidateId, job.JobId));
            }

            ViewBag.AllJobList = allJobList;
            ViewBag.RecommendedJobList = recommendedJobList;
            ViewBag.RemainingJobList = remainingJobList;
            ViewBag.CandidateJobApplications = candidateJobApplications;

            ViewBag.AllCities = GetAllCities();
            ViewBag.AllDomains = GetAllDomains();
            ViewBag.AllDegrees = GetAllDegrees();

            return View();
        }

        public ActionResult ShowCandidates(int id)
        {
            Job job = db.Jobs.Find(id);

            Dictionary<Candidate, bool> isIdealCandidateMap = new Dictionary<Candidate, bool>();
            foreach (Candidate candidate in job.Candidates)
            {
                isIdealCandidateMap.Add(candidate, candidate.CityId == job.CityId && candidate.DomainId == job.DomainId && candidate.DegreeId == job.DegreeId);
            }

            ViewBag.CandidateList = job.Candidates;
            ViewBag.IsIdealCandidateMap = isIdealCandidateMap;

            return View();
        }

        public ActionResult ShowApplications()
        {
            ICollection<Job> appliedJobList = GetCandidate().Jobs;

            ViewBag.AppliedJobList = appliedJobList;

            return View();
        }

        [HttpPost]
        public ActionResult Search(FormCollection form)
        {
            ViewBag.Test = form;

            TempData["CityId"] = form["SearchCity"];
            TempData["DomainId"] = form["SearchDomain"];
            TempData["DegreeId"] = form["SearchDegree"];
            return RedirectToAction("Results", "Job");
        }

        public ActionResult Results()
        {
            var jobList = (from job in db.Jobs
                           select job
                           ).ToList();

            City searchCity = (string)TempData["CityId"] != "" ? db.Cities.Find(ConvertStringToInt((string)TempData["CityId"])) : null;
            Domain searchDomain = (string)TempData["DomainId"] != "" ? db.Domains.Find(ConvertStringToInt((string)TempData["DomainId"])) : null;
            Degree searchDegree = (string)TempData["DegreeId"] != "" ? db.Degrees.Find(ConvertStringToInt((string)TempData["DegreeId"])) : null;

            jobList.RemoveAll(job => (searchCity != null && job.CityId != searchCity.CityId)
                                        || (searchDomain != null && job.DomainId != searchDomain.DomainId)
                                        || (searchDegree != null && job.DegreeId != searchDegree.DegreeId)
                             );

            Candidate candidate = GetCandidate();
            Dictionary<Job, bool> candidateJobApplications = new Dictionary<Job, bool>();
            foreach (Job job in jobList)
            {
                candidateJobApplications.Add(job, CandidateHasAppliedForJob(candidate.CandidateId, job.JobId));
            }

            ViewBag.CandidateJobApplications = candidateJobApplications;
            ViewBag.JobList = jobList;

            ViewBag.SearchCity = searchCity;
            ViewBag.SearchDomain = searchDomain;
            ViewBag.SearchDegree = searchDegree;

            return View();
        }

        public ActionResult New()
        {
            Job job = new Job();

            job.AllCities = GetAllCities();
            job.AllDomains = GetAllDomains();
            job.AllDegrees = GetAllDegrees();

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
        public IEnumerable<SelectListItem> GetAllDomains()
        {
            var selectList = new List<SelectListItem>();

            var domains = from domain in db.Domains
                          select domain;

            foreach (var domain in domains)
            {
                selectList.Add(new SelectListItem
                {
                    Value = domain.DomainId.ToString(),
                    Text = domain.Name.ToString()
                });
            }

            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllDegrees()
        {
            var selectList = new List<SelectListItem>();

            var degrees = from degree in db.Degrees
                          select degree;

            foreach (var degree in degrees)
            {
                selectList.Add(new SelectListItem
                {
                    Value = degree.DegreeId.ToString(),
                    Text = degree.Name.ToString()
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

        [NonAction]
        public int ConvertStringToInt(string s)
        {
            try
            {
                return Int32.Parse(s);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }
    }
}