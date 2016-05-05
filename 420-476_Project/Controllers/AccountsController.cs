﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _420_476_Project.Models;
using System.Net;
using System.Data.Entity;

namespace _420_476_Project.Controllers
{
    public class AccountsController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Login
        public ActionResult Details()
        {
            var userLoggedIn = Session["UserLoggedIn"] as User;
            return View(userLoggedIn);
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // Verify the login and password entered by the user
        [HttpPost]
        public ActionResult Login(User user, string remember)
        {
            if (user.Login != null && user.Password != null)
            {
                //  Verify if the user checked the "Remember me" checkbox
                if (remember != null)
                {
                    //  Write a cookie to store the user Login.
                    Response.Cookies["RememberMe"]["Login"] = user.Login;
                    Response.Cookies["RememberMe"].Expires = DateTime.Now.AddDays(7d);    //  The cookie expires after 7 days
                }
                //  Verify if the Login and password are correct
                    //  Verify if the Login exists
                    var q = from c in db.Users
                            where c.Login.Equals(user.Login)
                            select c;

                    //  Verify if the Password is correct
                    foreach (var customer in q.ToList())
                    {
                        byte[] data = System.Text.Encoding.ASCII.GetBytes(user.Password);
                        data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                        String hash = System.Text.Encoding.ASCII.GetString(data);
                        if (hash.Equals(customer.Password))
                        {
                            Session["loggedIn"] = true;
                            Session["UserLoggedIn"] = customer;
                        }
                        else
                            Session["loggedIn"] = false;
                    }
            }
            TempData["Message"] = "Your login or password are incorrect!";
            return RedirectToAction("Login");
        }

        //  Logout the current user
        public ActionResult Logout()
        {
            Session["loggedIn"] = false;
            Session["Name"] = null;
            return RedirectToAction("Login");
        }

        //  Verify if the user is logged in and return the result in a bool
        public static bool isLoggedIn()
        {
            if (System.Web.HttpContext.Current.Session["loggedIn"] == null)
                System.Web.HttpContext.Current.Session["loggedIn"] = false;
            return (bool)System.Web.HttpContext.Current.Session["loggedIn"];
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Login,Password,Email,Name,PhoneNumber,ShipAddress")] User user)
        {
            if (ModelState.IsValid)
            {
                user.RoleID = 1;
                byte[] data = System.Text.Encoding.ASCII.GetBytes(user.Password);
                data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                String hash = System.Text.Encoding.ASCII.GetString(data);
                user.Password = hash;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Login,Password,Email,Name,PhoneNumber,ShipAddress")] User user)
        {
            if (ModelState.IsValid)
            {
                var userLoggedIn = Session["UserLoggedIn"] as User;
                if (!user.Password.Equals(userLoggedIn.Password))
                {
                    byte[] data = System.Text.Encoding.ASCII.GetBytes(user.Password);
                    data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                    String hash = System.Text.Encoding.ASCII.GetString(data);
                    user.Password = hash;
                }
                user.RoleID = userLoggedIn.RoleID;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }


    }
}