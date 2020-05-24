using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautySaloon.Models;
using Microsoft.AspNetCore.Components;
using BeautySaloon.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BeautySaloon.Controllers.Admin
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationContext db;

        public OrderController(ApplicationContext context)
        {
            db = context;
        }

        // GET: Order
        [Route("admin/doneworks")]
        public IActionResult DoneWorks(int? service, string master, string user, DateTime? begindate, DateTime? enddate )
        {
            IQueryable<Order> orders = db.Orders.Include(o => o.Master).Include(o => o.Service).Include(o => o.User);
            if (begindate != null && enddate != null)
            {
                orders = orders.Where(o => o.Date >= begindate && o.Date <= enddate);
            }
            else if (begindate != null)
            {
                orders = orders.Where(o => o.Date >= begindate);
            }
            else if (enddate != null)
            {
                orders = orders.Where(o => o.Date <= enddate);
            }
            if (service != null && service != 0)
            {
                orders = orders.Where(o => o.ServiceID == service);
            }
            if (!String.IsNullOrEmpty(master) && !master.Equals("Все"))
            {
                orders = orders.Where(o => (o.Master.Surname+" "+ o.Master.Name + " " + o.Master.Patronymic).Contains(master));
            }
            if (!String.IsNullOrEmpty(user) && !user.Equals("Все"))
            {
                orders = orders.Where(o => (o.User.Surname + " " + o.User.Name + " " + o.User.Patronymic).Contains(user));
            }
            List<Service> services = db.Services.ToList();
            List<Master> masters = db.Masters.ToList();
            List<User> users = db.Users.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            services.Insert(0, new Service { Name = "Все", ID = 0 });
            masters.Insert(0, new Master { Surname = "Все", ID = 0 });
            users.Insert(0, new User { Surname = "Все", ID = 0 });
           
            doneWorks DW = new doneWorks
            {
                Orders = orders.ToList(),               
                Services = new SelectList(services, "ID", "Name"),
                Masters = masters.Select(a =>
                                new SelectListItem
                                {
                                    Value = (a.Surname + " " + a.Name + " " + a.Patronymic).Trim(),
                                    Text = a.Surname + " " + a.Name + " " + a.Patronymic
                                }).ToList(),
                Users = users.Select(a =>
                                new SelectListItem
                                {
                                    Value = (a.Surname + " " + a.Name + " " + a.Patronymic).Trim(),
                                    Text = a.Surname + " " + a.Name + " " + a.Patronymic
                                }).ToList()              
            };
             
            return View(DW);
        }



        ////GET: Order/Edit/5
        [Route("admin/doneworks/edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.Masters = db.Masters.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.ID.ToString(),
                                     Text = a.Surname + " " + a.Name + " " + a.Patronymic
                                 }).ToList();
            ViewBag.Users = db.Users.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.ID.ToString(),
                                     Text = a.Surname + " " + a.Name + " " + a.Patronymic
                                 }).ToList();
            ViewBag.Services = new SelectList(db.Services, "ID", "Name");
            
            return View(order);
        }

        // POST: Order/Edit/5
          [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/doneworks/edit/{id?}")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Date,MasterID,UserID,ServiceID")] Order order)
        {
            if (id != order.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(order);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DoneWorks));
            }
            ViewBag.Masters = db.Masters.Select(a =>
                              new SelectListItem
                              {
                                  Value = a.ID.ToString(),
                                  Text = a.Surname + " " + a.Name + " " + a.Patronymic
                              }).ToList();
            ViewBag.Users = db.Users.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.ID.ToString(),
                                     Text = a.Surname + " " + a.Name + " " + a.Patronymic
                                 }).ToList();
            ViewBag.Services = new SelectList(db.Services, "ID", "Name");
            return RedirectToAction(nameof(DoneWorks));
        }

        // GET: Order/Delete/5
        [Route("admin/doneworks/delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await db.Orders
                .Include(o => o.Master)
                .Include(o => o.Service)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.Masters = db.Masters.Select(a =>
                              new SelectListItem
                              {
                                  Value = a.ID.ToString(),
                                  Text = a.Surname + " " + a.Name + " " + a.Patronymic
                              }).ToList();
            ViewBag.Users = db.Users.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.ID.ToString(),
                                     Text = a.Surname + " " + a.Name + " " + a.Patronymic
                                 }).ToList();
            ViewBag.Services = new SelectList(db.Services, "ID", "Name");

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("admin/doneworks/delete/{id?}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(DoneWorks));
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Any(e => e.ID == id);
        }
    }
}
