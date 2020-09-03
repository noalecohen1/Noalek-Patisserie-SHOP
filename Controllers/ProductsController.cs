using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Patisserie.Data;
using Patisserie.Models;

namespace Patisserie.Controllers
{
    public class ProductsController : Controller
    {
        private PatisserieContext db = new PatisserieContext();

        // GET: Products
        public ActionResult Index(string searchName, int? searchCategoryID,
            bool? searchIsDairy, bool? searchIsGlutenFree, bool? searchIsVegan, double searchMaxPrice = double.MaxValue)
        {
            ViewData["Categories"] = new SelectList(db.Categories, "CategoryID", "Name", db.Categories.ToList());

            List<Product> searchProducts = Search(searchName, searchCategoryID, searchIsDairy, searchIsGlutenFree, searchIsVegan, searchMaxPrice);

            return View(searchProducts);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Product product = db.Products.Find(id);
            if (product == null)  return HttpNotFound();
            
            int? accountID = Utl.IsLoggedIn(Session) ? (int)Session["accountID"] : -1;
            var prefs = Utl.Preferences(accountID, (int)id);

            ViewBag.Preferences = Utl.PopulateProducts(prefs);
            ViewBag.CategoryName = db.Categories.Where(c => c.CategoryID == product.CategoryID).ToList()[0].Name;
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            if (!Utl.IsAdmin(Session)) return RedirectToAction("Index", "Home");

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Name,ImagePath,Description,Price,IsDairy,IsGlutenFree,IsVegan,PopularityRate,CategoryID")] Product product)
        {
            if (!Utl.IsAdmin(Session))
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
                return View(product);
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!Utl.IsAdmin(Session)) return RedirectToAction("Index", "Home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Product product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,ImagePath,Description,Price,IsDairy,IsGlutenFree,IsVegan,PopularityRate,CategoryID")] Product product)
        {
            if (!Utl.IsAdmin(Session)) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
                return View(product);
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!Utl.IsAdmin(Session)) return RedirectToAction("Index", "Home");

            Product product = db.Products.Find(id);

            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<Product> Search(string searchName, int? searchCategoryID,
            bool? searchIsDairy, bool? searchIsGlutenFree, bool? searchIsVegan, double searchMaxPrice = double.MaxValue)
        {
            var products = db.Products.Select(p => p);

            if ((searchName != null) && (searchName != ""))
                products = products.Where(p => p.Name.ToLower().Contains(searchName.ToLower()));

            if (searchCategoryID != null)
                products = products.Where(p => p.CategoryID == searchCategoryID);

            if (searchIsDairy != null)
                products = products.Where(p => p.IsDairy == searchIsDairy);

            if (searchIsGlutenFree != null)
                products = products.Where(p => p.IsGlutenFree == searchIsGlutenFree);

            if (searchIsVegan != null)
                products = products.Where(p => p.IsVegan == searchIsVegan);

            products = products.Where(p => p.Price <= searchMaxPrice);

            return products.ToList();
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
