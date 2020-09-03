using Patisserie.Data;
using Patisserie.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Patisserie.Controllers
{
    public class FooterController : Controller
    {
        private readonly PatisserieContext db = new PatisserieContext();

        // GET: Footer
        public ActionResult Index()
        {
            List<Branch> branches = db.Branches.ToList();
            return PartialView("_Footer", branches);
        }
    }
}