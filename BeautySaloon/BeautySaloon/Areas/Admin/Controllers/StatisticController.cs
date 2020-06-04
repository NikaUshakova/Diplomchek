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
    public class StatisticController : Controller
    {
        private readonly ApplicationContext _context;

        public StatisticController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Admin/Statistic
        public async Task<IActionResult> Index()
        {
            return View(await _context.Masters.ToListAsync());
        }

        // GET: Admin/Statistic/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var master = await _context.Masters
                .FirstOrDefaultAsync(m => m.ID == id);
            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // GET: Admin/Statistic/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Statistic/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Surname,Patronymic,Date,Phone")] Master master)
        {
            if (ModelState.IsValid)
            {
                _context.Add(master);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(master);
        }

        // GET: Admin/Statistic/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var master = await _context.Masters.FindAsync(id);
            if (master == null)
            {
                return NotFound();
            }
            return View(master);
        }

        // POST: Admin/Statistic/Edit/5
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
                    _context.Update(master);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(master);
        }

        // GET: Admin/Statistic/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var master = await _context.Masters
                .FirstOrDefaultAsync(m => m.ID == id);
            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // POST: Admin/Statistic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var master = await _context.Masters.FindAsync(id);
            _context.Masters.Remove(master);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MasterExists(int id)
        {
            return _context.Masters.Any(e => e.ID == id);
        }
    }
}
