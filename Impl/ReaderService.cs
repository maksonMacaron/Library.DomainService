using System;
using System.Collections.Generic;
using System.Linq;
using Library.DomainService.Adds;
using Library.DomainService.Contracts;
using Library.DomainService.Data;
using Library.DomainService.Objects;

namespace Library.DomainService.Impl
{
    public class ReaderService : IReaderService
    {
        public List<ReaderDto> GetReaders()
        {
            Logger.Info("Getting all readers");

            return LibraryDataStore.Readers.ToList();
        }

        public ReaderDto GetReaderById(int id)
        {
            Logger.Info($"Getting reader by id: {id}");

            return LibraryDataStore.Readers
                .FirstOrDefault(reader => reader.Id == id);
        }

        public List<ReaderDto> SearchReaders(string searchText)
        {
            Logger.Info($"Searching readers: {searchText}");

            if (string.IsNullOrWhiteSpace(searchText))
                return GetReaders();

            searchText = searchText.Trim();

            return LibraryDataStore.Readers
                .Where(reader =>
                    ContainsIgnoreCase(reader.FullName, searchText)
                    || ContainsIgnoreCase(reader.LibraryCardNumber, searchText)
                    || ContainsIgnoreCase(reader.Phone, searchText)
                    || ContainsIgnoreCase(reader.Email, searchText))
                .ToList();
        }

        public ServiceResult AddReader(ReaderDto reader)
        {
            Logger.Info("Adding reader");

            if (reader == null)
                return ServiceResult.Error("Данные читателя не переданы.");

            string validationError = ValidateReader(reader);

            if (!string.IsNullOrWhiteSpace(validationError))
                return ServiceResult.Error(validationError);

            bool cardNumberExists = LibraryDataStore.Readers.Any(item =>
                item.LibraryCardNumber.Equals(
                    reader.LibraryCardNumber,
                    StringComparison.OrdinalIgnoreCase));

            if (cardNumberExists)
                return ServiceResult.Error(
                    "Читатель с таким номером билета уже существует.");

            reader.Id = LibraryDataStore.NextReaderId++;

            LibraryDataStore.Readers.Add(reader);

            Logger.Success($"Reader added: {reader.FullName}");

            return ServiceResult.Ok("Читатель успешно зарегистрирован.");
        }

        public ServiceResult UpdateReader(ReaderDto reader)
        {
            Logger.Info($"Updating reader: {reader?.Id}");

            if (reader == null)
                return ServiceResult.Error("Данные читателя не переданы.");

            ReaderDto dbReader = LibraryDataStore.Readers
                .FirstOrDefault(item => item.Id == reader.Id);

            if (dbReader == null)
                return ServiceResult.Error("Читатель не найден.");

            string validationError = ValidateReader(reader);

            if (!string.IsNullOrWhiteSpace(validationError))
                return ServiceResult.Error(validationError);

            bool cardNumberExists = LibraryDataStore.Readers.Any(item =>
                item.Id != reader.Id
                && item.LibraryCardNumber.Equals(
                    reader.LibraryCardNumber,
                    StringComparison.OrdinalIgnoreCase));

            if (cardNumberExists)
                return ServiceResult.Error(
                    "Другой читатель уже имеет такой номер билета.");

            dbReader.LibraryCardNumber = reader.LibraryCardNumber;
            dbReader.FullName = reader.FullName;
            dbReader.Phone = reader.Phone;
            dbReader.Email = reader.Email;
            dbReader.Status = reader.Status;

            Logger.Success($"Reader updated: {dbReader.FullName}");

            return ServiceResult.Ok("Данные читателя успешно обновлены.");
        }

        public ServiceResult DeleteReader(int id)
        {
            Logger.Info($"Deleting reader: {id}");

            ReaderDto reader = LibraryDataStore.Readers
                .FirstOrDefault(item => item.Id == id);

            if (reader == null)
                return ServiceResult.Error("Читатель не найден.");

            bool hasActiveLoan = LibraryDataStore.Loans.Any(loan =>
                loan.ReaderId == id
                && loan.Status == "Выдана");

            if (hasActiveLoan)
                return ServiceResult.Error(
                    "Нельзя удалить читателя с активной выдачей.");

            LibraryDataStore.Readers.Remove(reader);

            Logger.Success($"Reader deleted: {reader.FullName}");

            return ServiceResult.Ok("Читатель успешно удалён.");
        }

        private static string ValidateReader(ReaderDto reader)
        {
            if (string.IsNullOrWhiteSpace(reader.LibraryCardNumber))
                return "Введите номер читательского билета.";

            if (string.IsNullOrWhiteSpace(reader.FullName))
                return "Введите ФИО читателя.";

            if (string.IsNullOrWhiteSpace(reader.Status))
                return "Укажите статус читателя.";

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