﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using BugTracker.Controllers.Helpers;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.Hubs;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HistoryHelper histHelper = new HistoryHelper();
        
        // GET: Tickets
        public ActionResult Index()
        {
            if(User.IsInRole("ProjectManager") || User.IsInRole("Developer"))
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                //var proj = user.Projects.ToList();
                var proj = user.Projects.Where(ap => ap.ProjectArchieved == false);
                var userTickets = proj.SelectMany(p => p.Tickets).AsQueryable();
                
                return View(userTickets.ToList());
            }

            if (User.IsInRole("Submitter"))
            {
                //var proj1 = db.Projects.ToList();
                var proj1 = db.Projects.Where(a => a.ProjectArchieved == false).ToList();
                var userTickets1 = proj1.SelectMany(p => p.Tickets).AsQueryable();
                userTickets1 = userTickets1.Where(t => t.OwnerUserId == User.Identity.GetUserId());
                return View(userTickets1.ToList());
            }

            //var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketStatus).Include(t => t.TicketType);


            var allProj = db.Projects.Where(p => p.ProjectArchieved == false).ToList();
            var tickets = allProj.SelectMany(t => t.Tickets); 
            
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            // Show only those projects on which the submitter is assigned to
            var user = db.Users.Find(User.Identity.GetUserId());
            var proj = user.Projects.ToList();
            var proj1 = proj.Where(l => l.ProjectArchieved == false);

            ViewBag.ProjectId = new SelectList(proj1 , "Id", "ProjectName");

            if (User.IsInRole("Admin"))
            {
                var admProj = db.Projects.Where(l => l.ProjectArchieved == false).ToList();
                ViewBag.ProjectId = new SelectList(admProj, "Id", "ProjectName");

                if (User.Identity.GetUserName() == "guestadmin@coderfoundry.com")
                {
                    ViewBag.ProjectId = new SelectList(proj1, "Id", "ProjectName");
                }
            } 

            if (User.IsInRole("Submitter"))
            {
                var subProj = db.Projects.ToList();
                var subProj1 = subProj.Where(l => l.ProjectArchieved == false);
                ViewBag.ProjectId = new SelectList(subProj1, "Id", "ProjectName");
            }
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "TicketName");
            //Hema
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "PriorityName");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,ProjectId,TicketTypeId,TicketPriorityId,OwnerUserId")]
                        Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                tickets.Created = System.DateTimeOffset.Now;
                tickets.Updated = null;
                tickets.TicketStatusId = 4;//Unassigned
                db.Tickets.Add(tickets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", tickets.ProjectId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "TicketName", tickets.TicketTypeId);
            ViewBag.TicketStatusId = new SelectList(db.TicketPriority, "Id", "PriorityName", tickets.TicketPriorityId);
            return View(tickets);
        }
        
        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            UserRoleHelpers roleHelpers = new UserRoleHelpers();
            UserProjectHelpers helper = new UserProjectHelpers();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            var projId = db.Projects.Find(tickets.ProjectId);
            var projUsers = projId.Users.ToList();
            var resultList = new List<ApplicationUser>(); 

            foreach (var user in projUsers)
            {
                if (roleHelpers.IsUserInRole(user.Id,"Developer"))
                {
                    resultList.Add(user);
                }
            }

            ViewBag.AssignedToUserId = new SelectList(resultList, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "TicketName", tickets.TicketTypeId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "PriorityName", tickets.TicketPriorityId);
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Tickets tickets)
        {
            // Get old values for TicketHistory table
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == tickets.Id);
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.GetUserName();
            
            histHelper.CreateHistory(oldTicket,tickets,userId,userName);

            // ForSignalR 

            //SignalRNotiHub sr = new SignalRNotiHub();
            //var srUser = db.Users.Find(tickets.AssignedToUserId);
            //sr.SendNotifications(srUser.UserName,"Check Assigned Ticket Id : " + tickets.Id + ".");
           
            if (ModelState.IsValid)
            {
                tickets.Updated = System.DateTimeOffset.Now;

                if (tickets.AssignedToUserId != null && tickets.TicketStatusId == 4)
                {
                    tickets.TicketStatusId = 3;// Assigned 
                }

                db.Tickets.Attach(tickets);
                db.Entry(tickets).Property("Updated").IsModified = true;
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", tickets.ProjectId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "TicketName", tickets.TicketTypeId);
            return View(tickets);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tickets tickets = db.Tickets.Find(id);
            db.Tickets.Remove(tickets);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CreateComments(TicketComments tc,HttpPostedFileBase image,string backUrl)
        {
            // uploading image
            if (image != null && image.ContentLength > 0)
            { 
                // Check the file name for allowed file types
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp" && ext != ".doc" && ext != ".pdf")
                {
                    ModelState.AddModelError("image", "Invalid Format");
                }
            }

            if (ModelState.IsValid)
            {
                // Saving Image
                if (image != null)
                {
                    // relative server path
                    var filePath = "/Uploads/";

                    // path on physical drive on server
                    var absPath = Server.MapPath("~" + filePath);

                    // media URL for relative path
                    tc.FileUrl = filePath + "/" + image.FileName;

                    // save image
                    Directory.CreateDirectory(absPath);
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }
                tc.CommentsCreated = System.DateTimeOffset.Now;
                tc.FilePath = null;
                db.TicketComments.Add(tc);
                db.SaveChanges();


                // Notification
                var ticket = db.Tickets.Find(tc.TicketId);

                if(ticket.TicketStatus.StatusName != "Unassigned")
                { 
                    var userId = User.Identity.GetUserId();
                    string notiReci = histHelper.getAssignedUserEmail(ticket.AssignedToUserId);
                    string notiMessage = "Hello " + notiReci + ". A new comment has been created for the following ticket <U>" + ticket.Id + "</U>";
                    histHelper.InitializeNoti(ticket.Id, userId, notiReci, notiMessage);
                }
            }
            if (backUrl.Equals("Edit"))
            {
                return RedirectToAction("Edit", "Tickets", new { id = tc.TicketId });
            }
            return RedirectToAction("Details", "Tickets", new { id = tc.TicketId });
        }

        // Edit Comment
        public ActionResult EditComment(TicketComments tc)
        {
            if (ModelState.IsValid)
            {
            
                TicketComments Comment = db.TicketComments.Find(tc.Id);
                Comment.UpdatedBy = User.Identity.GetUserName();
                Comment.UpdateDate = System.DateTimeOffset.Now;
                Comment.Comments = tc.Comments;
                Comment.FileDesc = tc.FileDesc;
                db.TicketComments.Attach(Comment);

                db.Entry(Comment).Property("UpdateDate").IsModified = true;
                db.Entry(Comment).Property("Comments").IsModified = true;
                db.Entry(Comment).Property("UpdatedBy").IsModified = true;
                db.Entry(Comment).Property("FileDesc").IsModified = true;
                db.SaveChanges();

                // Notification
                var ticket = db.Tickets.Find(tc.TicketId);

                if (ticket.TicketStatus.StatusName != "Unassigned")
                {
                    var userId = User.Identity.GetUserId();
                    string notiReci = histHelper.getAssignedUserEmail(ticket.AssignedToUserId);
                    string notiMessage = "Hello " + notiReci + ". A comment has been changed for the following ticket <U>" + ticket.Id +
                        "</U>";
                    histHelper.InitializeNoti(ticket.Id, userId, notiReci, notiMessage);
                }
            }
            return RedirectToAction("Details", "Tickets", new { id = tc.TicketId });
        }

        // Called from Details Page for Users
        public ActionResult ProjectTickets(int projectId)
        {
            Projects project = db.Projects.Find(projectId);
            var tickets = project.Tickets.ToList();
            return View(tickets);
        }

        // Notifications Index Page
        public ActionResult NotiIndex(string userId)
        {
            var user = db.Users.Find(userId);
            var tn = db.TicketNotification.Where(u => u.Recipient == user.Email).OrderBy(t => t.TicketId).ToList();
            return View(tn);
        }

        //// Get for Tickets Graph
        public ActionResult TicketProgress()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var proj = user.Projects.Where(p => p.ProjectArchieved == false).ToList();

            if (User.IsInRole("Admin"))
            {
                proj = db.Projects.Where(p => p.ProjectArchieved == false).ToList();
            }

            var pvmList = new List<ProgressViewModel>();
            
            foreach (var item in proj)
            {
                var pvm = new ProgressViewModel();
                var projTickets = item.Tickets.ToList();
                pvm.projectName= item.ProjectName;

                pvm.totalTickets = projTickets.Count();
                pvm.closedTickets = projTickets.Where(c => c.TicketStatus.StatusName == "Close").Count();
                pvm.unassignedTickets = projTickets.Where(c => c.TicketStatus.StatusName == "Unassigned").Count();
                pvm.holdTickets = projTickets.Where(c => c.TicketStatus.StatusName == "On Hold").Count();
                var asgn = projTickets.Where(a => a.TicketStatus.StatusName == "Assigned").Count();
                var reasgn = projTickets.Where(a => a.TicketStatus.StatusName == "Reassigned").Count();
                var test = projTickets.Where(a => a.TicketStatus.StatusName == "Testing").Count();
                var open = projTickets.Where(a => a.TicketStatus.StatusName == "Open").Count();
                pvm.inProgress = asgn + reasgn + test + open;

                if (pvm.totalTickets > 0)
                {
                    //pvm.closedPer = Convert.ToInt32(((double)pvm.closedTickets / (double)pvm.totalTickets) * 100);
                    pvm.closedPer = Convert.ToDouble(((double)pvm.closedTickets / (double)pvm.totalTickets) * 100);
                    pvm.closedPer = Math.Round(pvm.closedPer, 2);
                }
                else
                {
                    pvm.closedPer = 0;
                }
                pvmList.Add(pvm);
            }
            return View(pvmList);    
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
