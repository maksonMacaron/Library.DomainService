using System.Collections.Generic;
using System.ServiceModel;
using Library.DomainService.Objects;

namespace Library.DomainService.Contracts
{
    [ServiceContract]
    public interface IBookService
    {
        [OperationContract]
        List<BookDto> GetBooks();

        [OperationContract]
        BookDto GetBookById(int id);

        [OperationContract]
        List<BookDto> SearchBooks(string searchText);

        [OperationContract]
        ServiceResult AddBook(BookDto book);

        [OperationContract]
        ServiceResult UpdateBook(BookDto book);

        [OperationContract]
        ServiceResult DeleteBook(int id);
    }
}