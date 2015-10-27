using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Controllers.Helpers
{
    public class UserProjectHelpers
    {
        ApplicationUser au = new ApplicationUser();
        ApplicationDbContext db = new ApplicationDbContext();
        
        Projects proj = new Projects();

        public bool AddUserToProject(int projectId, string userId)
        {
            au = db.Users.Find(userId);
            proj = db.Projects.Find(projectId);
            if (CheckUserProj(projectId, userId))
            {
                au.Projects.Add(proj);
                db.SaveChanges();
                return true;
            }
            else{
                return false;
            }
        }

        public bool RemoveUserFromProject(int projectId, string userId)
        {
            au = db.Users.Find(userId);
            proj = db.Projects.Find(projectId);
            if (CheckUserProj(projectId, userId))
            {
                au.Projects.Remove(proj);
                db.SaveChanges();
                return true;
            }
            else {
                return false;
            }
            
        }

        public IList<Projects> ListOfProjects(string userId)
        {
            au = db.Users.Find(userId);
            var usrProjects = au.Projects.ToList();
            return usrProjects;
        }

        public IList<ApplicationUser> ListOfUsers(int projectId)
        {
            proj = db.Projects.Find(projectId);
            



            var projUsers = proj.Users.ToList();
            return projUsers;
        }

        public bool CheckUserProj(int projectId, string userId)
        {
            au = db.Users.Find(userId);
            var res = au.Projects.Where(p => p.Id == projectId);
            if(res != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        
        
        
        
        
        
        
        
        
        
    }
}