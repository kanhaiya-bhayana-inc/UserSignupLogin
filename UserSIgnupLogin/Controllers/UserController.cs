 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserSIgnupLogin.Models;

namespace UserSIgnupLogin.Controllers
{
    public class UserController : Controller
    {
        DbUserSignUpLoginEntities db = new DbUserSignUpLoginEntities();
        // GET: User
        public ActionResult Index()
        {
            return View(db.TblUserInfoes.ToList());
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(TblUserInfo request)
        {
            if(db.TblUserInfoes.Any(x => x.UserName == request.UserName))
            {
                ViewBag.Notification = "This account has already existed";
                return View();
            }
            else
            {
                db.TblUserInfoes.Add(request); 
                db.SaveChanges();

                Session["UserId"] = request.UserID.ToString();
                Session["UserName"] = request.UserName.ToString();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TblUserInfo requrest)
        {
            var checkLogin = db.TblUserInfoes.Where(x => x.UserName.Equals(requrest.UserName) && x.UserPassword.Equals(requrest.UserPassword)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["UserId"] = requrest.UserID.ToString();
                Session["UserName"] = requrest.UserName.ToString();
                return RedirectToAction("Index", User);
            }
            else
            {
                ViewBag.Notification = "Invalid crecentials!";
            }
                return View();
        }

    }
}