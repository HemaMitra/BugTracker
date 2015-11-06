using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class ProgressViewModel
    {
        public string projectName { get; set; }
        public int totalTickets { get; set; }
        public int unassignedTickets { get; set; }
        public int closedTickets { get; set; }
        public int holdTickets { get; set; }
        public int inProgress { get; set; }
        //public int closedPer { get; set; }
        public double closedPer { get; set; }
    }
}