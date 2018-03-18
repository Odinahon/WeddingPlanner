using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private WedPlannerContext _context;
 
        public HomeController(WedPlannerContext context)
        {
            _context = context;
        }
        
        
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.errors = new List<string>();
            return View();
        }

        [HttpPost]
        [Route("adduser")]
        public IActionResult AddUserToDB(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                List<User> CheckEmail = _context.UserTable.Where(theuser => theuser.Email == model.Email).ToList();
                if (CheckEmail.Count > 0)
                    {
                        ViewBag.ErrorRegister = "Email already in use...";
                        return View("Index");
                    }
                User newuser = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };  
                _context.UserTable.Add(newuser);
                _context.SaveChanges();
                
                User JustCreated = _context.UserTable.SingleOrDefault(theUser => theUser.Email == model.Email);
                HttpContext.Session.SetInt32("UserId", (int)JustCreated.UserId);
                HttpContext.Session.SetString("UserName", (string)JustCreated.FirstName + " " + (string)JustCreated.LastName);
                return RedirectToAction("dashboard","Wedding");// eto kogda mi hotim prosto sdelat refresh
                // return View("success"); //eto kogda mi hotim postroit stranicu zanovo, uvidet formu, dlya registration in dlya login
            }
            else
            {
                //   ViewBag.errors=ModelState.Values;
                  return View("Index");
            }
        } 
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            ViewBag.NiceTry = TempData["NiceTry"];
            return View("login");
        }
        [HttpPost]
        [Route("loginuser")]
        public IActionResult Login(string useremail=null, string userpassword=null)
        {
            if(userpassword != null && useremail != null)
            {
                // Checking if a User this provided Email exists in DB
                List<User> CheckUser = _context.UserTable.Where(theuser => theuser.Email == useremail).ToList();
                if (CheckUser.Count > 0)
                {
                    // Checking if the password matches
                    // var Hasher  = new PasswordHasher<User>();
                    if( CheckUser[0].Password==userpassword)
                    {
                        // If the checks are validated than save his ID and Name in session and redirect to the Dashboard page
                        HttpContext.Session.SetInt32("UserId", (int)CheckUser[0].UserId);
                        HttpContext.Session.SetString("UserName", (string)CheckUser[0].FirstName + " " + (string)CheckUser[0].LastName);
                        return RedirectToAction("dashboard","Wedding");
                    }
                }
            }
            // If check are not validated display an error
            ViewBag.ErrorLogin = "Invalid Login Data...";
            return View("login");
        }
        // [HttpGet]
        // [Route("dashboard")]
        // public IActionResult dashboard()
        // {
        //     // int? id=HttpContext.Session.GetInt32("UserId");
        //     // Console.WriteLine("%%%%%%%%%%%%%%%%%"+id);
        //     // User found=_context.UserTable.SingleOrDefault(u=>u.UserId==);
        //     int? id = HttpContext.Session.GetInt32("UserId");
        //     // User userId = _context.UserTable.Where(u =>u.UserId==id).ToList();
        //     User useruser= _context.UserTable.SingleOrDefault( i => i.UserId==id);
        //     ViewBag.UserName = (string)HttpContext.Session.GetString("UserName");
        //     return View("dashboard");
        //     // return View("dashboard",useruser);
        // } 
        // [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }
    }
}
