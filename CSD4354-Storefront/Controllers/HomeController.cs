using CSD4354_Storefront.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSD4354_Storefront.Controllers
{
    public class HomeController : Controller
    {
        private StoreDbContext db = new StoreDbContext();

        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}