﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class UserProjectModel
    {

        public Projects Project { get; set; }
        public MultiSelectList ApplicationUser { get; set; }
        public string[] SelectedUsers { get; set; }
    }
}