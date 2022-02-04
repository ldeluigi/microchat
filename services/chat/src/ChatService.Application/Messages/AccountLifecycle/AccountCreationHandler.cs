using ChatService.Domain;
using EasyDesk.CleanArchitecture.Application.Messaging;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Messages.AccountLifecycle;

public record AccountCreated(Guid AccountId, string Username) : IMessage;

public class AccountCreationHandler : IMessageHandler<AccountCreated>
{
    private readonly UserLifecycleService _userLifecycleService;

    public AccountCreationHandler(UserLifecycleService userLifecycleService)
    {
        _userLifecycleService = userLifecycleService;
    }

    public async Task Handle(AccountCreated message)
    {
        await _userLifecycleService.CreateUser(message.AccountId);
    }
}
