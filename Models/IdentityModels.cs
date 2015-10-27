using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        // Default Constructor

        public ApplicationUser()
        {
            this.Tickets = new HashSet<Tickets>(); 
            this.TicketHistory = new HashSet<TicketHistory>();
            this.TicketComments = new HashSet<TicketComments>();
            this.TicketNotification = new HashSet<TicketNotification>();
            this.Projects = new HashSet<Projects>();

        }

        public virtual ICollection<Tickets> Tickets { get; set; }
        public virtual ICollection<TicketHistory> TicketHistory { get; set; }
        public virtual ICollection<TicketComments> TicketComments { get; set; }
        public virtual ICollection<TicketNotification> TicketNotification { get; set; }
        public virtual ICollection<Projects> Projects { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // Declaring Models in ApplicationDbContext
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<TicketPriority> TicketPriority { get; set; }
        public DbSet<TicketTypes> TicketType { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<TicketHistory> TicketHistory { get; set; }
        public DbSet<TicketComments> TicketComments { get; set; }
        public DbSet<TicketNotification> TicketNotification { get; set; }
    }
}