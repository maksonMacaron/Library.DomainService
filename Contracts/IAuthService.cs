using System.ServiceModel;
using Library.DomainService.Objects;

namespace Library.DomainService.Contracts
{
    [ServiceContract]
    public interface IAuthService
    {
        [OperationContract]
        UserDto Login(string login, string password);

        [OperationContract]
        ServiceResult Register(UserDto user);

        [OperationContract]
        bool UserExists(string login);
    }
}