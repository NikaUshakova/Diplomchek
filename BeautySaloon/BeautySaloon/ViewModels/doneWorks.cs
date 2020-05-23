using BeautySaloon.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.ViewModels
{
    public class doneWorks
    {
        public IEnumerable<Order> Orders { get; set; }       
        public SelectList Services { get; set; }
        public List<SelectListItem> Masters { get; set; }
        public List<SelectListItem> Users { get; set; }
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
