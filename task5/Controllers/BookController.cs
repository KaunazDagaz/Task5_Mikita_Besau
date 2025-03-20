using Microsoft.AspNetCore.Mvc;
using task5.Services.IServices;

namespace task5.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService bookService;
        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadBooks(int count, int seed, float avgLikes)
        {
            var books = await bookService.GenerateBooksAsync(count, seed, avgLikes);
            return Json(books);
        }
    }
}
