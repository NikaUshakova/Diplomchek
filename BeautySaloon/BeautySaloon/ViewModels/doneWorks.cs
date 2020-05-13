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
        public SelectList Masters { get; set; }
        public SelectList Services { get; set; }
       
    }
}
