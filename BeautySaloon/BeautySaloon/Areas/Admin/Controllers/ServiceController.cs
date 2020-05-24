using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautySaloon.Models;

namespace BeautySaloon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly ApplicationContext db;

        public ServiceController(ApplicationContext context)
        {
            db = context;
        }

        // GET: Admin/Service
        [Route("admin/services")]
        public async Task<IActionResult> Allservices()
        {
            var services = db.Services.GroupBy(g => g.Category).Select(g =>
                                  new SelectListItem
                                  {
                                      Value = g.Key,
                                      Text = g.Key
                                  }).ToList();

            ViewBag.Categories = services;
            return View(await db.Services.ToListAsync());
        }

        // GET: Admin/Service/Create
        //[Route("admin/services/add")]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Admin/Service/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ID, string Name, int Price, string Category)
        {
            db.Add(new Service
            {
                ID = ID,
                Name = Name,
                Price = Price,
                Category = Category
            });
            await db.SaveChangesAsync();
                return RedirectToAction(nameof(Allservices));
        }

        // GET: Admin/Service/Edit/5
        [Route("admin/services/edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewBag.Categories = db.Services.GroupBy(g => g.Category).Select(g =>
                                  new SelectListItem
                                  {
                                      Value = g.Key,
                                      Text = g.Key
                                  }).ToList();
            return View(service);
        }

        // POST: Admin/Service/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/services/edit/{id?}")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Price,Category")] Service service)
        {
            if (id != service.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(service);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Allservices));
            }
            return Ok(service);
        }

        // GET: Admin/Service/Delete/5
        [Route("admin/services/delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await db.Services
                .FirstOrDefaultAsync(m => m.ID == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Admin/Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("admin/services/delete/{id?}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await db.Services.FindAsync(id);
            db.Services.Remove(service);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Allservices));
        }

        private bool ServiceExists(int id)
        {
            return db.Services.Any(e => e.ID == id);
        }
    }
}
