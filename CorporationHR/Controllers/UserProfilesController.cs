﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CorporationHR.CustomAttribute;
using CorporationHR.Models;
using CorporationHR.Context;
using CorporationHR.Repositories;

namespace CorporationHR.Controllers
{
    [Authorize]
    public class UserProfilesController : Controller
    {
        private readonly UserProfilesRepository _userProfilesRepo;

        public UserProfilesController(ICorporationHrDatabaseContext databaseContext)
        {
            _userProfilesRepo = new UserProfilesRepository(databaseContext);
        }

        // GET: /UserProfiles/
        [CustomAuthorizeRead(CallingController = "User Profiles")]
        public ActionResult Index()
        {
            return View(_userProfilesRepo.All);
        }

        // GET: /UserProfiles/Details/5
        [CustomAuthorizeRead(CallingController = "User Profiles")]
        public ActionResult Details(int id = 0)
        {
            UserProfile userprofile = _userProfilesRepo.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        // GET: /UserProfiles/Create
        [CustomAuthorizeWrite(CallingController = "User Profiles")]
        public ActionResult Create()
        {
            if (!Roles.IsUserInRole("Administrator")) return RedirectToAction("Forbidden", "Error");
            return View();
        }

        // POST: /UserProfiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeWrite(CallingController = "User Profiles")]
        public ActionResult Create(UserProfile userprofile)
        {
            if (!Roles.IsUserInRole("Administrator")) return RedirectToAction("Forbidden", "Error");
            if (ModelState.IsValid)
            {
                _userProfilesRepo.Save(userprofile);
                return RedirectToAction("Index");
            }

            return View(userprofile);
        }

        // GET: /UserProfiles/Edit/5
        [CustomAuthorizeWrite(CallingController = "User Profiles")]
        public ActionResult Edit(int id = 0)
        {
            if (!Roles.IsUserInRole("Administrator")) return RedirectToAction("Forbidden", "Error");
            UserProfile userprofile = _userProfilesRepo.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        // POST: /UserProfiles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeWrite(CallingController = "User Profiles")]
        public ActionResult Edit(UserProfile userprofile)
        {
            if (!Roles.IsUserInRole("Administrator")) return RedirectToAction("Forbidden", "Error");
            if (ModelState.IsValid)
            {
                _userProfilesRepo.Update(userprofile);
                return RedirectToAction("Index");
            }
            return View(userprofile);
        }

        // GET: /UserProfiles/Delete/5
        [CustomAuthorizeWrite(CallingController = "User Profiles")]
        public ActionResult Delete(int id = 0)
        {
            if (!Roles.IsUserInRole("Administrator")) return RedirectToAction("Forbidden", "Error");
            UserProfile userprofile = _userProfilesRepo.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        // POST: /UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeWrite(CallingController = "User Profiles")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!Roles.IsUserInRole("Administrator")) return RedirectToAction("Forbidden", "Error");
            UserProfile userprofile = _userProfilesRepo.Find(id);
            _userProfilesRepo.Remove(userprofile);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _userProfilesRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}