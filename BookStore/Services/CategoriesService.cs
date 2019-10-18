using BookStore.Models;
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
            this._context = new AppDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext>());
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
