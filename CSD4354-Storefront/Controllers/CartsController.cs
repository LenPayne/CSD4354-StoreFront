using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using CSD4354_Storefront.DAL;
using CSD4354_Storefront.Models;
using Microsoft.AspNet.Identity.Owin;

namespace CSD4354_Storefront.Controllers
{
    public class CartsController : Controller
    {
        private StoreDbContext db = new StoreDbContext();

        // GET: Carts
        public ActionResult Index()
        {
            if (this.User != null)
            {
                var appUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
                if (appUser != null) { 
                    var userId = appUser.UserId;
                    return View(db.Carts.Where(c => c.Purchaser.Id == userId).ToList());
                }
            }
            return View(db.Carts.ToList());
        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                var cartId = HttpContext.Session["cartId"];
                Cart c = db.Carts.Find(cartId);
                if (c == null)
                {
                    var userId = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId()).UserId;
                    var user = db.Users.Find(userId);
                    c = new Cart { Date = new DateTime(), Items = new List<ProductQty>(), Purchaser = user, Status = CartStatus.OPEN};
                    db.Carts.Add(c);
                    db.SaveChanges();
                }
                HttpContext.Session["cartId"] = c.Id;
                return View(c);
            }
            Cart cart = db.Carts.Find(id);
            
            if (cart == null)
            {
                return HttpNotFound();
            }
            foreach (ProductQty item in cart.Items)
                db.Entry(item).Reference(i => i.Item).Load();

            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Discount,Tracking,TaxRate,PaymentId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Discount,Tracking,TaxRate,PaymentId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
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

        public ActionResult AddToCart(int id, int quantity)
        {
            Product product = db.Products.Find(id);
            ProductQty item = new ProductQty();
            item.Item = product;
            item.Quantity = quantity;
            var cartId = HttpContext.Session["cartId"];
            Cart cart = db.Carts.Find(cartId);
            if (cart != null && (cart.Status == CartStatus.SHIPPED || cart.Status == CartStatus.PAID)) cart = null;
            if (cart == null)
            {
                var userId = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId()).UserId;
                var user = db.Users.Find(userId);
                cart = new Cart {
                    Date = new DateTime(),
                    Items = new List<ProductQty>(),
                    Purchaser = user,
                    Status = CartStatus.OPEN
                };
                db.Carts.Add(cart);                
            }
            cart.Items.Add(item);
            db.SaveChanges();
            HttpContext.Session["cartId"] = cart.Id;
            return RedirectToAction("Details", new { id = cart.Id });
        }

        public ActionResult Checkout()
        {
            var cartId = HttpContext.Session["cartId"];
            Cart cart = db.Carts.Find(cartId);
            if (cart == null)
            {
                return RedirectToAction("Index");
            }
            foreach (ProductQty item in cart.Items)
                db.Entry(item).Reference(i => i.Item).Load();
            cart.Status = CartStatus.CHECKING_OUT;
            db.SaveChanges();           
            return View(cart);
        }

        public ActionResult Confirm()
        {
            var cartId = HttpContext.Session["cartId"];
            Cart cart = db.Carts.Find(cartId);
            if (cart == null)
            {
                return RedirectToAction("Index");
            }
            cart.Status = CartStatus.PAID;
            HttpContext.Session["cartId"] = null;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
