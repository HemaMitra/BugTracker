using BugTracker.Controllers.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserRoleHelpers roleHelpers = new UserRoleHelpers();

        //// Get
        //[Authorize(Roles = "Admin, ProjectManager")]
        //public ActionResult ProjectList()
        //{
        //    return View(db.Projects.ToList());
        //}

        // Get
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult ProjectList()
        {
            if(User.IsInRole("ProjectManager"))
            { 
                var user = db.Users.Find(User.Identity.GetUserId());
                var proj = user.Projects.ToList();
                return View(proj.ToList());
            }
            return View(db.Projects.ToList());
        }


        // Get
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult EditUserProject(int projectId)
        {
            var project = db.Projects.Find(projectId);
            UserProjectModel projectModel = new UserProjectModel();
            // Creating an object of helper class
            UserProjectHelpers helper = new UserProjectHelpers();
            // Calling Helper Method
            var selected = helper.ListOfUsers(projectId).Select(l=>l.Id);

            var userRole = roleHelpers.UsersInRole("Developer");

            if (User.IsInRole("Admin"))
            {
                projectModel.ApplicationUser = new MultiSelectList(db.Users, "Id", "FirstName", selected);
            }
            if (User.IsInRole("ProjectManager"))
            {
                projectModel.ApplicationUser = new MultiSelectList(userRole, "Id", "FirstName", selected);
            }
            projectModel.Project = project;
            return View(projectModel);
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        public ActionResult EditUserProject(int projectId, UserProjectModel projectModel)
        {
            UserProjectHelpers helper = new UserProjectHelpers();
            if (ModelState.IsValid)
            {
                string[] empt = { };
                projectModel.SelectedUsers = projectModel.SelectedUsers ?? empt;

                foreach (var user in db.Users)
                {
                    if (projectModel.SelectedUsers.Contains(user.Id))
                    {
                        helper.AddUserToProject(projectId, user.Id);
                    }
                    //If not selected remove
                    else
                    {
                        helper.RemoveUserFromProject(projectId, user.Id);
                    }
                }
            }
            return RedirectToAction("ProjectList");
        }


        // GET: Admin
        [Authorize(Roles="Admin")]
        public ActionResult AdminIndex() 
        {
            return View(db.Users.ToList());
        }

        [Authorize(Roles="Admin")]
        public ActionResult EditUserRole(string userId)
        {
            var user = db.Users.Find(userId);
            AdminViewModel adminModel = new AdminViewModel();
            // Creating an object of helper class
            UserRoleHelpers helper = new UserRoleHelpers();
            // Calling Helper Method
            var selected = helper.ListUserRoles(userId);
            adminModel.Roles = new MultiSelectList(db.Roles, "Name", "Name", selected);
            adminModel.User = user;

            return View(adminModel);
        }
        
        [Authorize(Roles="Admin")]
        [HttpPost]
        public ActionResult EditUserRole(string userId, AdminViewModel adminModel)
        {
            UserRoleHelpers helper = new UserRoleHelpers();
            if(ModelState.IsValid)
            {
                string[] empt = { };
                adminModel.SelectedRoles = adminModel.SelectedRoles ?? empt;
                foreach (var role in db.Roles.ToList())
                {
                    //If selected but user doesn't have, add
                    if (adminModel.SelectedRoles.Contains(role.Name))
                    {
                        helper.AddUserToRole(userId, role.Name);
                    }
                    //If not selected remove
                    else
                    {
                        helper.RemoveuserFromRole(userId, role.Name);
                    }
                }
            }
            return RedirectToAction("AdminIndex");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult CreateProject(Projects Project,string ProjectName)
        {
            if (ModelState.IsValid)
            {
                Project.ProjectName = ProjectName;
                Project.ProjectArchieved = false;
                db.Projects.Add(Project);
                db.SaveChanges();
                TempData["Message"] = "Project Created.";
                return RedirectToAction("AdminIndex");
            }

            return View();
        }

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult EditProject()
        {
            return View(db.Projects.ToList());
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        public ActionResult EditProject(int Id, string ProjectName,bool? ProjectArchieved)
        {
            Projects project = db.Projects.Find(Id);
            project.ProjectName = ProjectName;
            project.ProjectArchieved = ProjectArchieved??false;

            db.Projects.Attach(project);
            db.Entry(project).Property("ProjectName").IsModified = true;
            db.Entry(project).Property("ProjectArchieved").IsModified = true;
            db.SaveChanges();
            TempData["Message"] = "Project Updated.";
            return RedirectToAction("EditProject");
        }
























    }
}