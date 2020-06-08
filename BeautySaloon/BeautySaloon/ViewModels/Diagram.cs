using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.ViewModels
{
    public class CountMaster
    {
        public string Master { get; set; }
        public int TotalService { get; set; }
        public DateTime Date { get; set; }
    }
    public class CountClient
    {
        public string Client { get; set; }
        public int Total { get; set; }
        public DateTime Date { get; set; }
    }
    public class CountServ
    {
        public string Service { get; set; }
        public int Total { get; set; }
        public DateTime Date { get; set; }
    }
}
