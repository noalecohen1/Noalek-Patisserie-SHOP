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
    public class CategoriesController : Controller
    {
        private PatisserieContext db = new PatisserieContext();

        enum SortBy { popularity=0, priceLTH=1, priceHTL=2 }

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            if (!Utl.IsAdmin(Session)) return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,Name,ImagePath,Description")] Category category)
        {
            if (!Utl.IsAdmin(Session))
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!Utl.IsAdmin(Session))
                return RedirectToAction("Index", "Home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Category category = db.Categories.Find(id);
            if (category == null)  return HttpNotFound();

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,Name,ImagePath,Description")] Category category)
        {
            if (!Utl.IsAdmin(Session)) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!Utl.IsAdmin(Session)) return RedirectToAction("Index", "Home");

            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id, int sortBy=(int)SortBy.popularity)
        {
            if (id == null) return RedirectToAction("Index", "Products");

            var productsList = db.Products.Include(p => p.Category)
                .Where(p => p.CategoryID == id);

            switch (sortBy)
            {
                case (int)SortBy.priceHTL:
                    productsList = productsList.OrderByDescending(p => p.Price);
                    break;

                case (int)SortBy.priceLTH:
                    productsList = productsList.OrderBy(p => p.Price);
                    break;

                default:
                    productsList = productsList.OrderByDescending(p => p.PopularityRate);
                    break;
            }

            ViewBag.Category = db.Categories.Where(c => c.CategoryID == id).ToList()[0];
            return View(productsList.ToList());
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
