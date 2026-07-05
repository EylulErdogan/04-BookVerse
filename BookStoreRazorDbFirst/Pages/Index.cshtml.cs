using BookStoreRazorDbFirst.Models;
using BookStoreRazorDbFirst.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStoreRazorDbFirst.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BookService _bookService;

        public IndexModel(BookService bookService)
        {
            _bookService = bookService;
        }

        public List<Book> Books { get; set; } = new List<Book>();
        public string Search { get; set; }

        public void OnGet(string search)
        {
            Search = search;
            Books = _bookService.GetAll(search ?? "");
        }
    }
}