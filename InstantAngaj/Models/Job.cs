using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstantAngaj.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Salary { get; set; }

        public string MinimumExperience { get; set; }

        //
        public int EmployerId { get; set; }

        public int CityId { get; set; }

        public int DomainId { get; set; }

        public int DegreeId { get; set; }

        //
        public virtual Employer Employer { get; set; }
        public virtual City City { get; set; }
        public virtual Domain Domain { get; set; }
        public virtual Degree Degree { get; set; }
        public virtual ICollection<Candidate> Candidates { get; set; }

        //
        public virtual IEnumerable<SelectListItem> AllCities { get; set; }
        public virtual IEnumerable<SelectListItem> AllDomains { get; set; }
        public virtual IEnumerable<SelectListItem> AllDegrees { get; set; }
    }
}