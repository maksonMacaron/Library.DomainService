using System.Collections.Generic;
using System.ServiceModel;
using Library.DomainService.Objects;

namespace Library.DomainService.Contracts
{
    [ServiceContract]
    public interface IReaderService
    {
        [OperationContract]
        List<ReaderDto> GetReaders();

        [OperationContract]
        ReaderDto GetReaderById(int id);

        [OperationContract]
        List<ReaderDto> SearchReaders(string searchText);

        [OperationContract]
        ServiceResult AddReader(ReaderDto reader);

        [OperationContract]
        ServiceResult UpdateReader(ReaderDto reader);

        [OperationContract]
        ServiceResult DeleteReader(int id);
    }
}