using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.Models
{
    public class OrderService
    {
        public int ID { get; set; }
        public int? OrderID { get; set; }
        public Order Order { get; set; }
        public int? ServiceID { get; set; }
        public Service Service { get; set; }
    }
}
