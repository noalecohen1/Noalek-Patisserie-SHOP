using Patisserie.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Patisserie.Controllers
{
    public class HomeController : Controller
    {
        private readonly PatisserieContext db = new PatisserieContext();

        public ActionResult Index()
        {
            List<int> topProductsIds = Utl.Preferences((int?)Session["accountID"], -1);

            ViewBag.TopProducts = db.Products.Where(p => topProductsIds.Any(t => t == p.ProductID));// db.Products.Where(p => topProductsIds.Contains(p.ProductID)).ToList();
            return View(db.Categories.ToList());
        }

        public ActionResult Admin()
        {
            if (!Utl.IsAdmin(Session)) return RedirectToAction("Index", "Home");

            List<object> productsRates = new List<object>();
            var products = db.Products.ToList();

            double i = 0;
            double colorIncrement = 1.0 / products.Count;

            foreach (var product in products)
            {
                productsRates.Add(new
                {
                    label = product.Name,
                    value = product.PopularityRate,
                    color = "#" + Utl.HSL2RGB(i, 0.5, 0.5).ToString()
                });

                i += colorIncrement;
            }

            ViewBag.ProductsRate = productsRates;
            return View();
        }

        public JsonResult GetOrderHistoryByDate()
        {
            if (!Utl.IsAdmin(Session))
                return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

            List<object> orderHistory = new List<object>();
            var ohDates = db.OrdersHistories.GroupBy(o => o.OrderDate.Month).ToList();


            foreach (var item in ohDates)
            {
                orderHistory.Add(new
                {
                    month = item.Key,
                    ordersNum = item.Count()
                });
            }

            return Json(orderHistory, JsonRequestBehavior.AllowGet);
        }
    }
}