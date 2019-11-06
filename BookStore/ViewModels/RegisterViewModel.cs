using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class RegisterViewModel
    {

        [Required]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [DisplayName("Credit Card Number")]
        public string CreditCardNumber { get; set; } 

        [Required]
        [DisplayName("Credit Card CVS")]
        public string CeditCardCVS { get; set; }

        [Required]
        [DisplayName("Credit Card Full Legal Name")]
        public string CreditCardLegalName { get; set; }

    }
}
