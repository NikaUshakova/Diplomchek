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
            ViewBag.Masters = db.Masters;
            var users = db.Users.ToList();
            ViewBag.Users = users.Select(i => (
               i.Name,
               i.Surname
            ));

            return View();
        }
        public IActionResult AdminIndex()
        {

            return View();
        }



        // GET: Clients/Create
        public IActionResult CreateOrder()
        {
            // var users = db.Users.ToList();
            // ViewBag.Masters =  new SelectList(masters, "ID", "Name");
            //ViewBag.Users = new SelectList(users, "ID", "Name");
            //SelectList users = new SelectList(db.Users, "ID", "Name");
            //ViewBag.Users = users;
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            //if (ModelState.IsValid)
            //{                
            //    // string InsertOrder = "INSERT INTO orders VALUES (0,concat_ws('-', " + day.Year + ", " + day.Month + "," + day.Day + ")," + IdMaster + ")";
            //    if (order == null)
            //    {
            // добавляем заказ
            db.Orders.Add(order);
            await db.SaveChangesAsync();


            return RedirectToAction("Home", "Home");
            //    }
            //    else
            //        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            //}
            //return View(order);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
