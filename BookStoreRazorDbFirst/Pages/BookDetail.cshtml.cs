using BookStoreRazorDbFirst.Models;
using BookStoreRazorDbFirst.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStoreRazorDbFirst.Pages
{
    public class BookDetailModel : PageModel
    {
        private readonly BookService _bookService;

        public BookDetailModel(BookService bookService)
        {
            _bookService = bookService;
        }

        public Book Book { get; set; }

        public void OnGet(int id)
        {
            Book = _bookService.GetById(id);
        }
    }
}