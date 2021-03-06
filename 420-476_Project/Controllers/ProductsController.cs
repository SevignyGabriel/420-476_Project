﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _420_476_Project.Models;
using _420_476_Project.ViewModels;
using System.Web.Services;

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
            writeCookie(id);
            return View(product);
        }

        public void writeCookie(int? id)
        {
            bool exist = false;
            if (Request.Cookies["RecentlyViewed"]["Item1"] != "")
            {
                if (Request.Cookies["RecentlyViewed"]["Item1"] == Convert.ToString(id))
                {
                    exist = true;
                }
            }
            if (Request.Cookies["RecentlyViewed"]["Item2"] != "")
            {
                if (Request.Cookies["RecentlyViewed"]["Item2"] == Convert.ToString(id))
                {
                    exist = true;
                }
            }
            if (Request.Cookies["RecentlyViewed"]["Item3"] != "")
            {
                if (Request.Cookies["RecentlyViewed"]["Item3"] == Convert.ToString(id))
                {
                    exist = true;
                }
            }
            if (Request.Cookies["RecentlyViewed"]["Item4"] != "")
            {
                if (Request.Cookies["RecentlyViewed"]["Item4"] == Convert.ToString(id))
                {
                    exist = true;
                }
            }
            if (Request.Cookies["RecentlyViewed"]["Item5"] != "")
            {
                if (Request.Cookies["RecentlyViewed"]["Item5"] == Convert.ToString(id))
                {
                    exist = true;
                }
            }
            if (!exist)
            {
                switch (Request.Cookies["RecentlyViewed"]["AssignTo"])
                {
                    case "1":
                        Response.Cookies["RecentlyViewed"]["Item1"] = Convert.ToString(id);
                        Response.Cookies["RecentlyViewed"]["Item2"] = Request.Cookies["RecentlyViewed"]["Item2"];
                        Response.Cookies["RecentlyViewed"]["Item3"] = Request.Cookies["RecentlyViewed"]["Item3"];
                        Response.Cookies["RecentlyViewed"]["Item4"] = Request.Cookies["RecentlyViewed"]["Item4"];
                        Response.Cookies["RecentlyViewed"]["Item5"] = Request.Cookies["RecentlyViewed"]["Item5"];
                        break;
                    case "2":
                        Response.Cookies["RecentlyViewed"]["Item2"] = Convert.ToString(id);
                        Response.Cookies["RecentlyViewed"]["Item1"] = Request.Cookies["RecentlyViewed"]["Item1"];
                        Response.Cookies["RecentlyViewed"]["Item3"] = Request.Cookies["RecentlyViewed"]["Item3"];
                        Response.Cookies["RecentlyViewed"]["Item4"] = Request.Cookies["RecentlyViewed"]["Item4"];
                        Response.Cookies["RecentlyViewed"]["Item5"] = Request.Cookies["RecentlyViewed"]["Item5"];
                        break;
                    case "3":
                        Response.Cookies["RecentlyViewed"]["Item3"] = Convert.ToString(id);
                        Response.Cookies["RecentlyViewed"]["Item1"] = Request.Cookies["RecentlyViewed"]["Item1"];
                        Response.Cookies["RecentlyViewed"]["Item2"] = Request.Cookies["RecentlyViewed"]["Item2"];
                        Response.Cookies["RecentlyViewed"]["Item4"] = Request.Cookies["RecentlyViewed"]["Item4"];
                        Response.Cookies["RecentlyViewed"]["Item5"] = Request.Cookies["RecentlyViewed"]["Item5"];
                        break;
                    case "4":
                        Response.Cookies["RecentlyViewed"]["Item4"] = Convert.ToString(id);
                        Response.Cookies["RecentlyViewed"]["Item1"] = Request.Cookies["RecentlyViewed"]["Item1"];
                        Response.Cookies["RecentlyViewed"]["Item2"] = Request.Cookies["RecentlyViewed"]["Item2"];
                        Response.Cookies["RecentlyViewed"]["Item3"] = Request.Cookies["RecentlyViewed"]["Item3"];
                        Response.Cookies["RecentlyViewed"]["Item5"] = Request.Cookies["RecentlyViewed"]["Item5"];
                        break;
                    case "5":
                        Response.Cookies["RecentlyViewed"]["Item5"] = Convert.ToString(id);
                        Response.Cookies["RecentlyViewed"]["Item1"] = Request.Cookies["RecentlyViewed"]["Item1"];
                        Response.Cookies["RecentlyViewed"]["Item2"] = Request.Cookies["RecentlyViewed"]["Item2"];
                        Response.Cookies["RecentlyViewed"]["Item3"] = Request.Cookies["RecentlyViewed"]["Item3"];
                        Response.Cookies["RecentlyViewed"]["Item4"] = Request.Cookies["RecentlyViewed"]["Item4"];
                        break;
                    default:
                        break;
                }
                if (Convert.ToInt32(Request.Cookies["RecentlyViewed"]["AssignTo"]) >= 5)
                {
                    Response.Cookies["RecentlyViewed"]["AssignTo"] = "1";
                }
                else
                {
                    Response.Cookies["RecentlyViewed"]["AssignTo"] = Convert.ToString(Convert.ToInt32(Request.Cookies["RecentlyViewed"]["AssignTo"]) + 1);
                }
            }
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

        public ActionResult GetProductsNames()
        {
            string term = Request.QueryString["term"].ToLower();
            var result = from p in db.Products
                         where p.ProductName.ToLower().Contains(term)
                         select p.ProductName;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SimpleList()
        {
            var products = db.Products.Include(p => p.Categories);
            return View(products.ToList());
        }

        public ActionResult addToCart(int? id)
        {
            if (Session["cart"] == null || Session["cart"].ToString() == "")
            {
                Session["cart"] = "," + id + ",";
            }
            else
            {
                Session["cart"] = Session["cart"].ToString() + id + ",";
            }

            ViewBag.cart = Session["cart"];

            return RedirectToAction("ViewShoppingCart");
        }

        public ActionResult removeFromCart(int? id)
        {
            string rep = "," + id.ToString() + ",";

            Session["cart"] = Session["cart"].ToString().Replace(@"," + id.ToString() + ",", @",");

            if (Session["cart"].ToString() == ",")
            {
                Session["cart"] = "";
            }

            return RedirectToAction("ViewShoppingCart");
        }

        public ActionResult confirmCart(List<int> p, List<int> q)
        {
            Orders order = new Orders();
            int MAXorderID = db.Orders.Max(o => o.OrderID);
            order.OrderID = MAXorderID + 1;
            order.UserLogin = Session["UserLogin"].ToString();
            order.OrderDate = DateTime.Now;
            db.Orders.Add(order);


            OrderDetails orderDetail = new OrderDetails();
            int MAXorderDeatailID = db.OrderDetails.Max(o => o.OrderDetailsID);
            List<int> products = (List<int>)Session["products"];
            List<int> quantities = (List<int>)Session["quantities"];
            for (int i = 0; i < products.Count; i++)
            {
                orderDetail.OrderDetailsID = MAXorderDeatailID + (i + 1);
                orderDetail.OrderID = order.OrderID;
                orderDetail.ProductID = products[i];
                orderDetail.Quantity = short.Parse(quantities[i].ToString());
                db.OrderDetails.Add(orderDetail);
                
            }

            db.SaveChanges();

            Session["cart"] = null;

            return RedirectToAction("commandConfirmation");
        }

        public ActionResult commandConfirmation()
        {

            return View();
        }

        public ActionResult ViewShoppingCart()
        {
            String[] productsID = null;
            List<Products> products = new List<Products>();
            CartViewModel cartModel = new CartViewModel();

            if (Session["cart"] != null)
            {
                if (Session["cart"].ToString() != "")
                {
                    // Split string on ','. This will separate all the numbers in a string
                    productsID = Session["cart"].ToString().Split(',');
                    for (int i = 1; i <= (productsID.Count() - 2); i++)
                    {
                        products.Add(db.Products.Find(Int32.Parse(productsID[i].ToString())));
                    }

                    //foreach(var p in productsID) {
                    //     products.Add(db.Products.Find(Int32.Parse(p)));
                    //}
                }
            }
            cartModel.products = products;

            return View(cartModel);
        }
    }
}
