using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class TicketComments
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        [Required]
        [AllowHtml]
        public string Comments { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
        public string FileDesc { get; set; }
        public System.DateTimeOffset CommentsCreated { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTimeOffset UpdateDate { get; set; }
        
        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }


    }
}