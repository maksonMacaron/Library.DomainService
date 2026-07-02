using System;
using System.Collections.Generic;
using System.Linq;
using Library.DomainService.Adds;
using Library.DomainService.Contracts;
using Library.DomainService.Data;
using Library.DomainService.Objects;

namespace Library.DomainService.Impl
{
    public class BookService : IBookService
    {
        public List<BookDto> GetBooks()
        {
            Logger.Info("Getting all books");

            return LibraryDataStore.Books.ToList();
        }

        public BookDto GetBookById(int id)
        {
            Logger.Info($"Getting book by id: {id}");

            return LibraryDataStore.Books
                .FirstOrDefault(book => book.Id == id);
        }

        public List<BookDto> SearchBooks(string searchText)
        {
            Logger.Info($"Searching books: {searchText}");

            if (string.IsNullOrWhiteSpace(searchText))
                return GetBooks();

            searchText = searchText.Trim();

            return LibraryDataStore.Books
                .Where(book =>
                    ContainsIgnoreCase(book.Title, searchText)
                    || ContainsIgnoreCase(book.Author, searchText)
                    || ContainsIgnoreCase(book.Isbn, searchText)
                    || ContainsIgnoreCase(book.InventoryNumber, searchText))
                .ToList();
        }

        public ServiceResult AddBook(BookDto book)
        {
            Logger.Info("Adding book");

            if (book == null)
                return ServiceResult.Error("Данные книги не переданы.");

            string validationError = ValidateBook(book);

            if (!string.IsNullOrWhiteSpace(validationError))
                return ServiceResult.Error(validationError);

            bool inventoryNumberExists = LibraryDataStore.Books.Any(item =>
                item.InventoryNumber.Equals(
                    book.InventoryNumber,
                    StringComparison.OrdinalIgnoreCase));

            if (inventoryNumberExists)
                return ServiceResult.Error(
                    "Книга с таким инвентарным номером уже существует.");

            book.Id = LibraryDataStore.NextBookId++;

            LibraryDataStore.Books.Add(book);

            Logger.Success($"Book added: {book.Title}");

            return ServiceResult.Ok("Книга успешно добавлена.");
        }

        public ServiceResult UpdateBook(BookDto book)
        {
            Logger.Info($"Updating book: {book?.Id}");

            if (book == null)
                return ServiceResult.Error("Данные книги не переданы.");

            BookDto dbBook = LibraryDataStore.Books
                .FirstOrDefault(item => item.Id == book.Id);

            if (dbBook == null)
                return ServiceResult.Error("Книга не найдена.");

            string validationError = ValidateBook(book);

            if (!string.IsNullOrWhiteSpace(validationError))
                return ServiceResult.Error(validationError);

            bool inventoryNumberExists = LibraryDataStore.Books.Any(item =>
                item.Id != book.Id
                && item.InventoryNumber.Equals(
                    book.InventoryNumber,
                    StringComparison.OrdinalIgnoreCase));

            if (inventoryNumberExists)
                return ServiceResult.Error(
                    "Другой экземпляр уже имеет такой инвентарный номер.");

            dbBook.Title = book.Title;
            dbBook.Author = book.Author;
            dbBook.Isbn = book.Isbn;
            dbBook.PublicationYear = book.PublicationYear;
            dbBook.InventoryNumber = book.InventoryNumber;
            dbBook.Status = book.Status;

            Logger.Success($"Book updated: {dbBook.Title}");

            return ServiceResult.Ok("Данные книги успешно обновлены.");
        }

        public ServiceResult DeleteBook(int id)
        {
            Logger.Info($"Deleting book: {id}");

            BookDto book = LibraryDataStore.Books
                .FirstOrDefault(item => item.Id == id);

            if (book == null)
                return ServiceResult.Error("Книга не найдена.");

            bool hasActiveLoan = LibraryDataStore.Loans.Any(loan =>
                loan.BookId == id
                && loan.Status == "Выдана");

            if (hasActiveLoan)
                return ServiceResult.Error(
                    "Нельзя удалить книгу, которая сейчас выдана читателю.");

            LibraryDataStore.Books.Remove(book);

            Logger.Success($"Book deleted: {book.Title}");

            return ServiceResult.Ok("Книга успешно удалена.");
        }

        private static string ValidateBook(BookDto book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                return "Введите название книги.";

            if (string.IsNullOrWhiteSpace(book.Author))
                return "Введите автора книги.";

            if (book.PublicationYear < 1000 ||
                book.PublicationYear > DateTime.Now.Year)
                return "Укажите корректный год издания.";

            if (string.IsNullOrWhiteSpace(book.InventoryNumber))
                return "Введите инвентарный номер.";

            if (string.IsNullOrWhiteSpace(book.Status))
                return "Укажите статус книги.";

            return string.Empty;
        }

        private static bool ContainsIgnoreCase(
            string source,
            string value)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            return source.IndexOf(
                value,
                StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}