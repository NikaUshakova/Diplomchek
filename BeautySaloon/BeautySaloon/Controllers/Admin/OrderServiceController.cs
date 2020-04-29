using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautySaloon.Models;
using BeautySaloon.ViewModels;

namespace BeautySaloon.Controllers.Admin
{
    public class OrderServiceController : Controller
    {
        private readonly ApplicationContext db;

        public OrderServiceController(ApplicationContext context)
        {
            db = context;
        }

        // GET: OrderService
        public async Task<IActionResult> Index()
        {
            //string querySelectOrders = "SELECT orders.id_order,masters.Surname,masters.Name,masters.Patronymic,Group_concat(service.name_service SEPARATOR ', ')," +
            //                    " sum(service.price) ,concat_ws('-',day(orders.Date),month(orders.Date),year(orders.Date))" +
            //      " FROM service INNER JOIN order_service ON service.id_service=order_service.id_service " +
            //      " INNER JOIN orders ON orders.id_order= order_service.id_order" +
            //      " INNER JOIN masters ON masters.id_master = orders.id_master" +
            //      " GROUP BY order_service.id_order";
            var done = from orderServices in db.OrderServices
                       join service in db.Services on orderServices.ServiceID equals service.ID
                       join order in db.Orders on orderServices.OrderID equals order.ID
                       join master in db.Masters on order.MasterID equals master.ID
                       join user in db.Users on order.UserID equals user.ID
                       select new doneWorks
                       {
                           ID = order.ID,
                           surMaster = master.Surname,
                           nameMaster = master.Name,
                           patroMaster = master.Patronymic,
                           service = service.Name,
                           sum = service.Price,
                           date = order.Date,
                           sur = user.Surname,
                           name = user.Name,
                           patro = user.Patronymic
                       };
           // var done = db.Orders.Include(o => o.User).Include(o => o.Master).Include(o => o.OrderServices);
                        return View( await done.ToListAsync());
        }


        

        // GET: OrderService/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderService = await db.OrderServices
                .Include(o => o.Order)
                .Include(o => o.Service)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orderService == null)
            {
                return NotFound();
            }

            return View(orderService);
        }

        // GET: OrderService/Create
        public IActionResult Create()
        {
            ViewData["OrderID"] = new SelectList(db.Orders, "ID", "ID");
            ViewData["ServiceID"] = new SelectList(db.Services, "ID", "ID");
            return View();
        }

        // POST: OrderService/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OrderID,ServiceID")] OrderService orderService)
        {
            if (ModelState.IsValid)
            {
                db.Add(orderService);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderID"] = new SelectList(db.Orders, "ID", "ID", orderService.OrderID);
            ViewData["ServiceID"] = new SelectList(db.Services, "ID", "ID", orderService.ServiceID);
            return View(orderService);
        }

        // GET: OrderService/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderService = await db.OrderServices.FindAsync(id);
            if (orderService == null)
            {
                return NotFound();
            }
            ViewData["OrderID"] = new SelectList(db.Orders, "ID", "ID", orderService.OrderID);
            ViewData["ServiceID"] = new SelectList(db.Services, "ID", "ID", orderService.ServiceID);
            return View(orderService);
        }

        // POST: OrderService/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,OrderID,ServiceID")] OrderService orderService)
        {
            if (id != orderService.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(orderService);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderServiceExists(orderService.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderID"] = new SelectList(db.Orders, "ID", "ID", orderService.OrderID);
            ViewData["ServiceID"] = new SelectList(db.Services, "ID", "ID", orderService.ServiceID);
            return View(orderService);
        }

        // GET: OrderService/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderService = await db.OrderServices
                .Include(o => o.Order)
                .Include(o => o.Service)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orderService == null)
            {
                return NotFound();
            }

            return View(orderService);
        }

        // POST: OrderService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderService = await db.OrderServices.FindAsync(id);
            db.OrderServices.Remove(orderService);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderServiceExists(int id)
        {
            return db.OrderServices.Any(e => e.ID == id);
        }
    }
}
