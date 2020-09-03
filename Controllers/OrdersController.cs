using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Patisserie.Data;
using Patisserie.Models;

namespace Patisserie.Controllers
{
    public class OrdersController : Controller
    {
        private PatisserieContext db = new PatisserieContext();

        public ActionResult UpdateItem(int accountID, int productID, int amount)
        {
            if (!isAbleToChangeOrder(accountID)) return RedirectToAction("Index", "Home");

            var accountOrders = getAccountsOrders(accountID);

            if (accountOrders.Any(o => o.ProductID == productID))
                updateItemInDB(accountID, productID, amount);
            else
                addNewItemToDB(accountID, productID, amount);

            db.SaveChanges();
            Session["cart"] = Utl.CreateCart(Session, accountOrders.ToList());

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddItem(int accountID, int productID, int amount)
        {
            if (!isAbleToChangeOrder(accountID)) return RedirectToAction("Index", "Home");

            var accountOrders = getAccountsOrders(accountID);

            addItemToDB(accountID, productID, amount, accountOrders);

            db.SaveChanges();
            Session["cart"] = Utl.CreateCart(Session, accountOrders.ToList());

            return RedirectToAction("Index", "Home");
        }

        public ActionResult DeleteItem(int accountID, int productID, int amount)
        {
            return AddItem(accountID, productID, -amount);
        }

        private Order getOrderFromDB(int accountID, int productID)
        {
            return db.Orders.SingleOrDefault(order => order.AccountID == accountID && order.ProductID == productID);
        }


        private void addItemToDB(int accountID, int productID, int amount, IQueryable<Order> accountOrders)
        {
            if (accountOrders.Any(o => o.ProductID == productID))
                updateItemInDB(accountID, productID, getOrderFromDB(accountID, productID).Amount + amount);
            else
                addNewItemToDB(accountID, productID, amount);
        }

        private void updateItemInDB(int accountID, int productID, int amount)
        {
            Order item = getOrderFromDB(accountID, productID);

            item.Amount = amount;
            if (item.Amount <= 0)
                db.Orders.Remove(item);
        }

        private void addNewItemToDB(int accountID, int productID, int amount)
        {
            db.Orders.Add(new Order
            {
                Product = db.Products.Single(p => p.ProductID == productID),
                Account = db.Accounts.Single(a => a.AccountID == accountID),
                Amount = amount
            });
        }

        private IQueryable<Order> getAccountsOrders(int accountID)
        {
            return db.Orders.Include(o => o.Product).Include(o => o.Account)
                .Where(a => a.AccountID == accountID);
        }

        private bool isAbleToChangeOrder(int accountID)
        {
            return Utl.IsLoggedIn(Session) && (Utl.IsAdmin(Session) || accountID == (int)Session["accountID"]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitOrder()
        {
            if (!Utl.IsLoggedIn(Session)) return RedirectToAction("Index", "Home");

            int accountID = (int)Session["accountID"];

            Account account = db.Accounts.FirstOrDefault(a => a.AccountID == accountID);
            if (account == null) return RedirectToAction("Index", "Home");

            var order = db.Orders.Include(o => o.Product).Include(o => o.Account).Where(o => o.AccountID == account.AccountID).ToList();
            if (order == null || order.Count == 0) return RedirectToAction("Index", "Home");
  
            int orderNumber = 1;
            if(db.OrdersHistories.Count()>0)
                orderNumber += db.OrdersHistories.Max(o => o.OrderNumber);

            foreach (Order item in order)
            {
                db.Products.Find(item.ProductID).PopularityRate += item.Amount;
                archiveItemFromOrder(item, orderNumber);
            }

            db.SaveChanges();
            Session["cart"] = null;

            return RedirectToAction("Details", "Accounts", new { id = (int)Session["accountID"] });
        }

        private void archiveItemFromOrder(Order item, int orderNumber)
        {
            db.OrdersHistories.Add(new OrdersHistory
            {
                Product = item.Product,
                Account = item.Account,
                Amount = item.Amount,
                OrderDate = DateTime.Now,
                OrderNumber = orderNumber
            });

            db.Orders.Remove(item);
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
