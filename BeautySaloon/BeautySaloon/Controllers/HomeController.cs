using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BeautySaloon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautySaloon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext db;

        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Home()
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
            //var masters = db.Masters.Select(m => (
            //   m.Surname + " " +
            //   m.Name + " " +
            //   m.Patronymic
            //));            
           // ViewBag.Masters = new SelectList(masters);

            ViewBag.Services = new SelectList(db.Services, "ID", "Name");
            return View();
        }
        public IActionResult AdminIndex()
        {
            return View();
        }



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
            if (ModelState.IsValid)
            {
                // добавляем заказ в бд
                db.Orders.Add(new Order
                {
                    Date = modelorder.Date,
                    MasterID = modelorder.MasterID,
                    ServiceID = modelorder.ServiceID,
                    UserID = modelorder.UserID
                });
                await db.SaveChangesAsync();


                return RedirectToAction("Home", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }
            //return RedirectToAction("Home", "Home");
            return Ok(modelorder);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
