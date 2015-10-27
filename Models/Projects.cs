using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Projects
    {
        public Projects()
        {
            // One Project Can Have Multiple Tickets and Multiple Users. So creating Collection of Tickets and Users.
            this.Tickets = new HashSet<Tickets>();
            this.Users = new HashSet<ApplicationUser>();
        }
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public bool ProjectArchieved { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}