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
    public class MastersController : Controller
    {
        private readonly ApplicationContext db;

        public MastersController(ApplicationContext context)
        {
            db = context;
        }

        // GET: Admin/Masters
        [Route("admin/masters")]
        public async Task<IActionResult> Allmasters()
        {          
            return View(await db.Masters.ToListAsync());
        }


       //// GET: Admin/Masters/Create
       //[Route("admin/masters/add")]
       // public IActionResult Create()
       // {
       //     return View();
       // }

        // POST: Admin/Masters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,Name,Surname,Patronymic,Date,Phone")] Master master)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Add(master);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction(nameof(Allmasters));
        //    }
        //    return View(master);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ID, string Surname, string Name, string Patronymic, DateTime Date, string Phone)
        {
            db.Add(new Master
            {
                ID = ID,
                Surname = Surname,
                Name = Name,
                Patronymic = Patronymic,
                Date = Date,
                Phone = Phone
            });
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Allmasters));
        }

        // GET: Admin/Masters/Edit/5
        [Route("admin/masters/edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var master = await db.Masters.FindAsync(id);

            if (master == null)
            {
                return NotFound();
            }
            return View(master);
        }

        // POST: Admin/Masters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Surname,Patronymic,Date,Phone")] Master master)
        {
            if (id != master.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Masters.Update(master);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MasterExists(master.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Allmasters));
            }
            return View(master);
        }

        // GET: Admin/Masters/Delete/5
        [Route("admin/masters/delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var master = await db.Masters
                .FirstOrDefaultAsync(m => m.ID == id);
            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // POST: Admin/Masters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var master = await db.Masters.FindAsync(id);
            db.Masters.Remove(master);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Allmasters));
        }

        private bool MasterExists(int id)
        {
            return db.Masters.Any(e => e.ID == id);
        }
    }
}
