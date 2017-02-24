using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CSD4354_Storefront.DAL;
using CSD4354_Storefront.Models;
using System.IO;
using System.Drawing;

namespace CSD4354_Storefront.Controllers
{
    public class ProductsController : Controller
    {
        private StoreDbContext db = new StoreDbContext();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ProductDetailsViewModel pdvm = new ProductDetailsViewModel();
            pdvm.Item = product;
            pdvm.Quantity = 1;
            return View(pdvm);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StockQuantity,Location,Colour,Size")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(new ProductEditViewModel { Item = product });
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Item,File")] ProductEditViewModel pevm)
        {
            if (ModelState.IsValid)
            {
                if (pevm.File != null && pevm.File.ContentLength > 0)
                {
                    var imgFolder = "/Content/ProductImages/";
                    // Saves the existing image to the server
                    var filename = DateTime.Now.ToBinary().ToString("X") + Path.GetFileName(pevm.File.FileName);
                    var path = Path.Combine(Server.MapPath("~" + imgFolder), filename);
                    pevm.File.SaveAs(path);
                    pevm.Item.ImageFilename = imgFolder + filename;

                    // Resizes the existing image to 200px wide while retaining aspect ratio
                    var thumbname = "t_" + filename;
                    var thumbpath = Path.Combine(Server.MapPath("~" + imgFolder), thumbname);
                    using (var srcImage = Image.FromFile(path))
                    {
                        var newWidth = 200;
                        var newHeight = (int)(newWidth * srcImage.Height / srcImage.Width);
                        using (var newImage = new Bitmap(newWidth, newHeight))
                            using (var graphics = Graphics.FromImage(newImage))
                        {
                            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                            graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
                            newImage.Save(thumbpath);
                        }
                    }
                    pevm.Item.ThumbnailFilename = imgFolder + thumbname;
                }
                db.Entry(pevm.Item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pevm.Item);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
