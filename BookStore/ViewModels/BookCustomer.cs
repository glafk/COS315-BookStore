using BookStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class BookCustomer
    {
       public User User { get; set; }
       public Book Book { get; set; }
    }
}
