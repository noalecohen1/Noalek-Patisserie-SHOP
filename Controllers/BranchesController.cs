using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Patisserie.Data;
using Patisserie.Models;

namespace Patisserie.Controllers
{
    public class BranchesController : Controller
    {
        private PatisserieContext db = new PatisserieContext();

        // GET: Branches
        public ActionResult Index()
        {
            return View(db.Branches.ToList());
        }

        // GET: Branches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // GET: Branches/Create
        public ActionResult Create()
        {
            if (!Utl.IsAdmin(Session))
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BranchID,Name,Address,PhoneNumber")] Branch branch)
        {
            if (!Utl.IsAdmin(Session))
                return RedirectToAction("Index", "Home");

            bool isNameInDB = db.Branches.Any(a => a.Name == branch.Name && a.BranchID != branch.BranchID);
            bool isAddressInDB = db.Branches.Any(a => a.Address == branch.Address && a.BranchID != branch.BranchID);

            if (ModelState.IsValid && !isAddressInDB && !isNameInDB)
            {
                db.Branches.Add(branch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (isNameInDB)
                ViewBag.isNameInDB = isNameInDB;
            if (isAddressInDB)
                ViewBag.isAddressInDB = isAddressInDB;

            return View(branch);
        }

        // GET: Branches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!Utl.IsAdmin(Session))
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BranchID,Name,Address,PhoneNumber")] Branch branch)
        {
            if (!Utl.IsAdmin(Session))
                return RedirectToAction("Index", "Home");

            bool isNameInDB = db.Branches.Any(a => a.Name == branch.Name && a.BranchID != branch.BranchID);
            bool isAddressInDB = db.Branches.Any(a => a.Address == branch.Address && a.BranchID != branch.BranchID);

            if (ModelState.IsValid && !isAddressInDB && !isNameInDB)
            {
                db.Entry(branch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (isNameInDB)
                ViewBag.isNameInDB = isNameInDB;
            if (isAddressInDB)
                ViewBag.isAddressInDB = isAddressInDB;

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!Utl.IsAdmin(Session))
                return RedirectToAction("Index", "Home");

            Branch branch = db.Branches.Find(id);

            db.Branches.Remove(branch);
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
