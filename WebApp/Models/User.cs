using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class User
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(40, MinimumLength = 4)]
        [Key]
        public string Username { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(40, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(40, MinimumLength = 6)]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool Admin { get; set; }

        public CreditCard CreditCard { get; set; }
        public ICollection<Stock> OwnedStocks { get; set; }
    }
}