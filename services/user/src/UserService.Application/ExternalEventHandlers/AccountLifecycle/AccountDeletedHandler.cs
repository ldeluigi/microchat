using EasyDesk.CleanArchitecture.Application.Messaging;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Application.ExternalEventHandlers.AccountLifecycle;

public record AccountDeleted(Guid AccountId) : IMessage;

public class AccountDeletedHandler : IMessageHandler<AccountDeleted>
{
    private readonly IUserRepository _userRepository;

    public AccountDeletedHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(AccountDeleted message)
    {
        await _userRepository.RemoveById(message.AccountId);
    }
}
