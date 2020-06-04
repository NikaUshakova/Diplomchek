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
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
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
          
            ViewBag.Services = new SelectList(db.Services, "ID", "Name");
            ViewBag.Time = "";
            ViewData["NowDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        [HttpPost]
        public IActionResult GetDateTime(DateTime day, int? master)
        {
            List<string> numbers = new List<string>() { "09:00", "10:00", "11:00", "12:00","13:00", "14:00","15:00", "16:00", "17:00", "18:00", "19:00", "20:00" };
            var datetime = db.Orders.Where(o=>o.MasterID==master).Select(o => o.Date).ToList(); //все даты с бд заказов (без времени)

            string sday = day.ToShortDateString();  //короткая дата с календаря
            foreach (var item in datetime)
            {
                if (item.ToShortDateString().Equals(sday))  //если датa в заказах равна датe с кландаря
                {
                    for (int i = 0; i < numbers.Count; i++)
                    {
                        if (item.ToShortTimeString().Contains(numbers[i]))
                        {
                             numbers.Remove(numbers[i]);
                             i--;
                            if (i == 0) numbers.Add("Свободных мест нет");
                        }
                          
                    }
                }
            }
                ViewBag.Time = numbers;
            return Ok(numbers);
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
        public async Task<IActionResult> CreateOrder(CreateOrderModel modelorder)
        {
            string date = modelorder.Date.ToShortDateString() + " " + modelorder.Time;
            DateTime dt = DateTime.Parse(date);
            user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    // добавляем заказ в бд
                    db.Orders.Add(new Order
                    {
                        Date = dt,
                        MasterID = modelorder.MasterID,
                        ServiceID = modelorder.ServiceID,
                        UserID = user.ID,
                        OrderDate = DateTime.Now
                    }); ;
                    await db.SaveChangesAsync();


                    return RedirectToAction("Home", "Menu");
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
   
