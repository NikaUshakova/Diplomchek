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
        public IActionResult Index(DateTime? begindate, DateTime? enddate)
        {
            var list = new List<CountMaster>();
            if (begindate == null && enddate == null)
            {
                list = db.Orders.Include(m => m.Master).GroupBy(l => l.Master.Surname + " " + l.Master.Name).Select(l => new CountMaster
                {
                    Master = l.Key,
                    TotalService = l.Count()
                }).ToList();
            }
            else if (begindate != null && enddate != null)
            {
                list = db.Orders.Include(m => m.Master).Where(o => o.Date >= begindate && o.Date <= enddate).GroupBy(l => l.Master.Surname + " " + l.Master.Name).Select(l => new CountMaster
                {
                    Master = l.Key,
                    TotalService = l.Count()
                }).ToList(); 
            }
            else if (begindate != null)
            {
                list = db.Orders.Include(m => m.Master).Where(o => o.Date >= begindate).GroupBy(l => l.Master.Surname + " " + l.Master.Name).Select(l => new CountMaster
                {
                    Master = l.Key,
                    TotalService = l.Count()
                }).ToList(); 
            }
            else if (enddate != null)
            {
                list = db.Orders.Include(m => m.Master).Where(o => o.Date <= enddate).GroupBy(l => l.Master.Surname + " " + l.Master.Name).Select(l => new CountMaster
                {
                    Master = l.Key,
                    TotalService = l.Count()
                }).ToList(); 
            }
            List<DataPoint> dataPoints = new List<DataPoint>();
			foreach (var item in list)
			{
				dataPoints.Add(new DataPoint(item.Master.ToString(), Convert.ToInt32(item.TotalService)));
			}			
			ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
    /////////////////
    ////////////////
            var top = db.Orders.Include(m => m.Service).GroupBy(l => l.Service.Name).Select(l => new CountServ
                {
                    Service = l.Key,
                    Total = l.Count()
                }).OrderByDescending(l=>l.Total).Take(5).ToList();
       
            List<DataPoint> dataPoints1 = new List<DataPoint>();
            foreach (var item in top)
            {
                dataPoints1.Add(new DataPoint(item.Service.ToString(), Convert.ToInt32(item.Total)));
            }
            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);

     ////////////////////
       ////////////////////
            var topClient = new List<CountClient>();
            if (begindate == null && enddate == null)
            {
                topClient = db.Orders.Include(m => m.User).GroupBy(l => l.User.Surname + " " + l.User.Name).Select(l => new CountClient
                {
                    Client = l.Key,
                    Total = l.Count()
                }).OrderByDescending(l => l.Total).Take(5).ToList();
            }
            else if (begindate != null && enddate != null)
            {
                topClient = db.Orders.Include(m => m.User).Where(o => o.Date >= begindate && o.Date <= enddate).GroupBy(l => l.User.Surname + " " + l.User.Name).Select(l => new CountClient
                {
                    Client = l.Key,
                    Total = l.Count()
                }).OrderByDescending(l => l.Total).Take(5).ToList();
            }
            else if (begindate != null)
            {
                topClient = db.Orders.Include(m => m.User).Where(o => o.Date >= begindate).GroupBy(l => l.User.Surname + " " + l.User.Name).Select(l => new CountClient
                {
                    Client = l.Key,
                    Total = l.Count()
                }).OrderByDescending(l => l.Total).Take(5).ToList();
            }
            else if (enddate != null)
            {
                topClient = db.Orders.Include(m => m.User).Where(o => o.Date <= enddate).GroupBy(l => l.User.Surname + " " + l.User.Name).Select(l => new CountClient
                {
                    Client = l.Key,
                    Total = l.Count()
                }).OrderByDescending(l => l.Total).Take(5).ToList();
            }
            List<DataPoint> dataPoints2 = new List<DataPoint>();
            foreach (var item in topClient)
            {
                dataPoints2.Add(new DataPoint(item.Client.ToString(), Convert.ToInt32(item.Total)));
            }
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);

            //if (begindate == null && enddate == null)
            //{
            //    ViewBag.BeginDate = begindate;
            //    ViewBag.EndDate = enddate;
            //}
            //if (begindate != null && enddate == null)
            //{
            //    ViewBag.BeginDate = begindate.Value.ToShortDateString();
            //    ViewBag.EndDate = enddate;
            //}
            //else if(begindate == null && enddate != null)
            //{
            //    ViewBag.BeginDate = begindate;
            //    ViewBag.EndDate = enddate.Value.ToShortDateString();
            //}
            //else if (begindate != null && enddate != null)
            //{
            //    ViewBag.BeginDate = begindate.Value.ToShortDateString();
            //    ViewBag.EndDate = enddate.Value.ToShortDateString();
            //}

            return View();
		}         

        
       

    }
}
