using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace UserService.Domain.Aggregates.UserService;

public record UserRegistrationData(Guid Id, Name Name, Name Surname, Email Email);

public class UserRegistrationMethod : IRegistrationMethod<UserRegistrationData>
{
    private readonly IUserRepository _userRepository;

    public UserRegistrationMethod(
        IUserRepository accountRepository)
    {
        _userRepository = accountRepository;
    }

    Task<Result<User>> IRegistrationMethod<UserRegistrationData>.CreateUser(UserRegistrationData userData)
    {
        var user = User.Create(
         userData.Id,
         userData.Name,
         userData.Surname,
         userData.Email);

        _userRepository.Save(user);

        return Task.FromResult(Success(user));
    }
}
