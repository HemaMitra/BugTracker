using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class ProgressViewModel
    {
        //public ProgressViewModel()
        //{
        //    projectName = new List<string>();
        //    totalTickets = new List<int>();
        //    unassignedTickets = new List<int>();
        //    closedTickets = new List<int>();
        //    holdTickets = new List<int>();
        //    inProgress = new List<int>();
        //}
        
        public string projectName { get; set; }
        public int totalTickets { get; set; }
        public int unassignedTickets { get; set; }
        public int closedTickets { get; set; }
        public int holdTickets { get; set; }
        public int inProgress { get; set; }
        public int closedPer { get; set; }
    }
}