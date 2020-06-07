using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeautySaloon.Models;
using BeautySaloon.ViewModels;
using System.Web;
using System;
using Newtonsoft.Json;

namespace BeautySaloon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticController : Controller
    {
        private readonly ApplicationContext db;

        public StatisticController(ApplicationContext context)
        {
            db = context;
        }


        // GET: Admin/Statistic
        public IActionResult Index()
        {
            var list = db.Orders.Include(m=>m.Master).GroupBy(l => l.Master.Name).Select(l => new CountServ
            {
                Master = l.Key,
                TotalService = l.Count()
            }).ToList();
		
			List<DataPoint> dataPoints = new List<DataPoint>();
			foreach (var item in list)
			{
				dataPoints.Add(new DataPoint(item.Master.ToString(), Convert.ToInt32(item.TotalService)));
			}			
			ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

			return View();
		}         
        
       

    }
}
