using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public System.DateTimeOffset ChangedDate { get; set; }
        public string UserId { get; set; }

        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}