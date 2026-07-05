using BookStoreRazorDbFirst.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStoreRazorDbFirst.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly BookService _bookService;

        public DashboardModel(BookService bookService)
        {
            _bookService = bookService;
        }

        public int TotalBook { get; set; }
        public int TotalStock { get; set; }
        public decimal TotalValue { get; set; }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("AdminLogin") == null)
            {
                Response.Redirect("/Login");
                return;
            }

            TotalBook = _bookService.TotalBookCount();
            TotalStock = _bookService.TotalStock();
            TotalValue = _bookService.TotalValue();
        }
    }
}