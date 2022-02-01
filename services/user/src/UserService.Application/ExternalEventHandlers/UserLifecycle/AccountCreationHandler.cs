using EasyDesk.CleanArchitecture.Application.Messaging;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Application.ExternalEventHandlers.UserLifecycle;

// TODO

/// <summary>
/// An external event published by the customer context whenever a new user is registered to the system.
/// </summary>
/// <param name="AccountId">The Id of the user.</param>
public record AccountCreated(Guid AccountId) : IMessage;

/// <summary>
/// Class that handles Account Registration.
/// </summary>
public class AccountCreationHandler : IMessageHandler<AccountCreated>
{
    private readonly IUserRepository _userRepository;

    public AccountCreationHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /*
    protected override IHandleMessages<UserCreated> Handle(UserCreated message)
    {
        var user = User.Create(message.Id);
        _userRepository.Save(user);
        return ;
    }*/

    Task IHandleMessages<AccountCreated>.Handle(AccountCreated message) => throw new NotImplementedException();
}
