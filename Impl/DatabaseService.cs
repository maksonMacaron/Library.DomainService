using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library.DomainService.Contracts;
using Library.DomainService.DbContext;
using Library.DomainService.Objects;

namespace Library.DomainService.Impl
{
    public class DatabaseService : IDatabaseService
    {
        public List<BookDto> GetBooks()
        {
            var books = new List<BookDto>();

            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();

                string sql = @"
                    SELECT 
                        bc.copy_id,
                        b.title,
                        b.isbn,
                        b.publication_year,
                        bc.inventory_number,
                        bc.status
                    FROM book_copies bc
                    INNER JOIN books b ON b.book_id = bc.book_id";

                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new BookDto
                        {
                            Id = reader.GetInt32("copy_id"),
                            Title = reader.GetString("title"),
                            Isbn = reader["isbn"]?.ToString(),
                            PublicationYear = reader.GetInt32("publication_year"),
                            InventoryNumber = reader.GetString("inventory_number"),
                            Status = reader.GetString("status"),
                            Author = ""
                        });
                    }
                }
            }

            return books;
        }

        public BookDto GetBookById(int id)
        {
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();

                string sql = @"
                    SELECT 
                        bc.copy_id,
                        b.title,
                        b.isbn,
                        b.publication_year,
                        bc.inventory_number,
                        bc.status
                    FROM book_copies bc
                    INNER JOIN books b ON b.book_id = bc.book_id
                    WHERE bc.copy_id = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return new BookDto
                        {
                            Id = reader.GetInt32("copy_id"),
                            Title = reader.GetString("title"),
                            Isbn = reader["isbn"]?.ToString(),
                            PublicationYear = reader.GetInt32("publication_year"),
                            InventoryNumber = reader.GetString("inventory_number"),
                            Status = reader.GetString("status"),
                            Author = ""
                        };
                    }
                }
            }
        }

        public ServiceResult AddBook(BookDto book)
        {
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    string insertBookSql = @"
                        INSERT INTO books 
                        (title, isbn, publication_year, publisher, genre)
                        VALUES
                        (@title, @isbn, @year, @publisher, @genre);
                        SELECT LAST_INSERT_ID();";

                    int bookId;

                    using (var command = new MySqlCommand(insertBookSql, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@title", book.Title);
                        command.Parameters.AddWithValue("@isbn", book.Isbn);
                        command.Parameters.AddWithValue("@year", book.PublicationYear);
                        command.Parameters.AddWithValue("@publisher", "");
                        command.Parameters.AddWithValue("@genre", "");

                        bookId = System.Convert.ToInt32(command.ExecuteScalar());
                    }

                    string insertCopySql = @"
                        INSERT INTO book_copies
                        (book_id, inventory_number, status, condition_state, receipt_date)
                        VALUES
                        (@bookId, @inventory, @status, @condition, CURDATE())";

                    using (var command = new MySqlCommand(insertCopySql, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@bookId", bookId);
                        command.Parameters.AddWithValue("@inventory", book.InventoryNumber);
                        command.Parameters.AddWithValue("@status", book.Status);
                        command.Parameters.AddWithValue("@condition", "Хорошее");

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }

            return ServiceResult.Ok("Книга успешно добавлена.");
        }

        public ServiceResult DeleteBook(int id)
        {
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();

                string sql = "DELETE FROM book_copies WHERE copy_id = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows == 0)
                        return ServiceResult.Error("Книга не найдена.");
                }
            }

            return ServiceResult.Ok("Книга успешно удалена.");
        }
    }
}