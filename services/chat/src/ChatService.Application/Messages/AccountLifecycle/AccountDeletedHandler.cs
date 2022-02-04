using ChatService.Domain;
using EasyDesk.CleanArchitecture.Application.Messaging;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Messages.AccountLifecycle;

public record AccountDeleted(Guid AccountId) : IMessage;

public class AccountDeletedHandler : IMessageHandler<AccountDeleted>
{
    private readonly UserLifecycleService _userLifecycleService;

    public AccountDeletedHandler(UserLifecycleService userLifecycleService)
    {
        _userLifecycleService = userLifecycleService;
    }

    public async Task Handle(AccountDeleted message)
    {
        await _userLifecycleService.DeleteUser(message.AccountId);
    }
}
