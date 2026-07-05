using BookStoreRazorDbFirst.Models;
using BookStoreRazorDbFirst.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BookStoreRazorDbFirst.Pages.Admin.Reports
{
    public class IndexModel : PageModel
    {
        private readonly BookService _bookService;

        public IndexModel(BookService bookService)
        {
            _bookService = bookService;
        }

        public List<Book> Books { get; set; } = new List<Book>();
        public int TotalBookCount { get; set; }
        public int TotalStock { get; set; }
        public decimal TotalValue { get; set; }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("AdminLogin") == null)
            {
                Response.Redirect("/Login");
                return;
            }
            Books = _bookService.GetAll();
            TotalBookCount = _bookService.TotalBookCount();
            TotalStock = _bookService.TotalStock();
            TotalValue = _bookService.TotalValue();
        }

        public IActionResult OnGetExcel()
        {
            var books = _bookService.GetAll();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Kitap Raporu");

            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Kitap";
            worksheet.Cell(1, 3).Value = "Yazar";
            worksheet.Cell(1, 4).Value = "Kategori";
            worksheet.Cell(1, 5).Value = "Fiyat";
            worksheet.Cell(1, 6).Value = "Stok";

            int row = 2;

            foreach (var book in books)
            {
                worksheet.Cell(row, 1).Value = book.Id;
                worksheet.Cell(row, 2).Value = book.BookName;
                worksheet.Cell(row, 3).Value = book.Author;
                worksheet.Cell(row, 4).Value = book.Category;
                worksheet.Cell(row, 5).Value = book.Price;
                worksheet.Cell(row, 6).Value = book.Stock;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "KitapRaporu.xlsx");
        }

        public IActionResult OnGetPdf()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var books = _bookService.GetAll();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("Kitap Raporu")
                        .FontSize(22)
                        .Bold();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Id").Bold();
                            header.Cell().Text("Kitap").Bold();
                            header.Cell().Text("Yazar").Bold();
                            header.Cell().Text("Kategori").Bold();
                            header.Cell().Text("Fiyat").Bold();
                            header.Cell().Text("Stok").Bold();
                        });

                        foreach (var book in books)
                        {
                            table.Cell().Text(book.Id.ToString());
                            table.Cell().Text(book.BookName);
                            table.Cell().Text(book.Author);
                            table.Cell().Text(book.Category);
                            table.Cell().Text($"{book.Price} TL");
                            table.Cell().Text(book.Stock.ToString());
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("BookStore Razor Page ADO.NET Projesi");
                });
            }).GeneratePdf();

            return File(pdf, "application/pdf", "KitapRaporu.pdf");
        }
    }
}