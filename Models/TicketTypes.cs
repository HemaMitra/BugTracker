using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketTypes
    {
        public TicketTypes()
        {
            this.Tickets = new HashSet<Tickets>();            
        }
        public int Id { get; set; }
        [Required]
        public string TicketName { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}