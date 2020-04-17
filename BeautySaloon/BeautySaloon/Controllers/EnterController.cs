using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeautySaloon.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers
{
    public class EnterController : Controller
    {
        private ApplicationContext db;
        public EnterController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Enter()
        {
             return View();
        }
        public IActionResult Login()
        {
           return  RedirectToAction("Login", "Account");
           // return View("Login","Account");
        }
     
        public IActionResult Register()
        {
            return RedirectToAction("Register", "Account");
        }
    }
}