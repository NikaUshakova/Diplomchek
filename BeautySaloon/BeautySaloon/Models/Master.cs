using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.Models
{
    public class Master
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Phone { get; set; }

        public IList<Order> Orders { get; set; }
        public Master()
        {
            Orders = new List<Order>();
        }
    }
}
