using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.UserAggregate;
using EasyDesk.CleanArchitecture.Application.Messaging;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Messages.AccountLifecycle;

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
                message.AccountId));
        return Task.CompletedTask;
    }
}
