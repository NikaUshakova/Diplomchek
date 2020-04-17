using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.Models
{
    public class Service
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }

        public ICollection<OrderService> OrderServices { get; set; }
        public Service()
        {
            OrderServices = new List<OrderService>();
        }
    }
}
