using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BookStore.Models;
using BookStore.Services;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace BookStore.Controllers
{
    public class BooksController : Controller
    {

        private AppDbContext _context;
        private UserManager<User> _userManager;
        private CategoriesService categoriesService;

        public BooksController(UserManager<User> userManager)
        {

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BookStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            this._context = new AppDbContext(optionsBuilder.Options);
            categoriesService = new CategoriesService();
            _userManager = userManager;
        }


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddBook()
        {
            AddBookViewModel model = new AddBookViewModel {
                Categories = new SelectList(_context.Categories.ToList(), "Id", "Name")
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
                    Author = model.Author,
                    Price = model.Price,
                    Category = _context.Categories.Single(x => x.Id == model.Category)
                };

                try
                {
                    var addBook = _context.Books.Add(newBook);
                    _context.SaveChanges();
                    ViewBag.Message = "Book created successfully.";
                    model.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
                    return View(model);
                }
                catch(Exception ex)
                {
                    //Refactor to display proper message
                    ViewBag.Message = ex.Message;
                    return View(model);
                }

            }

            return View();
        }
		
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = new BookCustomer
            {
                Book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id),
                User = await _userManager.GetUserAsync(HttpContext.User)
            };

            if (model.Book == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Books.ToListAsync();
            return View(model);
        }
    }
}