using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _420_476_Project.Models;

namespace _420_476_Project.Controllers
{
    public class ProductsController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        [NonAction]
        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Categories);
            return View(products.ToList());
        }


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Index(String request, String category, int pageNumber)
        {
            var products = db.Products.Where(p => p.CategoryID == 1);

            if (category != "All")
                products = db.Products.Where(p => p.ProductName.Contains(request) && p.Categories.CategoryName == category).Include(p => p.Categories);
            else
                products = db.Products.Where(p => p.ProductName.Contains(request)).Include(p => p.Categories);

            ViewBag.category = category;
            ViewBag.request = request;
            return View(createProductsList(pageNumber, products.ToList()));
        }

        public List<Products> createProductsList(int pageNumber, List<Products> products)
        {
            List<Products> limited = new List<Products>();
            int productNumber = 0;
            foreach (var p in products)
            {
                productNumber++;
                if (productNumber <= (pageNumber * 2) && productNumber > ((pageNumber - 1) * 2))
                {
                    limited.Add(p);
                }
            }
            ViewBag.productNumber = (int)(productNumber / 2) + 1;
            return limited;
        }

        [ChildActionOnly]
        public ActionResult RenderSearch()
        {
            Create();

            return PartialView("_SearchPartial");
        }

        [ChildActionOnly]
        public ActionResult RenderMenu()
        {
            Create();

            return PartialView("_MenuPartial");
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,UnitPrice,UnitsInStock,CategoryID")] Products product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,UnitPrice,UnitsInStock,CategoryID")] Products product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = db.Products.Find(id);
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
            Products product = db.Products.Find(id);
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

        public ActionResult ViewShoppingCart()
        {
            var orderDetails = db.OrderDetails.Include(p => p.Products);
            return View(orderDetails.ToList());
        }
    }
}
