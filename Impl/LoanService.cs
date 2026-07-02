using System;
using System.Collections.Generic;
using System.Linq;
using Library.DomainService.Adds;
using Library.DomainService.Contracts;
using Library.DomainService.Data;
using Library.DomainService.Objects;

namespace Library.DomainService.Impl
{
    public class LoanService : ILoanService
    {
        private const decimal FinePerDay = 10m;

        public List<LoanDto> GetLoans()
        {
            Logger.Info("Getting all loans");
            return LibraryDataStore.Loans.ToList();
        }

        public List<LoanDto> GetActiveLoansByReaderId(int readerId)
        {
            Logger.Info($"Getting active loans for reader: {readerId}");

            return LibraryDataStore.Loans
                .Where(loan =>
                    loan.ReaderId == readerId &&
                    loan.Status == "Выдана")
                .ToList();
        }

        public ServiceResult IssueBook(int readerId, int bookId, int days)
        {
            Logger.Info($"Issuing book {bookId} to reader {readerId}");

            var reader = LibraryDataStore.Readers
                .FirstOrDefault(item => item.Id == readerId);

            if (reader == null)
                return ServiceResult.Error("Читатель не найден.");

            if (reader.Status != "Активен")
                return ServiceResult.Error("Читатель не активен.");

            var book = LibraryDataStore.Books
                .FirstOrDefault(item => item.Id == bookId);

            if (book == null)
                return ServiceResult.Error("Книга не найдена.");

            if (book.Status != "Доступна")
                return ServiceResult.Error("Книга недоступна для выдачи.");

            if (days <= 0)
                return ServiceResult.Error("Срок выдачи должен быть больше нуля.");

            var loan = new LoanDto
            {
                Id = LibraryDataStore.NextLoanId++,
                ReaderId = readerId,
                BookId = bookId,
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(days),
                ReturnDate = null,
                FineAmount = 0,
                Status = "Выдана"
            };

            LibraryDataStore.Loans.Add(loan);
            book.Status = "Выдана";

            Logger.Success($"Book issued. Loan id: {loan.Id}");

            return ServiceResult.Ok("Выдача книги успешно оформлена.");
        }

        public ServiceResult ReturnBook(int loanId)
        {
            Logger.Info($"Returning book by loan id: {loanId}");

            var loan = LibraryDataStore.Loans
                .FirstOrDefault(item => item.Id == loanId);

            if (loan == null)
                return ServiceResult.Error("Запись о выдаче не найдена.");

            if (loan.Status != "Выдана")
                return ServiceResult.Error("Данная выдача уже закрыта.");

            var book = LibraryDataStore.Books
                .FirstOrDefault(item => item.Id == loan.BookId);

            if (book == null)
                return ServiceResult.Error("Книга по записи выдачи не найдена.");

            loan.ReturnDate = DateTime.Now;
            loan.Status = "Возвращена";

            if (DateTime.Now.Date > loan.DueDate.Date)
            {
                int overdueDays =
                    (DateTime.Now.Date - loan.DueDate.Date).Days;

                loan.FineAmount = overdueDays * FinePerDay;
            }

            book.Status = "Доступна";

            Logger.Success($"Book returned. Loan id: {loan.Id}");

            if (loan.FineAmount > 0)
                return ServiceResult.Ok(
                    $"Книга возвращена. Начислен штраф: {loan.FineAmount} руб.");

            return ServiceResult.Ok("Книга возвращена без штрафа.");
        }
    }
}