using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public DateTime Date { get; set; }

        public string UserName { get; set; }

        public string Symbol { get; set; }

        public int Amount { get; set; }

        public double PricePerUnit { get; set; }

        public double Total { get; set; }

    }
}
