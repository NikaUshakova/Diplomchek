using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BeautySaloon.Models
{
    [DataContract]
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Phone { get; set; }
        [DataMember]
        public string Email { get; set; }
        public string Password { get; set; }

        public IList <Order> Orders { get; set; }
        public User()
        {
            Orders = new List<Order>();
        }
    }
}
