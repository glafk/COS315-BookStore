using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {

        private AppDbContext _context;

        public BooksController()
        {
            _context = new AppDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext>());
        }


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(AddBookViewModel model)
        {
            if(ModelState.IsValid)
            {
                var newBook = new Book
                {
                    Title = model.Title,
                    Description = model.Description,
                    QuantityAvailable = model.Quantity,
                    Price = model.Price

                };

                try
                {
                    var addBook = _context.Books.Add(newBook);
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