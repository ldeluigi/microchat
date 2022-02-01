using EasyDesk.CleanArchitecture.Application.Messaging;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Application.ExternalEventHandlers.UserLifecycle;

/// <summary>
/// An external event published by the customer context whenever a new user is deleted to the system.
/// </summary>
/// <param name="AccountId">The Id of the user.</param>
public record AccountDeleted(Guid AccountId) : IMessage;

/// <summary>
/// Class that handles User Deletion.
/// </summary>
public class AccountDeletedHandler : IMessageHandler<AccountDeleted>
{
    private readonly IUserRepository _userRepository;

    public AccountDeletedHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /*
    protected override async Task<Response<Nothing>> Handle(AccountDeleted ev)
    {
        return await _userRepository.GetById(ev.Id)
            .ThenIfSuccess(_userRepository.Remove)
            .ThenToResponse();
    }
    */
    Task IHandleMessages<AccountDeleted>.Handle(AccountDeleted message) => throw new NotImplementedException();
}
