using BookStoreRazorDbFirst.Models;
using System.Data.SqlClient;

namespace BookStoreRazorDbFirst.Services
{
    public class BookService
    {
        private readonly string _connectionString;

        public BookService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Book> GetAll(string search = "")
        {
            List<Book> books = new List<Book>();

            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = @"SELECT * FROM Books 
                             WHERE BookName LIKE @search 
                             OR Author LIKE @search 
                             OR Category LIKE @search
                             ORDER BY Id DESC";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@search", "%" + search + "%");

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                books.Add(new Book
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    BookName = reader["BookName"].ToString(),
                    Author = reader["Author"].ToString(),
                    Category = reader["Category"].ToString(),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Stock = Convert.ToInt32(reader["Stock"]),
                    ImageUrl = reader["ImageUrl"].ToString(),
                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                });
            }

            return books;
        }

        public Book GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT * FROM Books WHERE Id=@id";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Book
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    BookName = reader["BookName"].ToString(),
                    Author = reader["Author"].ToString(),
                    Category = reader["Category"].ToString(),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Stock = Convert.ToInt32(reader["Stock"]),
                    ImageUrl = reader["ImageUrl"].ToString(),
                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                };
            }

            return null;
        }
        public bool Login(string username, string password)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            SqlCommand command = new SqlCommand(
                "select count(*) from Admins where UserName=@u and Password=@p",
                connection);

            command.Parameters.AddWithValue("@u", username);
            command.Parameters.AddWithValue("@p", password);

            int result = (int)command.ExecuteScalar();

            return result > 0;
        }
        public void Add(Book book)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = @"INSERT INTO Books(BookName,Author,Category,Price,Stock,ImageUrl)
                             VALUES(@BookName,@Author,@Category,@Price,@Stock,@ImageUrl)";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BookName", book.BookName);
            command.Parameters.AddWithValue("@Author", book.Author);
            command.Parameters.AddWithValue("@Category", book.Category);
            command.Parameters.AddWithValue("@Price", book.Price);
            command.Parameters.AddWithValue("@Stock", book.Stock);
            command.Parameters.AddWithValue("@ImageUrl", book.ImageUrl ?? "");

            command.ExecuteNonQuery();
        }

        public void Update(Book book)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = @"UPDATE Books SET 
                             BookName=@BookName,
                             Author=@Author,
                             Category=@Category,
                             Price=@Price,
                             Stock=@Stock,
                             ImageUrl=@ImageUrl
                             WHERE Id=@Id";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", book.Id);
            command.Parameters.AddWithValue("@BookName", book.BookName);
            command.Parameters.AddWithValue("@Author", book.Author);
            command.Parameters.AddWithValue("@Category", book.Category);
            command.Parameters.AddWithValue("@Price", book.Price);
            command.Parameters.AddWithValue("@Stock", book.Stock);
            command.Parameters.AddWithValue("@ImageUrl", book.ImageUrl ?? "");

            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = "DELETE FROM Books WHERE Id=@id";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }

        public int TotalBookCount()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            using SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Books", connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }


        public int TotalStock()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            using SqlCommand command = new SqlCommand("SELECT ISNULL(SUM(Stock),0) FROM Books", connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public decimal TotalValue()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            using SqlCommand command = new SqlCommand("SELECT ISNULL(SUM(Price * Stock),0) FROM Books", connection);
            return Convert.ToDecimal(command.ExecuteScalar());
        }
    }
}