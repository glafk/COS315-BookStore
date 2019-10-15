using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        public string Address { get; set; }

        public string CreditCardNumber { get; set; }

        public string CreditCardSVC { get; set; }

        public string CreditCardLegalName { get; set; }
    }
}
