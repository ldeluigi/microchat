using EasyDesk.CleanArchitecture.Application.Events.ExternalEvents;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.Tools;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Application.ExternalEventHandlers.AccountLifecycle;

/// <summary>
/// An external event published by the customer context whenever a new user is registered to the system.
/// </summary>
/// <param name="Id">The Id of the user.</param>
public record AccountCreated(Guid Id) : ExternalEvent;

/// <summary>
/// Class that handles Account Registration.
/// </summary>
public class AccountCreationHandler : ExternalEventHandlerBase<AccountCreated>
{
    private readonly IUserRepository _userRepository;

    public AccountCreationHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override Task<Response<Nothing>> Handle(AccountCreated ev)
    {
        var user = User.Create(ev.Id);
        _userRepository.Save(user);
        return OkAsync;
    }
}
