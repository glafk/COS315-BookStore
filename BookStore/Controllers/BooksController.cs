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

        public async Task<IActionResult> Index(string category, string searchString, int? pageNumber)
        {
            //Get all books and categories from the database
            var categories = await _context.Categories.ToListAsync();
            var books = from book in _context.Books select book;

            //Storing the filters in viewdata object
            ViewData["CategoryFilter"] = !String.IsNullOrEmpty(category) ? category : "All";
            ViewData["CurrentFilter"] = searchString;


            //Get the books only from the requested category.
            if (!String.IsNullOrEmpty(category) && category != "All")
            {
                books = books.Where(b => b.Category.Name == category);
            }

            //Search the title for the given search string
            if (!String.IsNullOrEmpty(searchString)){
                books = books.Where(b => b.Title.Contains(searchString));
            }

            //Create the model for the view
            Tuple<IEnumerable<Category>, PaginatedList<Book>> model = new Tuple<IEnumerable<Category>, PaginatedList<Book>>(categories, await PaginatedList<Book>.CreateAsync(books.AsNoTracking().Include(b => b.Category), pageNumber ?? 1, 10));

            return View(model);
        }

        public async Task<IActionResult> ReadReviews(int? id)
        {
            return View(await _context.Review.Where(review => review.BookID == id).Include(review => review.book).Where(review => review.book.Id == id).ToListAsync());
        }
    }
}