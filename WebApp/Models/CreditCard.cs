using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class CreditCard
    {
        [Key]
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Card Number must be numeric")]
        public string CardNum { get; set; }
        [MaxLength(3)]
        [MinLength(3)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CVV be 3 digits")]
        [Required]
        public string CVV { get; set; }
        [Required]
        public string CardHolder { get; set; }
    }
}
