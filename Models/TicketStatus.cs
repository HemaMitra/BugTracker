using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketStatus
    {
        public TicketStatus()
        {
            this.Tickets = new HashSet<Tickets>();            
        }
        public int Id { get; set; }
        [Required]
        public string StatusName { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }

    }
}