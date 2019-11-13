using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        private ICollection<Book> books;

        public virtual ICollection<Book> Books { get { return books; } set { books = value; } }
    }
}
