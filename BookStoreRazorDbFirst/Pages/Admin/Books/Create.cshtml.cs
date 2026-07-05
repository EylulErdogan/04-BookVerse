using BookStoreRazorDbFirst.Models;
using BookStoreRazorDbFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStoreRazorDbFirst.Pages.Admin.Books
{
    public class CreateModel : PageModel
    {
        private readonly BookService _bookService;

        public CreateModel(BookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty]
        public Book Book { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            _bookService.Add(Book);
            return RedirectToPage("Index");
        }
    }
}