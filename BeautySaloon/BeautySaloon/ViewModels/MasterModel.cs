using BeautySaloon.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.ViewModels
{
    public class MasterModel
    {
        public IEnumerable<Master> Masters { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Phone { get; set; }
    }
}
