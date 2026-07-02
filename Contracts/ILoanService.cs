using System.Collections.Generic;
using System.ServiceModel;
using Library.DomainService.Objects;

namespace Library.DomainService.Contracts
{
    [ServiceContract]
    public interface ILoanService
    {
        [OperationContract]
        List<LoanDto> GetLoans();

        [OperationContract]
        List<LoanDto> GetActiveLoansByReaderId(int readerId);

        [OperationContract]
        ServiceResult IssueBook(int readerId, int bookId, int days);

        [OperationContract]
        ServiceResult ReturnBook(int loanId);
    }
}