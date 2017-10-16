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
    public class WeddingController : Controller
    {
        private WeddingContext _context;

        public WeddingController(WeddingContext context)
        {
            _context = context;
        }

        // Loads the dashboard.
        // Redirects if the user is not logged in.
        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            int? Id = HttpContext.Session.GetInt32("Id");
            if(Id == null)
            {
                TempData["LoginError"] = "Please login to view that page";
                return RedirectToAction("Index", "User");
            }
            // Gets a list of all of the weddings
            List<Wedding> weddings = _context.Weddings.Include(w => w.WedderOne)
                                                        .Include(w => w.WedderTwo)
                                                        .Include(w => w.Guests)
                                                        .ToList();
            ViewBag.Weddings = weddings;
            ViewBag.UserId = Id;

            // Creates a list of all of the wedding ids the user is attending.
            List<Guest> Guests = _context.Guests.Where(g => g.UserId == Id).ToList();
            List<int> Attending = new List<int>();
            foreach (Guest g in Guests) {
                Attending.Add(g.WeddingId);
            }
            ViewBag.Attending = Attending;
            return View();
        }

        // Displays the form for a new wedding.
        // Displays an error and redirects to the hompage if they user is not logged in.
        [HttpGet]
        [Route("NewWedding")]
        public IActionResult NewWedding()
        {
            if(HttpContext.Session.GetInt32("Id") == null)
            {
                TempData["LoginError"] = "Please login to view that page";
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        // Displays the page for a single weddings.
        // Displays an error and redirects to the hompage if they user is not logged in.
        [HttpGet]
        [Route("Wedding/{id}")]
        public IActionResult SingleWedding(int id)
        {
            if(HttpContext.Session.GetInt32("Id") == null)
            {
                TempData["LoginError"] = "Please login to view that page";
                return RedirectToAction("Index", "User");
            }
            Wedding Wedding = _context.Weddings.Include(w => w.WedderOne)
                                                        .Include(w => w.WedderTwo)
                                                        .Include(w => w.Guests)
                                                            .ThenInclude(u => u.User)
                                                        .FirstOrDefault();
            ViewBag.Wedding = Wedding;
            return View();
        }

        // Creates a new wedding if the given information is valid and redirects to that weddings page.
        // Shows the new wedding form if the information was incorrect. 
        [HttpPost]
        [Route("CreateWedding")]
        public IActionResult CreateWedding(WeddingViewModel model)
        {
            // Check to see if the users exist
            User WedderOne =_context.Users.Where(u => u.FirstName == model.WedderOneName).SingleOrDefault();
            User WedderTwo =_context.Users.Where(u => u.FirstName == model.WedderTwoName).SingleOrDefault();
            if(ModelState.IsValid && WedderOne != null && WedderTwo != null)
            {
                Wedding NewWedding = new Wedding
                {
                    Date = model.Date,
                    Address = model.Address,
                    WedderOneId = WedderOne.Id,
                    WedderTwoId = WedderTwo.Id
                };
                _context.Weddings.Add(NewWedding);
                _context.SaveChanges();
                int WeddingId = _context.Weddings.Last().Id;
                return Redirect($"Wedding/{WeddingId}");
            }
            TempData["Error"] = (WedderOne == null || WedderTwo == null) ? "One or both of the wedders do not exist" : null;
            return View("NewWedding");
        }

        // Deletes the specified wedding from the database :(.
        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int Id)
        {
            Wedding Wedding = _context.Weddings.Where(w => w.Id == Id).SingleOrDefault();
            _context.Weddings.Remove(Wedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        // Allows the user to RSVP to a certain wedding.
        [HttpGet]
        [Route("Rsvp/{id}")]
        public IActionResult Rsvp(int Id)
        {
            Guest Guest = new Guest {
                UserId = (int)HttpContext.Session.GetInt32("Id"),
                WeddingId = Id
            };
            _context.Add(Guest);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");            
        }

        // Allows the user to un-RSVP from a certain wedding.
        [HttpGet]
        [Route("UnRsvp/{id}")]
        public IActionResult UnRsvp(int Id)
        {
            int UserId = (int)HttpContext.Session.GetInt32("Id");
            Guest Guest = _context.Guests.Where(g => g.UserId == UserId && g.WeddingId == Id).SingleOrDefault();
            _context.Guests.Remove(Guest);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");            
        }
    }
}
