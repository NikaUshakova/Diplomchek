﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.Models
{
    public class Order
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int? MasterID { get; set; }
        public Master Master { get; set; }
        public int? UserID { get; set; }
        public User User { get; set; }
        public int? ServiceID { get; set; }
        public Service Service { get; set; }

    }
}
