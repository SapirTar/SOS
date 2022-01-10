using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class OrderAndStockViewModel
    {
        [Display(Name = "Order Amount")]
        public int OrderAmount { get; set; }
        [Display(Name = "Stock Symbol")]
        public IEnumerable<OrderAndStock> StockOrders { get; set; }
    }
}
