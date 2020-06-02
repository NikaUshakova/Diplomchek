using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautySaloon.Models;
using System.Globalization;

namespace BeautySaloon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationContext db;

        public UsersController(ApplicationContext context)
        {
            db = context;
        }

        // GET: Admin/Users
        [Route("admin/clients")]
        public async Task<IActionResult> Allusers()
        {
            return View(await db.Users.ToListAsync());
        }
            
        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ID, string Surname, string Name, string Patronymic, DateTime Date, string Phone, string Email)
        {
            EmailService emailService = new EmailService();
            string URL = "https://beautysaloonnika.azurewebsites.net/";
            string URLpass = "КИНУТЬ СТРИНГУ ЗАБЫТЫЙ ПАРОЛЬ";
            string message = @"<html>
                                   <body>
                <h3> " + Name + @", добро пожаловать в салон красоты 'Nika'! ⠀</h3>
                <p>⠀⠀⠀⠀Спасибо, что посетили наш салон красоты. Мы будем рады видеть Вас на нашем <a href='" + URL + @"'>сайте</a> ! <br>
            Наши высококвалифицированные специалисты помогут Вам с вопросами в сфере красоты.  <br><br>
                    Ждем Вас снова в салоне красоты 'Nika'! </p>
                    <br>
                    <p>Ваш логин: " + Email + @"  <br>
                   Для завершения регистрации и создания личного кабинета пройдите по ссылке: " + URLpass + @" </p>
                   <br>
                   С уважением, администрация салона красоты 'Nika'!
                   </body>
                   </html>
                    ";
            User user = await db.Users.FirstOrDefaultAsync(cl => cl.Email == Email);
            if (user == null)
            {
                db.Add(new User
                {
                    ID = ID,
                    Surname = Surname,
                    Name = Name,
                    Patronymic = Patronymic,
                    Date = Date,
                    Phone = Phone,
                    Email = Email
                });
                await db.SaveChangesAsync();
                //РАССЫЛОЧКУ
                if (!String.IsNullOrEmpty(Email))
                {
                    await emailService.SendEmailAsync(Email, "Please complete your registration", message);
                }
                return RedirectToAction(nameof(Allusers));
            }
            else
                ModelState.AddModelError("Email", "Такой логин уже существует");

            
           
            return RedirectToAction(nameof(Allusers));
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Surname,Patronymic,Date,Phone,Email,Password")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(user);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Allusers));
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        [Route("admin/users/delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await db.Users
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Allusers));
        }

        private bool UserExists(int id)
        {
            return db.Users.Any(e => e.ID == id);
        }
    }
}
