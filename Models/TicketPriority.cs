using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketPriority
    {
        // Same priority can be assigned to Multiple Tickets
        public TicketPriority()
        {
            this.Tickets = new HashSet<Tickets>();            
        }
        public int Id { get; set; }
        [Required]
        public string PriorityName { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}