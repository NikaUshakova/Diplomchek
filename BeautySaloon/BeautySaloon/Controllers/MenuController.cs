using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BeautySaloon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MimeKit;
using MailKit.Net.Smtp;

namespace BeautySaloon.Controllers
{
    [Authorize]  
    public class MenuController : Controller
    {
        private readonly ApplicationContext db;
       
        public MenuController(ApplicationContext context)
        {
            db = context;
        }

        [AllowAnonymous]
        public IActionResult Home()
        {
            ViewBag.Services1 = db.Services.Where(s => s.Category.Equals("Парикмахерские услуги")).Select(n => n.Name + " " + n.Price +" BYN");
            ViewBag.Services2 = db.Services.Where(s => s.Category.Equals("Прически")).Select(n => n.Name + " " + n.Price + " BYN");
            ViewBag.Services3 = db.Services.Where(s => s.Category.Equals("Ногтевой сервис")).Select(n => n.Name + " " + n.Price + " BYN");
            return View();
        }
       

        public async Task<IActionResult> SendMessage(string Name, string Message, string Emailsend)
        {
            EmailService emailService = new EmailService();
            await emailService.GetEmailAsync("BeautycenterNika@gmail.com", "Feedback", Message, Emailsend, Name);
            return RedirectToAction(nameof(Home));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


       
    }
}
   
