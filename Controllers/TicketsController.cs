using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        public ActionResult Index()
        {
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
            var proj = db.Projects.Where(l=>l.ProjectArchieved == false);
            ViewBag.ProjectId = new SelectList(proj , "Id", "ProjectName");
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
        public ActionResult Create([Bind(Include = "Id,Title,Description,ProjectId,TicketTypeId,TicketPiorityId,OwnerUserId")]
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
            ViewBag.TicketStatusId = new SelectList(db.TicketPriority, "Id", "PriorityName", tickets.TicketPiorityId);
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
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", tickets.ProjectId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "TicketName", tickets.TicketTypeId);
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPiorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
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
