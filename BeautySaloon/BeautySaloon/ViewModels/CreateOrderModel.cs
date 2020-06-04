using BeautySaloon.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.ViewModels
{
    public class CreateOrderModel
    {
        public int ID { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime OrderDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm}")]
        public DateTime Time { get; set; }
        public int? MasterID { get; set; }
        public Master Master { get; set; }
        public int? UserID { get; set; }
        public User User { get; set; }
        public int? ServiceID { get; set; }
        public Service Service { get; set; }

    }
}
