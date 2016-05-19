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
    public class HomeController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        public ActionResult Index()
        {
            var topProducts = db.Top_5_Most_Popular_Products();
            var products = db.Products.Where(p => p.ProductID.Equals(111111111)).Include(p => p.Categories);
            foreach (var product in topProducts.ToList())
            {
                products = products.Concat(db.Products.Where(p => p.ProductID.Equals(product.ProductID)).Include(p => p.Categories));
            }
            ViewBag.TopSellers = products.ToList();
            var recentlyViewed = db.Products.Where(p => p.ProductID.Equals(111111111)).Include(p => p.Categories);
            List<int> productsId = new List<int>();
            if (Request.Cookies["RecentlyViewed"] != null)
            {
                if (Request.Cookies["RecentlyViewed"]["Item1"] != "")
                {
                    productsId.Add(Convert.ToInt32(Request.Cookies["RecentlyViewed"]["Item1"]));
                }
                if (Request.Cookies["RecentlyViewed"]["Item2"] != "")
                {
                    productsId.Add(Convert.ToInt32(Request.Cookies["RecentlyViewed"]["Item2"]));
                }
                if (Request.Cookies["RecentlyViewed"]["Item3"] != "")
                {
                    productsId.Add(Convert.ToInt32(Request.Cookies["RecentlyViewed"]["Item3"]));
                }
                if (Request.Cookies["RecentlyViewed"]["Item4"] != "")
                {
                    productsId.Add(Convert.ToInt32(Request.Cookies["RecentlyViewed"]["Item4"]));
                }
                if (Request.Cookies["RecentlyViewed"]["Item5"] != "")
                {
                    productsId.Add(Convert.ToInt32(Request.Cookies["RecentlyViewed"]["Item5"]));
                }
            }
            else
            {
                Response.Cookies["RecentlyViewed"]["AssignTo"] = "1";
                Response.Cookies["RecentlyViewed"].Expires = DateTime.Now.AddDays(30d);    //  The cookie expires after 30 days
            }
            foreach (var productId in productsId)
            {
                recentlyViewed = recentlyViewed.Concat(db.Products.Where(p => p.ProductID.Equals(productId)).Include(p => p.Categories));
            }
            ViewBag.RecentlyViewed = recentlyViewed.ToList();
            return View();
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