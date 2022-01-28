using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Events.ExternalEvents;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Application.ExternalEventHandlers.AccountLifecycle;

/// <summary>
/// An external event published by the customer context whenever a new user is deleted to the system.
/// </summary>
/// <param name="Id">The Id of the user.</param>
public record AccountDeleted(Guid Id) : ExternalEvent;

/// <summary>
/// Class that handles Account Deletion.
/// </summary>
public class AccountDeletionHandler : ExternalEventHandlerBase<AccountDeleted>
{
    private readonly IUserRepository _userRepository;

    public AccountDeletionHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task<Response<Nothing>> Handle(AccountDeleted ev)
    {
        return await _userRepository.GetById(ev.Id)
            .ThenIfSuccess(_userRepository.Remove)
            .ThenToResponse();
    }
}
