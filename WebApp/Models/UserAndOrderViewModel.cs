using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class UserAndOrderViewModel
    {
        [Display(Name = "User Order ID")]
        public string UserOrderId { get; set; }
        [Display(Name = "Orders")]
        public IEnumerable<UserAndOrder> UserOrders { get; set; }
    }
}
