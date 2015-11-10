namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "hemamitra@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "hemamitra@gmail.com",
                    Email = "hemamitra@gmail.com",
                    FirstName = "Hema",
                    LastName = "Mitra"
                }, "DBconnect-1");
            }

            // Add Admin to the Roles Table
            var userId = userManager.FindByEmail("hemamitra@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            // GuestAdmin
            
            if (!context.Users.Any(u => u.Email == "guestadmin@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "guestadmin@coderfoundry.com",
                    Email = "guestadmin@coderfoundry.com",
                    FirstName = "Guest",
                    LastName = "Admin"
                }, "Guest-1");
            }

            // Add GuestAdmin to the Roles Table
            userId = userManager.FindByEmail("guestadmin@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Admin");
            

            // Role for Project Manager
            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "moderator@coderfoundry.com",
                    Email = "moderator@coderfoundry.com",
                    FirstName = "ProjectManager",
                    LastName = "P"
                }, "Password-1");
            }

            // Add ProjectManager to the Roles Table
            userId = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(userId, "ProjectManager");

            // Project Manager 2
            if (!context.Users.Any(u => u.Email == "vickym@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "vickym@gmail.com",
                    Email = "vickym@gmail.com",
                    FirstName = "Vicky PM",
                    LastName = "PM"
                }, "Bugtracker-1");
            }

            // Add ProjectManager to the Roles Table
            userId = userManager.FindByEmail("vickym@gmail.com").Id;
            userManager.AddToRole(userId, "ProjectManager");

            // Guest Project Manager
            if (!context.Users.Any(u => u.Email == "guestPM@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "guestPM@coderfoundry.com",
                    Email = "guestPM@coderfoundry.com",
                    FirstName = "Guest PM",
                    LastName = "PM"
                }, "Guest-1");
            }

            // Add ProjectManager to the Roles Table
            userId = userManager.FindByEmail("guestPM@coderfoundry.com").Id;
            userManager.AddToRole(userId, "ProjectManager");



            // Role For Developer
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Users.Any(u => u.Email == "hemaakolte@yahoo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "hemaakolte@yahoo.com",
                    Email = "hemaakolte@yahoo.com",
                    FirstName = "Developer",
                    LastName = "D"
                }, "Developer-1");
            }

            // Add Developer to the Roles Table
            userId = userManager.FindByEmail("hemaakolte@yahoo.com").Id;
            userManager.AddToRole(userId, "Developer");


            // Developer 2
            if (!context.Users.Any(u => u.Email == "hmitra_411@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "hmitra_411@gmail.com",
                    Email = "hmitra_411@gmail.com",
                    FirstName = "hmitra Developer",
                    LastName = "D"
                }, "Bugtracker-1");
            }

            // Add Developer to the Roles Table
            userId = userManager.FindByEmail("hmitra_411@gmail.com").Id;
            userManager.AddToRole(userId, "Developer");

            // Developer 3
            if (!context.Users.Any(u => u.Email == "rano_l@yahoo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "rano_l@yahoo.com",
                    Email = "rano_l@yahoo.com",
                    FirstName = "Rano Developer",
                    LastName = "D"
                }, "Bugtracker-1");
            }

            // Add Developer to the Roles Table
            userId = userManager.FindByEmail("rano_l@yahoo.com").Id;
            userManager.AddToRole(userId, "Developer");

            // Guest Developer
            if (!context.Users.Any(u => u.Email == "guestdeveloper@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "guestdeveloper@coderfoundry.com",
                    Email = "guestdeveloper@coderfoundry.com",
                    FirstName = "Guest Dev",
                    LastName = "D"
                }, "Guest-1");
            }

            // Add Developer to the Roles Table
            userId = userManager.FindByEmail("guestdeveloper@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Developer");

            // Role for Submitter
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            if (!context.Users.Any(u => u.Email == "hemakolte@hotmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "hemakolte@hotmail.com",
                    Email = "hemakolte@hotmail.com",
                    FirstName = "Submitter",
                    LastName = "S"
                }, "Submitter-1");
            }
            // Add Submitter to the Roles Table
            userId = userManager.FindByEmail("hemakolte@hotmail.com").Id;
            userManager.AddToRole(userId, "Submitter");

            // Submitter 2
            // Role for Submitter
            if (!context.Users.Any(u => u.Email == "hemaskolte@yahoo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "hemaskolte@yahoo.com",
                    Email = "hemaskolte@yahoo.com",
                    FirstName = "Submitter2",
                    LastName = "S"
                }, "Bugtracker-1");
            }
            // Add Submitter to the Roles Table
            userId = userManager.FindByEmail("hemaskolte@yahoo.com").Id;
            userManager.AddToRole(userId, "Submitter");

            // Guest Submitter

            if (!context.Users.Any(u => u.Email == "guestsubmitter@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "guestsubmitter@coderfoundry.com",
                    Email = "guestsubmitter@coderfoundry.com",
                    FirstName = "Guest Submitter",
                    LastName = "S"
                }, "Guest-1");
            }
            // Add Submitter to the Roles Table
            userId = userManager.FindByEmail("guestsubmitter@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Submitter");

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
