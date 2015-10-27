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
