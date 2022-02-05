using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.UserAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools;
using System;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace ChatService.Domain;

public record UserDeleted(Guid Id) : DomainEvent;

public class UserLifecycleService
{
    private readonly IUserRepository _userRepository;
    private readonly IDomainEventNotifier _domainEventNotifier;

    public UserLifecycleService(
        IUserRepository userRepository,
        IDomainEventNotifier domainEventNotifier)
    {
        _userRepository = userRepository;
        _domainEventNotifier = domainEventNotifier;
    }

    public async Task<Result<Nothing>> DeleteUser(Guid userId)
    {
        return await _userRepository
            .RemoveById(userId)
            .ThenIfSuccess(u =>
                _domainEventNotifier.Notify(new UserDeleted(u.Id)));
    }

    public Task<Result<User>> CreateUser(Guid accountId)
    {
        var user = User.Create(accountId);
        _userRepository.Save(user);
        return Task.FromResult(Success(user));
    }
}
