using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Category
    {

        public Category()
        {;
            this.Books = new List<Book>();
        }

        public int Id { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
