using EasyDesk.CleanArchitecture.Application.Messaging;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Application.ExternalEventHandlers.AccountLifecycle;

public record AccountCreated(Guid AccountId, string Username) : IMessage;

public class AccountCreationHandler : IMessageHandler<AccountCreated>
{
    private readonly IUserRepository _userRepository;

    public AccountCreationHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task Handle(AccountCreated message)
    {
        _userRepository.Save(
            User.Create(
                message.AccountId,
                Username.From(message.Username)));
        return Task.CompletedTask;
    }
}
