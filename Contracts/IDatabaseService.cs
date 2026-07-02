using System.Collections.Generic;
using System.ServiceModel;
using Library.DomainService.Objects;

namespace Library.DomainService.Contracts
{
    [ServiceContract]
    public interface IDatabaseService
    {
        [OperationContract]
        List<BookDto> GetBooks();

        [OperationContract]
        ServiceResult AddBook(BookDto book);

        [OperationContract]
        BookDto GetBookById(int id);

        [OperationContract]
        ServiceResult DeleteBook(int id);
    }
}