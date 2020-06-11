using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautySaloon.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Security.Cryptography;

namespace BeautySaloon.Controllers
{
    public class PersonalOfficeController : Controller
    {
        private readonly ApplicationContext db;

        public PersonalOfficeController(ApplicationContext context)
        {
            db = context;
        }

        // GET: PersonalOffice
        public IActionResult Index()
        {
            var user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            return View(user);
        }

        // GET: PersonalOffice/Details/5
        public IActionResult Orders()
        {
            var orders = db.Orders.Include(o => o.Master).Include(o => o.Service).Include(o => o.User).Where(o => o.User.Email == User.Identity.Name).OrderBy(o => o.Date).ToList();
            
            return View(orders);
        }

        // GET: PersonalOffice/Edit/5
        public IActionResult Edit()
        {
            var user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }
            return  View(user);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User model)
        {
            User user = await db.Users.FirstOrDefaultAsync(cl => cl.Email == User.Identity.Name);
            var oldpass = db.Users.Where(u => u.Email == User.Identity.Name).Select(u=>u.Password).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    user.Surname = model.Surname;
                    user.Name = model.Name;
                    user.Patronymic = model.Patronymic;
                    user.Date = model.Date;
                    user.Phone = model.Phone;
                    user.Email = User.Identity.Name;
                    user.Password = GetHashString(model.Password);
                   
                    await db.SaveChangesAsync();
                    if (GetHashString(model.Password) != oldpass[0])
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else 
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(model.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }
      
        string GetHashString(string s)
        {
            //переводим строку в байт-массив  
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }
        private bool UserExists(int id)
        {
            return db.Users.Any(e => e.ID == id);
        }
    }
}
