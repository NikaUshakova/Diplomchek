using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BeautySaloon.Models;
using BeautySaloon.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private readonly ApplicationContext db;
        private User user;
        public MenuController(ApplicationContext context)
        {
            db = context;
        }

        [AllowAnonymous]
        public IActionResult Main()
        {            
            ViewBag.Services1 = db.Services.Where(s => s.Category.Equals("Парикмахерские услуги")).Select(n => n.Name);
            ViewBag.Services2 = db.Services.Where(s => s.Category.Equals("Прически")).Select(n => n.Name);
            ViewBag.Services3 = db.Services.Where(s => s.Category.Equals("Ногтевой сервис")).Select(n => n.Name);
            ViewBag.Masters = db.Masters.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.ID.ToString(),
                                      Text = a.Surname + " " + a.Name +" " + a.Patronymic
                                  }).ToList();
          
            ViewBag.Services = new SelectList(db.Services, "ID", "Name");
            return View();
        }
        //public IActionResult AdminIndex()
        //{
        //    return View();
        //}
       

        // GET: Clients/Create
        public IActionResult CreateOrder()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(Order modelorder)
        {
            user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    // добавляем заказ в бд
                    db.Orders.Add(new Order
                    {
                        Date = modelorder.Date,
                        MasterID = modelorder.MasterID,
                        ServiceID = modelorder.ServiceID,
                        UserID = user.ID
                    });
                    await db.SaveChangesAsync();


                    return RedirectToAction("Main", "Menu");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
                //return RedirectToAction("Home", "Home");
                return Ok(modelorder);
                //return Content(User.Identity.Name);
            }
            return Ok(modelorder);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


       
    }
}
   
