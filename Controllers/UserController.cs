using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using wedding.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace wedding.Controllers
{
    public class UserController : Controller
    {
        private WeddingContext _context;

        public UserController(WeddingContext context)
        {
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(UserViewmodel model)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User NewUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password
                };
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                _context.Users.Add(NewUser);
                _context.SaveChanges();
                int UserId = _context.Users.Last().Id;
                HttpContext.Session.SetInt32("Id", UserId);
                return RedirectToAction("Dashboard", "Wedding");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string Email, string Password)
        {
            User user = _context.Users.Where(u => u.Email == Email).SingleOrDefault();
            if(user != null && Password != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(Hasher.VerifyHashedPassword(user, user.Password, Password) != 0)
                {
                    HttpContext.Session.SetInt32("Id", user.Id);
                    return RedirectToAction("Dashboard", "Wedding");
                }
            }
            TempData["Error"] = "Email and Password did not match or the provided email is not registered";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
