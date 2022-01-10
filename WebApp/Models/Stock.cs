using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Stock
    {
        [Key]
        [Required]
        public String Symbol { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }

        public double Change { get; set; }

        public string Category { get; set; }
        public ICollection<MarketIndex> Indices { get; set; }



    }
}