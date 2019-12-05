using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class AddReviewModel
    {
        [Required]
        public int BookID { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string CustomerReview { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        
    }
}
