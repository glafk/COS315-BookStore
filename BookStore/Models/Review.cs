using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public Book book { get; set; }

        [ForeignKey("Books")]
        public int BookID { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string CustomerReview { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReviewDate { get; set; }
    }
}
