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
        [Microsoft.AspNetCore.Mvc.Route("admin/doneworks")]
        public IActionResult DoneWorks(int? service, string master, string user)
        {
            IQueryable<Order> orders = db.Orders.Include(o => o.Master).Include(o => o.Service).Include(o => o.User);
            if (service != null && service != 0)
            {
                orders = orders.Where(o => o.ServiceID == service);
            }
            if (!String.IsNullOrEmpty(master) && !master.Equals("Все"))
            {
                orders = orders.Where(o => (o.Master.Surname+" "+o.Master.Name + " " + o.Master.Patronymic).Contains(master));
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

            //string ID =masters.Select(m => m.ID).ToString();
            //string FIO = masters.Select(m => m.Surname + m.Name + m.Patronymic).ToString();
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
                                }).ToList(),
            };
              
            //ViewBag.Masters = db.Masters.Select(a =>
            //                     new SelectListItem
            //                     {
            //                         Value = a.ID.ToString(),
            //                         Text = a.Surname + " " + a.Name + " " + a.Patronymic
            //                     }).ToList();

        //ViewBag.Services = new SelectList(db.Services, "ID", "Name");
            return View(DW);
        }


        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(order);
        }



        //GET: Order/Edit/5
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
            ViewData["MasterID"] = new SelectList(db.Masters, "ID", "ID", order.MasterID);
            ViewData["ServiceID"] = new SelectList(db.Services, "ID", "ID", order.ServiceID);
            ViewData["UserID"] = new SelectList(db.Users, "ID", "ID", order.UserID);
            return View(order);
        }

        //// POST: Order/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,Date,MasterID,UserID,ServiceID")] Order order)
        //{
        //    if (id != order.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            db.Update(order);
        //            await db.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MasterID"] = new SelectList(db.Masters, "ID", "ID", order.MasterID);
        //    ViewData["ServiceID"] = new SelectList(db.Services, "ID", "ID", order.ServiceID);
        //    ViewData["UserID"] = new SelectList(db.Users, "ID", "ID", order.UserID);
        //    return View(order);
        //}

        // GET: Order/Delete/5
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

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
