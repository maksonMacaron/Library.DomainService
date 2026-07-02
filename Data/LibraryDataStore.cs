using System;
using System.Collections.Generic;
using Library.DomainService.Objects;

namespace Library.DomainService.Data
{
    public static class LibraryDataStore
    {
        public static List<UserDto> Users { get; } = new List<UserDto>
        {
            new UserDto
            {
                Id = 1,
                Login = "librarian",
                Password = "1234",
                FullName = "Иванова Анна Сергеевна",
                Role = "Библиотекарь"
            }
        };

        public static List<BookDto> Books { get; } = new List<BookDto>
        {
            new BookDto
            {
                Id = 1,
                Title = "Мастер и Маргарита",
                Author = "М. А. Булгаков",
                Isbn = "978-5-17-090183-5",
                PublicationYear = 2022,
                InventoryNumber = "КН-0001",
                Status = "Доступна"
            },
            new BookDto
            {
                Id = 2,
                Title = "Преступление и наказание",
                Author = "Ф. М. Достоевский",
                Isbn = "978-5-04-116672-1",
                PublicationYear = 2021,
                InventoryNumber = "КН-0002",
                Status = "Выдана"
            }
        };

        public static List<ReaderDto> Readers { get; } =
         new List<ReaderDto>
         {
            new ReaderDto
            {
                Id = 1,
                LibraryCardNumber = "ЧБ-0001",
                FullName = "Петров Иван Сергеевич",
                Phone = "+7 900 111-22-33",
                Email = "petrov@example.com",
                Status = "Активен"
            },
            new ReaderDto
            {
                Id = 2,
                LibraryCardNumber = "ЧБ-0002",
                FullName = "Сидорова Мария Андреевна",
                Phone = "+7 900 222-33-44",
                Email = "sidorova@example.com",
                Status = "Активен"
            }
         };

        public static List<LoanDto> Loans { get; } =
            new List<LoanDto>();

        public static int NextUserId { get; set; } = 2;

        public static int NextBookId { get; set; } = 3;

        public static int NextReaderId { get; set; } = 3;

        public static int NextLoanId { get; set; } = 1;
    }
}