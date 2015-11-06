using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Configuration;

namespace BugTracker.Controllers.Helpers
{
    public class HistoryHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ApplicationUser au1 = new ApplicationUser();
        
        public void CreateHistory(Tickets oldTicket, Tickets newTicket, string userId, string userName)
        {
            

            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                TicketHistory history = new TicketHistory();
                history.TicketId = newTicket.Id;
                history.UserId = userId;
                history.UserName = userName;
                history.ChangedDate = System.DateTimeOffset.Now;
                history.OldValue = oldTicket.TicketType.TicketName;
                history.NewValue = getTicketName(newTicket.TicketTypeId);
                history.Property = "Ticket Type";
                db.TicketHistory.Add(history);

                // Notification
                string notiReci = getAssignedUserEmail(newTicket.AssignedToUserId);
                string notiMessage = "Hello " + notiReci + ". Ticket Type has been changed for the following ticket <U>" + newTicket.Id + "</U>";
                InitializeNoti(newTicket.Id, userId, notiReci, notiMessage);

            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                TicketHistory history1 = new TicketHistory();
                history1.TicketId = newTicket.Id;
                history1.UserId = userId;
                history1.UserName = userName;
                history1.ChangedDate = System.DateTimeOffset.Now;
                history1.OldValue = oldTicket.TicketStatus.StatusName;
                history1.NewValue = getStatusName(newTicket.TicketStatusId);
                history1.Property = "Ticket Status";
                db.TicketHistory.Add(history1);

                // Notification
                
                String notiReci = getAssignedUserEmail(newTicket.AssignedToUserId);
                string notiMessage = "Hello " + notiReci + ". Ticket Status has been changed for the following ticket <U>" + newTicket.Id + 
                    "</U>";
                InitializeNoti(newTicket.Id, userId, notiReci, notiMessage);

            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                TicketHistory history2 = new TicketHistory();
                history2.TicketId = newTicket.Id;
                history2.UserId = userId;
                history2.UserName = userName;
                history2.ChangedDate = System.DateTimeOffset.Now;
                history2.OldValue = oldTicket.TicketPriority.PriorityName;
                history2.NewValue = getPriorityName(newTicket.TicketPriorityId);
                history2.Property = "Ticket Priority";
                db.TicketHistory.Add(history2);

                // Notification
                string notiReci = getAssignedUserEmail(newTicket.AssignedToUserId);
                string notiMessage = "Hello " + notiReci + ". Ticket Priority has been changed for the following ticket <U>" + newTicket.Id + 
                    "</U>";
                InitializeNoti(newTicket.Id, userId, notiReci, notiMessage);
            }
            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                //string empt = "";
                
                TicketHistory history3 = new TicketHistory();
                history3.TicketId = newTicket.Id;
                history3.UserId = userId;
                history3.UserName = userName;
                history3.ChangedDate = System.DateTimeOffset.Now;

                if(oldTicket.AssignedToUserId == null)
                {
                    history3.OldValue = "Unassigned";
                }
                else
                {
                    history3.OldValue = oldTicket.AssignedToUser.FirstName;
                }
                history3.NewValue = getAssignedUser(newTicket.AssignedToUserId);
                history3.Property = "Assigned To";
                db.TicketHistory.Add(history3);

                //Notification
                string notiReci = getAssignedUserEmail(newTicket.AssignedToUserId);
                string notiMessage = "Hello " + notiReci + ". You have been assigned the following ticket <U>" + newTicket.Id + "</U>";
                InitializeNoti(newTicket.Id,userId,notiReci,notiMessage);

                //// Notification
                //TicketNotification noti = new TicketNotification();
                //noti.TicketId = newTicket.Id;
                //noti.UserId = userId;
                //noti.Recipient = getAssignedUserEmail(newTicket.AssignedToUserId);
                //noti.Message = "Hello " + noti.Recipient + ". You have been assigned the following ticket <U>" + newTicket.Id + "</U>";
                //bool result = Notification(noti.Recipient, noti.Message);
                //if (result == true)
                //{
                //    noti.NotiSent = true;
                //    db.TicketNotification.Add(noti);
                //    db.SaveChanges();
                //}

            }
            db.SaveChanges();
        }

        public string getTicketName(int id)
        { 
            
            var ticType = db.TicketType.Find(id);

            return ticType.TicketName;
        }

        public string getStatusName(int id)
        { 
            
            var ticStatus = db.TicketStatus.Find(id);

            return ticStatus.StatusName;
        }

        public string getPriorityName(int id)
        {

            var priName = db.TicketPriority.Find(id);

            return priName.PriorityName;
        }

        public string getAssignedUser(string id)
        {
            au1 = db.Users.Find(id);
            return au1.FirstName;
        }

        public string getAssignedUserEmail(string AssignedToUserId)
        {
            au1 = db.Users.Find(AssignedToUserId);
            return au1.Email;
        }


        public bool InitializeNoti(int ticketId, string userId, string notiReci, string message)
        {
            TicketNotification noti = new TicketNotification();
            noti.TicketId = ticketId;
            noti.UserId = userId;
            noti.Recipient = notiReci;
            noti.Message = message;
            bool result = Notification(noti.Recipient, noti.Message);
            if (result == true)
            {
                noti.NotiSent = true;
                noti.SentDate = System.DateTimeOffset.Now;
                db.TicketNotification.Add(noti);
                db.SaveChanges();
            }

            return true;
        }


        public bool Notification(string recipient, string message)
        {
            IdentityMessage msg = new IdentityMessage();
            EmailService ems = new EmailService();

            msg.Subject = "Ticket Update";
            msg.Destination = recipient;
            msg.Body = message;
            ems.SendAsync(msg);
            return true;
        }

        

    }
}