using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools.Options;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Domain.Aggregates.UserService;

public class UserLifecycleService
{
    private readonly IUserRepository _userRepository;
    private readonly IRegistrationMethod<UserRegistrationData> _registrationMethod;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserLifecycleService"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="userRegistrationMethod">The user registration method.</param>
    public UserLifecycleService(IUserRepository userRepository, UserRegistrationMethod userRegistrationMethod)
    {
        _userRepository = userRepository;
        _registrationMethod = userRegistrationMethod;
    }

    public async Task<Result<User>> Register(Guid id, Name name, Name surname, Email email)
    {
        return await Task.FromResult(ResultImports.Ok)
            .ThenFlatMapAsync(_ => _registrationMethod.CreateUser(new UserRegistrationData(id, name, surname, email))
            .ThenIfSuccess(user => _userRepository.Save(user)));
    }

    public async Task<Result<User>> Unregister(Guid userId)
    {
        return await _userRepository
            .GetById(userId)
            .ThenIfSuccess(user => _userRepository.Remove(user));
    }
}
