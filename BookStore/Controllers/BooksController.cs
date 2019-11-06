using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BookStore.Models;
using BookStore.Services;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {

        private AppDbContext _context;
        private CategoriesService categoriesService;

        public BooksController()
        {
            _context = new AppDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext>());
            categoriesService = new CategoriesService();
        }


        [HttpGet]
        public IActionResult AddBook()
        {
            AddBookViewModel model = new AddBookViewModel {
                Categories = new SelectList(categoriesService.GetAllCategories(), "Id", "Name")
            };

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult AddBook(AddBookViewModel model)
        {
            if(ModelState.IsValid)
            {
                var newBook = new Book
                {
                    Title = HttpUtility.HtmlEncode(model.Title),
                    Description = HttpUtility.HtmlEncode(model.Description),
                    QuantityAvailable = model.Quantity,
                    Price = model.Price,
                    Category = categoriesService.GetCategoryById(model.Category)
                };

                try
                {
                    var addBook = _context.Books.Add(newBook);
                    _context.SaveChanges();
                    ViewBag.Message = "Book created successfully.";
                    return View();
                }
                catch(Exception ex)
                {
                    //Refactor to display proper message
                    ViewBag.Message = ex.Message;
                    return View();
                }

            }

            return View();
        }
    }
}