using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Patisserie.Data;
using Patisserie.Models;

namespace Patisserie.Controllers
{
    public class AccountsController : Controller
    {
        private PatisserieContext db = new PatisserieContext();

        // GET: Accounts
        public ActionResult Index(string searchEmail, string searchPhone, string searchAddress, bool? searchIsModerator)
        {
            if (!Utl.IsAdmin(Session))
                return RedirectToAction("Index", "Home");

            List<Account> searchProducts = Search(searchEmail, searchPhone, searchAddress, searchIsModerator);

            return View(searchProducts);
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (!Utl.IsLoggedIn(Session) || 
               (!Utl.IsAdmin(Session)  && id != null && id != (int)Session["accountID"]))
                return RedirectToAction("Index", "Home");

            if (id == null) id = (int)Session["accountID"];

            Account account = db.Accounts.Find(id);
            if (account == null) return HttpNotFound();

            account.OrdersHistory = db.OrdersHistories.Where(o => o.AccountID == id).OrderBy(oh => oh.OrderNumber).ToList();
            account.OrdersHistory.ForEach(o => o.Product = db.Products.Find(o.ProductID));

            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            if (!Utl.IsAdmin(Session)) return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountID,IsModerator,Email,Password,Address,PhoneNumber")] Account account)
        {
            if (!Utl.IsAdmin(Session)) return RedirectToAction("Index", "Home");

            return CreateAccount(account);
        }

        private ActionResult CreateAccount(Account account)
        {
            bool isEmailInDB = db.Accounts.Any(a => a.Email == account.Email);

            if (!ModelState.IsValid || isEmailInDB)
            {
                if (isEmailInDB)
                    ViewBag.isEmailInDB = isEmailInDB;
                return View(account);
            }

            db.Accounts.Add(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!Utl.IsLoggedIn(Session) || ((!Utl.IsAdmin(Session) && id != null && id != (int)Session["accountID"])))
                return RedirectToAction("Index", "Home");

            if (id == null) id = (int)Session["accountID"];

            Account account = db.Accounts.Find(id);
            if (account == null) return HttpNotFound();
            
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountID,IsModerator,Email,Password,Address,PhoneNumber")] Account account)
        {
            if (!Utl.IsLoggedIn(Session) || (!Utl.IsAdmin(Session) && (int)Session["accountID"] != account.AccountID))
                return RedirectToAction("Index", "Home");

            bool isEmailInDB = db.Accounts.Any(a => a.Email == account.Email && a.AccountID != account.AccountID);

            if (!ModelState.IsValid || isEmailInDB)
            {
                if (isEmailInDB)
                    ViewBag.isEmailInDB = isEmailInDB;
                return View(account);
            }
            
            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", "Accounts", new { id = account.AccountID });
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!Utl.IsAdmin(Session)) return RedirectToAction("Index", "Home");

            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<Account> Search(string searchEmail, string searchPhone, string searchAddress, bool? searchIsModerator)
        {
            var accounts = db.Accounts.Select(a => a);

            if ((searchEmail != null) && (searchEmail != ""))
                accounts = accounts.Where(a => a.Email.ToLower().Contains(searchEmail.ToLower()));

            if ((searchPhone != null) && (searchPhone != ""))
                accounts = accounts.Where(a => a.PhoneNumber.Replace("-", "").Contains(searchPhone.Replace("-", "")));

            if ((searchAddress != null) && (searchAddress != ""))
                accounts = accounts.Where(a => a.Address.ToLower().Contains(searchAddress.ToLower()));

            if (searchIsModerator != null)
                accounts = accounts.Where(a => a.IsModerator == searchIsModerator);

            return accounts.ToList();
        }

        /********
         * 
         * Login
         * 
         * ********/

        // POST: Accounts/Login
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (Utl.IsLoggedIn(Session)) return RedirectToAction("Index", "Home");

            Account account = db.Accounts.FirstOrDefault(a => a.Email == email && a.Password == password);
            if (account == null) return View(new List<string> { "The email and password you entered are incorrect. Please try again." });

            var accountOrders = db.Orders.Include(o => o.Product).Include(o => o.Account)
                .Where(a => a.AccountID == account.AccountID).ToList();
            

            Session["accountID"] = account.AccountID;
            Session["cart"] = Utl.CreateCart(Session, accountOrders);
            Session.Timeout = 60;

            return RedirectToAction("Index", "Home");
        }

        /********
        * 
        * Register
        * 
        * ********/ 

        // POST: Accounts/Register
        [HttpPost]
        public ActionResult Register([Bind(Include = "AccountID,IsModerator,Email,Password,Address,PhoneNumber")] Account account)
        {
            if (Utl.IsLoggedIn(Session)) return RedirectToAction("Index", "Home");

            List<object> errorMessages = accountDetailsValidation(account.Email, account.Password, account.Address, account.PhoneNumber);
            if (errorMessages.Count>0) return Json(errorMessages);

            CreateAccount(account);
            return Login(account.Email, account.Password);
        }

        private List<object> accountDetailsValidation (string email, string password, string address, string phoneNumber)
        {
            List<object> errorMessages = new List<object>();

            if (email == null || email.Trim() == "" || email.Trim().Length == 0)
                errorMessages.Add(new
                {
                    email = "Email is missing."
                });
            else if (!IsValidEmail(email))
                errorMessages.Add(new
                {
                    email = "Email is not valid."
                });
            else
            {
                Account account = db.Accounts.FirstOrDefault(a => a.Email == email);
                if (account != null) errorMessages.Add(new
                {
                    email = "Email already exists."
                });
            }
            if (password == null || password.Trim() == "" || password.Trim().Length == 0)
                errorMessages.Add(new
                {
                    password = "Password is missing."
                });
            if (address == null || address.Trim() == "" || address.Trim().Length == 0)
                errorMessages.Add(new
                {
                    address = "Address is missing."
                });
            if (phoneNumber == null || phoneNumber.Trim() == "" || phoneNumber.Trim().Length == 0)
                errorMessages.Add(new
                {
                    phoneNumber = "Phone number is missing."
                });

            return errorMessages;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /********
        * 
        * Logout
        * 
        * ********/
        // GET: Accounts/Logout
        public ActionResult LogOut()
        {
            if (Utl.IsLoggedIn(Session))
            {
                Session["accountID"] = null;
                Session["cart"] = null;
                Session.Clear();
                Session.Abandon();
            }

            return RedirectToAction("Index", "Home");
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
