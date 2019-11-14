using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class AddCategoryViewModel
    {
        [Required]
        public string Name { get; set; }


    }
}
