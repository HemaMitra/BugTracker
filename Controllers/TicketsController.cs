using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        public ActionResult Index()
        {
            if(User.IsInRole("ProjectManager") || User.IsInRole("Developer") || (User.IsInRole("Submitter")))
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                var proj = user.Projects.ToList();
                var userTickets = proj.SelectMany(p => p.Tickets).AsQueryable();

                if (User.IsInRole("Submitter"))
                {
                    userTickets = userTickets.Where(t => t.OwnerUserId == User.Identity.GetUserId());
                }


                return View(userTickets.ToList());
            }
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketStatus).Include(t => t.TicketType);

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
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName");
           
            // Show only those projects on which the submitter is assigned to
            var user = db.Users.Find(User.Identity.GetUserId());
            var proj = user.Projects.ToList();
            var proj1 = proj.Where(l => l.ProjectArchieved == false);


            //var proj = db.Projects.Where(l=>l.ProjectArchieved == false);
            ViewBag.ProjectId = new SelectList(proj1 , "Id", "ProjectName");
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName");
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
                tickets.TicketStatusId = 4;
                db.Tickets.Add(tickets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", tickets.ProjectId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "TicketName", tickets.TicketTypeId);
            ViewBag.TicketStatusId = new SelectList(db.TicketPriority, "Id", "PriorityName", tickets.TicketPriorityId);
            return View(tickets);
        }
        
        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
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
            var projId = db.Projects.Find(tickets.ProjectId);
            var projUsers = projId.Users.ToList();

            //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.AssignedToUserId = new SelectList(projUsers, "Id", "FirstName", tickets.AssignedToUserId);
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", tickets.ProjectId);
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
            if (ModelState.IsValid)
            {
                tickets.Updated = System.DateTimeOffset.Now;

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
                Comment.UpdatedBy = User.Identity.GetUserId();
                Comment.UpdateDate = System.DateTimeOffset.Now;
                Comment.Comments = tc.Comments;
                Comment.FileDesc = tc.FileDesc;
                db.TicketComments.Attach(Comment);

                db.Entry(Comment).Property("UpdateDate").IsModified = true;
                db.Entry(Comment).Property("Comments").IsModified = true;
                db.Entry(Comment).Property("UpdatedBy").IsModified = true;
                db.Entry(Comment).Property("FileDesc").IsModified = true;
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Tickets", new { id = tc.TicketId });
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
