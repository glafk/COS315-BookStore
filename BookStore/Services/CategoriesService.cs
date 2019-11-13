using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class CategoriesService
    {
        private AppDbContext _context;

        public CategoriesService()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BookStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            this._context = new AppDbContext(optionsBuilder.Options);
        }

        public List<Category> GetAllCategories()
        {

            return new List<Category>(_context.Categories.ToList());
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.Single(x => x.Id == id);
        }
    }
}
