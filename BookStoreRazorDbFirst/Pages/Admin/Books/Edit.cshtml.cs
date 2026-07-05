using BookStoreRazorDbFirst.Models;
using BookStoreRazorDbFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStoreRazorDbFirst.Pages.Admin.Books
{
    public class EditModel : PageModel
    {
        private readonly BookService _bookService;

        public EditModel(BookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty]
        public Book Book { get; set; }

        public void OnGet(int id)
        {
            if (HttpContext.Session.GetString("AdminLogin") == null)
            {
                Response.Redirect("/Login");
                return;
            }
            Book = _bookService.GetById(id);
        }

        public IActionResult OnPost()
        {
            _bookService.Update(Book);
            return RedirectToPage("Index");
        }
    }
}