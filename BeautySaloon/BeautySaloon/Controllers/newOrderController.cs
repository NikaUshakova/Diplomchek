using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautySaloon.Models;

namespace BeautySaloon.Controllers
{
    public class newOrderController : Controller
    {
        private readonly ApplicationContext db;
        private User user;
        public newOrderController(ApplicationContext context)
        {
            db = context;
        }
        [Route("home/neworders")]
        public IActionResult Online()
        {
           
            ViewBag.Masters = db.Masters.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.ID.ToString(),
                                      Text = a.Surname + " " + a.Name + " " + a.Patronymic
                                  }).ToList();

            ViewBag.Services = new SelectList(db.Services, "ID", "Name");
            ViewBag.Time = "";
            return View();
        }
        public IActionResult Combo()
        {
            ViewBag.Masters = db.Masters.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.ID.ToString(),
                                      Text = a.Surname + " " + a.Name + " " + a.Patronymic
                                  }).ToList();
            var serv = db.Services.Where(s => s.Category == "Комплекс");
            ViewBag.Services = new SelectList(serv, "ID", "Name");
            ViewBag.Time = "";
            return View("Online");
        }

        [Route("home/neworders/time")]
        public IActionResult Time(DateTime day, int? master, int? service)
        {
            List<string> numbers = new List<string>() { "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00" };
            var datetime = db.Orders.ToList().Where(o => o.MasterID == master && o.Date.ToShortDateString()==day.ToShortDateString()).Select(o => o.Date).ToList(); //даты с бд заказов, равные дате с календаря с выбранным мастером
            foreach (var item in datetime)
            {
                for (int i = 0; i < numbers.Count; i++)
                {
                    if (item.ToString("HH:mm").Equals(numbers[i]))
                    {
                        var category = db.Orders.Include(s => s.Service).ToList().Where(s => s.Date == item).Select(s => s.Service.Category).ToList(); //получили категорию услуги, которая уже есть в этот день
                                                                                                                                                       // var cat = db.Orders.Include(s => s.Service).ToList().Where(s => s.ServiceID == service).Select(s => s.Service.Category).ToList(); //получили категорию услуги, которую выбрал клиент
                                                                                                                                                       //if (cat[0] == "Прически")
                                                                                                                                                       //{
                                                                                                                                                       //    try
                                                                                                                                                       //    {
                                                                                                                                                       //    numbers.RemoveAt(i);
                                                                                                                                                       //    numbers.RemoveAt(i-1);
                                                                                                                                                       //}
                                                                                                                                                       //    catch
                                                                                                                                                       //    {
                                                                                                                                                       //        numbers.RemoveAt(i);
                                                                                                                                                       //    }
                                                                                                                                                       //}
                                                                                                                                                       //else if (cat[0] == "Парикмахерские услуги")
                                                                                                                                                       //{
                                                                                                                                                       //    numbers.RemoveAt(i);
                                                                                                                                                       //}
                                                                                                                                                       //else if (cat[0] == "Ногтевой сервис")
                                                                                                                                                       //{
                                                                                                                                                       //    try
                                                                                                                                                       //    {
                                                                                                                                                       //    numbers.RemoveAt(i);
                                                                                                                                                       //    numbers.RemoveAt(i - 1);
                                                                                                                                                       //    numbers.RemoveAt(i-  2);
                                                                                                                                                       //}
                                                                                                                                                       //    catch
                                                                                                                                                       //    {
                                                                                                                                                       //        numbers.RemoveAt(i);
                                                                                                                                                       //    }
                                                                                                                                                       //}                                                                                                                                                                                                                                                                                                                                                                                                          
                        if (category[0] == "Прически")
                        {
                            try
                            {
                                numbers.RemoveRange(i, 2);
                            }
                            catch
                            {
                                numbers.RemoveAt(i);
                            }
                        }
                        else if (category[0] == "Парикмахерские услуги")
                        {
                            numbers.RemoveAt(i);
                        }
                        else if (category[0] == "Ногтевой сервис")
                        {
                            try
                            {
                                numbers.RemoveRange(i, 3);
                            }
                            catch
                            {
                                numbers.RemoveAt(i);
                            }
                        }
                        break;
                    }

                }

            }

            if (numbers.Count == 0) 
            {
                ViewBag.Null = "Свободных мест нет";
            } 
            ViewBag.Time = numbers;
            ViewBag.Master = master;
            ViewBag.Service = service;
            ViewBag.Day = day;
            return View();
        }
        // GET: Clients/Create
        public IActionResult CreateOrder()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(DateTime day, int? master, int? service, string time)
        {
            string date = day.ToShortDateString() + " " + time;
            DateTime fulldate = DateTime.Parse(date);
            user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    // добавляем заказ в бд
                    db.Orders.Add(new Order
                    {
                        Date = fulldate,
                        MasterID = master,
                        ServiceID = service,
                        UserID = user.ID,
                        OrderDate = DateTime.Now.AddHours(3)
                    });
                    await db.SaveChangesAsync();

                    return RedirectToAction("Home", "Menu");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
                return Ok();
                //return Content(User.Identity.Name);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
