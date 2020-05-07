using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautySaloon.Models;

namespace BeautySaloon.Controllers.Admin
{
    public class OrderController : Controller
    {
        private readonly ApplicationContext db;

        public OrderController(ApplicationContext context)
        {
            db = context;
        }

        // GET: Order
      //[Route("Admin/Order/DoneWorks")]
        public async Task<IActionResult> DoneWorks()
        {
            var orders = db.Orders.Include(o => o.Master).Include(o => o.Service).Include(o => o.User);
            return View(await orders.ToListAsync());
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

       

        // GET: Order/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await db.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["MasterID"] = new SelectList(db.Masters, "ID", "ID", order.MasterID);
        //    ViewData["ServiceID"] = new SelectList(db.Services, "ID", "ID", order.ServiceID);
        //    ViewData["UserID"] = new SelectList(db.Users, "ID", "ID", order.UserID);
        //    return View(order);
        //}

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
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Any(e => e.ID == id);
        }
    }
}
