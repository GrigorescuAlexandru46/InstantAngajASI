using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InstantAngaj.Models
{
    public class Domain
    {
        [Key]
        public int DomainId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}