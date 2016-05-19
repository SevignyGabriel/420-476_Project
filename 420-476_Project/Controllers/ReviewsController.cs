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
    public class ReviewsController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.Users);
            return View(reviews.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            ViewBag.UserLogin = new SelectList(db.Users, "Login", "Login");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReviewID,Score,Title,Text,UserLogin")] Reviews review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserLogin = new SelectList(db.Users, "Login", "Login", review.UserLogin);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserLogin = new SelectList(db.Users, "Login", "Login", review.UserLogin);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewID,Score,Title,Text,UserLogin")] Reviews review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserLogin = new SelectList(db.Users, "Login", "Login", review.UserLogin);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reviews review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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

        public ActionResult ReportedList()
        {
            var reviews = db.Reviews.Where(r => r.Reports != null && r.Reports > 0).OrderByDescending(r => r.Reports);
            return View(reviews.ToList());
        }

        public ActionResult Add(int productId)
        {
            if (AccountsController.isLoggedIn())
            {
                return View();
            }
            return RedirectToAction("Login", "Accounts");
        }

        [HttpPost]
        public ActionResult Add([Bind(Include = "Score,Title,Text")] Reviews review, int productId)
        {
            if (ModelState.IsValid && review.Score <= 10 && review.Score >= 0)
            {
                var userLoggedIn = Session["UserLoggedIn"] as Users;
                review.UserLogin = userLoggedIn.Login;
                var id = db.Reviews.Count() + 1;
                review.ProductID = productId;
                review.ReviewID = id;
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Details", "Products", new { id = productId });
            }
            else
                TempData["Error"] = "Your review is invalid";

            return View();
        }

        public ActionResult Report(int reviewId, int productId)
        {
            var review = db.Reviews.Find(reviewId);
            if (review.Reports == null)
            {
                review.Reports = 1;
            }
            else
                review.Reports += 1;
            db.Entry(review).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", "Products", new { id = productId });
        }

        public ActionResult ResetReports(int reviewId)
        {
            var review = db.Reviews.Find(reviewId);
            review.Reports = 0;
            db.Entry(review).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ReportedList");
        }
    }
}
