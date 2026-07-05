using BookStoreRazorDbFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStoreRazorDbFirst.Pages
{
    public class LoginModel : PageModel
    {
        private readonly BookService _bookService;

        public LoginModel(BookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            bool result = _bookService.Login(UserName, Password);

            if (result)
            {
                HttpContext.Session.SetString("AdminLogin", "true");
                return RedirectToPage("/Admin/Books/Index");
            }

            ErrorMessage = "Kullanıcı adı veya şifre hatalı";
            return Page();
        }
    }
}