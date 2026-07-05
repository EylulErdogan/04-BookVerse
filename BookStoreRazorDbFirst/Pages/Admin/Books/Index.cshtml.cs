using BookStoreRazorDbFirst.Models;
using BookStoreRazorDbFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace BookStoreRazorDbFirst.Pages.Admin.Books
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
            if (HttpContext.Session.GetString("AdminLogin") == null)
            {
                Response.Redirect("/Login");
                return;
            }
            Search = search;
            Books = _bookService.GetAll(search ?? "");
        }
        public IActionResult OnPostDelete(int id)
        {
            _bookService.Delete(id);
            return RedirectToPage();
        }
    }
}