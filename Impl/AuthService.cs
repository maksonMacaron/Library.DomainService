using System;
using System.Linq;
using Library.DomainService.Contracts;
using Library.DomainService.Data;
using Library.DomainService.Objects;

namespace Library.DomainService.Impl
{
    public class AuthService : IAuthService
    {
        public UserDto Login(string login, string password)
        {
            return LibraryDataStore.Users.FirstOrDefault(user =>
                user.Login.Equals(login,
                    StringComparison.OrdinalIgnoreCase)
                && user.Password == password);
        }

        public bool UserExists(string login)
        {
            return LibraryDataStore.Users.Any(user =>
                user.Login.Equals(login,
                    StringComparison.OrdinalIgnoreCase));
        }

        public ServiceResult Register(UserDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Login))
                return ServiceResult.Error("Не указан логин.");

            if (string.IsNullOrWhiteSpace(user.Password))
                return ServiceResult.Error("Не указан пароль.");

            if (UserExists(user.Login))
                return ServiceResult.Error(
                    "Пользователь уже существует.");

            user.Id = LibraryDataStore.NextUserId++;

            LibraryDataStore.Users.Add(user);

            return ServiceResult.Ok(
                "Пользователь успешно зарегистрирован.");
        }
    }
}