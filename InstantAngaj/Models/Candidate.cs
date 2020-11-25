using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InstantAngaj.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        //
        public string UserId { get; set; }
        public int CityId { get; set; }
        public int DomainId { get; set; }
        public int DegreeId { get; set; }

        //
        public virtual City City { get; set; }
        public virtual Domain Domain { get; set; }
        public virtual Degree Degree { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }

        //public string Abc { get; set; }
    }
}