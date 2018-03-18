using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class WeddingController : Controller 
    {
        private WedPlannerContext _context;
 
        public WeddingController(WedPlannerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult dashboard()
        {
            // int? id=HttpContext.Session.GetInt32("UserId");
            // Console.WriteLine("%%%%%%%%%%%%%%%%%"+id);
            // User found=_context.UserTable.SingleOrDefault(u=>u.UserId==);
            int? id = HttpContext.Session.GetInt32("UserId");
            // User userId = _context.UserTable.Where(u =>u.UserId==id).ToList();
            User useruser= _context.UserTable.SingleOrDefault( i => i.UserId==id);
            ViewBag.UserName = (string)HttpContext.Session.GetString("UserName");
            // var x = _context.WeddingTable.ToList();
            // List <Wedding> guests = _context.WeddingTable.Include(u =>u.Guests).ToList();
            // List <User> weddingstoGo =_context.UserTable.Include(w =>w.WeddingsToGo).ToList();
            var y= _context.WeddingTable.Include(u => u.Guests).ThenInclude(w => w.User).ToList();
            ViewBag.session=HttpContext.Session.GetInt32("UserId");
            // ViewBag.weddings=Weddings;
            ViewBag.flag=false;
            return View("dashboard", y);
            // return View("dashboard",useruser);
        } 
        [HttpGet]
        [Route("weddingpage")]
        public IActionResult weddingpage()
        {
            ViewBag.errors = new List<string>();
            return View("NewWedding");
        }
        [HttpPost]
        [Route("addwedding")]
        public IActionResult AddWeddingToDB(Wedding model)
        {
            int? id = HttpContext.Session.GetInt32("UserId");
            User newuser = _context.UserTable.SingleOrDefault(i =>i.UserId==id);
            if(ModelState.IsValid)
            {
                int uId = Convert.ToInt32 (id);
                Wedding newwedding = new Wedding()
                {
                    WedderOne = model.WedderOne,
                    WedderTwo = model.WedderTwo,
                    WeddingDate=model.WeddingDate,
                    WeddingAddress=model.WeddingAddress,
                    UserId=newuser.UserId // UserId doesn't come from the html page, then we need to set our UserId to id from session
                };
                _context.WeddingTable.Add(newwedding);
                _context.SaveChanges();

                // Wedding last = _context.WeddingTable.ToList().Last();
                // int wId = last.WeddingId;
                // WeddingUser join = new WeddingUser()
                // {
                //     UserId = uId,
                //     WeddingId = wId
                // };
                // _context.WeddingGuest.Add (join);
                // _context.SaveChanges ();
                return RedirectToAction ("dashboard");
            } else {
                return View ("NewWedding");
            }
  
        }

        [HttpGet]
        [Route("thiswedding/{wedId}")]
        public IActionResult ThisWedding(int wedId){
            List<Wedding> Weddings = _context.WeddingTable
                .Include (p => p.Guests)
                .ThenInclude (s => s.User)
                .ToList ();
            Wedding wedding=Weddings.SingleOrDefault(w=>w.WeddingId==wedId);
            // Console.WriteLine("***********"+wedding.Users.Count());
            ViewBag.wedding=wedding;
            return View("ThisWedding");
        }


        [HttpGet]
        [Route("delete/{wId}")]
        public IActionResult DeleteWed(int wId)
        {
            Wedding wedding=_context.WeddingTable.SingleOrDefault(w=>w.WeddingId==wId);
            _context.WeddingTable.Remove(wedding);
            List<WeddingUser> weduser=_context.WeddingGuest.Where(w=>w.WeddingId==wId).ToList();
            foreach(var wd in weduser){
                _context.WeddingGuest.Remove(wd);
            }
            _context.SaveChanges();
            return RedirectToAction("dashboard");

        }
        [HttpGet]
        [Route("RSVP/{wId}")]
        public IActionResult JoinWedding(int wId){
            int? id=HttpContext.Session.GetInt32("UserId");
            int uid=Convert.ToInt32(id);
            WeddingUser join=new WeddingUser(){
                UserId=uid,
                WeddingId=wId
            };
            _context.WeddingGuest.Add(join);
            _context.SaveChanges();
            return RedirectToAction("dashboard");
        }
        [HttpGet]
        [Route("unRSVP/{wId}")]
        public IActionResult LeaveWedding(int wId){
            int? id=HttpContext.Session.GetInt32("UserId");
            int uid=Convert.ToInt32(id);
            WeddingUser mjoins=_context.WeddingGuest.SingleOrDefault(w=>w.WeddingId==wId && w.UserId==uid);
            _context.WeddingGuest.Remove(mjoins);
            _context.SaveChanges();
            return RedirectToAction("dashboard");
        }

    }

}