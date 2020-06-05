using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BeautySaloon.Models;
using BeautySaloon.ViewModels;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            ViewBag.Services1 = db.Services.Where(s => s.Category.Equals("Парикмахерские услуги")).Select(n => n.Name);
            ViewBag.Services2 = db.Services.Where(s => s.Category.Equals("Прически")).Select(n => n.Name);
            ViewBag.Services3 = db.Services.Where(s => s.Category.Equals("Ногтевой сервис")).Select(n => n.Name);
            return View();
        }      
        

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


       
    }
}
   
