﻿using System;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Web.Security;
using CorporationHR.Helpers;
using CorporationHR.Models;
using WebMatrix.WebData;

namespace CorporationHR.Context
{
    public class DatabaseInitializator : DbMigrationsConfiguration<CorporationHrDbContext>
    {
        public DatabaseInitializator()
        {
            this.AutomaticMigrationsEnabled = true;;
        }

        protected override void Seed(CorporationHrDbContext context)
        {
            AttachSimpleAuth();
            AddUsers();
            AddClerences(context);
            AddTechnologies(context);
            AddClearencesToUsers(context);
            AddClearancesToTechs(context);
        }

        private void AttachSimpleAuth()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
        }

        private void AddTechnologies(CorporationHrDbContext context)
        {
            if (context.Technologies.ToList().Any()) return;

            var newPublicTech = new Technology()
                                {
                                    TechnologyInternalId = "This is GUID of some public tech",
                                    ShortDescription = "This is a short description of some public tech",
                                    FullDescription = "This is a long description of some public tech",
                                    CreatedOn = DateTime.Now,
                                    IsCompleted = false
                                };

            var newConfidentialTech = new Technology()
                                      {
                                          TechnologyInternalId = "This is GUID of some confidential tech",
                                          ShortDescription = "This is a short description of some confidential tech",
                                          FullDescription = "This is a long description of some confidential tech",
                                          CreatedOn = DateTime.Now,
                                          IsCompleted = false
                                      };

            var newSecretTech = new Technology()
                                {
                                    TechnologyInternalId = "This is GUID of some secret tech",
                                    ShortDescription = "This is a short description of some secret tech",
                                    FullDescription = "This is a long description of some secret tech",
                                    CreatedOn = DateTime.Now,
                                    IsCompleted = false
                                };

            var newTopSecretTech = new Technology()
                                   {
                                       TechnologyInternalId = "This is GUID of some TopSecret tech",
                                       ShortDescription = "This is a short description of some TopSecret tech",
                                       FullDescription = "This is a long description of some TopSecret tech",
                                       CreatedOn = DateTime.Now,
                                       IsCompleted = false
                                   };

            context.Technologies.Add(newPublicTech);
            context.Technologies.Add(newConfidentialTech);
            context.Technologies.Add(newSecretTech);
            context.Technologies.Add(newTopSecretTech);
            context.SaveChanges();
        }

        private void AddClearancesToTechs(CorporationHrDbContext context)
        {
            var techs = context.Technologies.ToList();
            var clearences = context.Clearences.ToList();
            if (!techs.Any() || !clearences.Any()) return;

            foreach (var tech in techs)
            {
                if (tech.ShortDescription.ToLower().Contains("public")) { tech.ClearenceModel = clearences.Single(x => x.ClearenceName.Equals("Public")); }
                else if (tech.ShortDescription.ToLower().Contains("confidential")) { tech.ClearenceModel = clearences.Single(x => x.ClearenceName.Equals("Confidential")); }
                else if (tech.ShortDescription.ToLower().Contains("topsecret")) { tech.ClearenceModel = clearences.Single(x => x.ClearenceName.Equals("Top Secret")); }
                else if (tech.ShortDescription.ToLower().Contains("secret")) { tech.ClearenceModel = clearences.Single(x => x.ClearenceName.Equals("Secret")); }
                context.SaveChanges();
            }
        }

        private void AddUsers()
        {
            var roles = (SimpleRoleProvider) Roles.Provider;
            if (!roles.RoleExists("Active")) roles.CreateRole("Active");
            if (!roles.RoleExists("Disabled")) roles.CreateRole("Disabled");
            
            if (!WebSecurity.UserExists("admin")) { WebSecurity.CreateUserAndAccount("admin", "test", new { FirstName = "Admin", LastName = "Admin", Email = "admin@admin.ad"}); }
            if (!WebSecurity.UserExists("userTest1")) { WebSecurity.CreateUserAndAccount("userTest1", "qwerty", new { FirstName = "test1", LastName = "testos1", Email = "test1@test.ts"}); }
            if (!WebSecurity.UserExists("userTest2"))
            {
                WebSecurity.CreateUserAndAccount("userTest2", "qwerty", new { FirstName = "test2", LastName = "testos2", Email = "test2@test.ts"});
                roles.AddUsersToRoles(new[] { "admin", "userTest1", "userTest2" }, new[] { "Active" });
            }

        }

        private void AddClerences(CorporationHrDbContext context)
        {
            var listOfClearencesInDb = context.Clearences.Where(x => x.ClearenceId > 0).ToList(); //Don't ask
            if (listOfClearencesInDb.Any())
            {
                listOfClearencesInDb.ForEach(x => GeneralHelper.Clearences.Add(x.ClearenceId, x.ClearenceName));
                return;
            }

            var clearenceGreen = new ClearenceModel { ClearenceName = "Public" };
            var clearenceOrange = new ClearenceModel { ClearenceName = "Confidential" };
            var clearenceRed = new ClearenceModel { ClearenceName = "Secret" };
            var clearenceBlack = new ClearenceModel { ClearenceName = "Top Secret" };

            context.Clearences.Add(clearenceGreen);
            context.Clearences.Add(clearenceOrange);
            context.Clearences.Add(clearenceRed);
            context.Clearences.Add(clearenceBlack);
            context.SaveChanges();

            context.Clearences.ToList().ForEach(x => GeneralHelper.Clearences.Add(x.ClearenceId, x.ClearenceName));
        }

        private void AddClearencesToUsers(CorporationHrDbContext context)
        {
            var users = context.UserProfiles.ToList();
            var clearences = context.Clearences.ToList();
            if (!users.Any() || !clearences.Any()) return;

            foreach (var user in users)
            {
                user.ClearenceModel = !user.UserName.Equals("admin") ? clearences.Single(x => x.ClearenceName.Equals("Public")) : clearences.Single(x => x.ClearenceName.Equals("Top Secret"));
                context.SaveChanges();
            }
        }
    }
}