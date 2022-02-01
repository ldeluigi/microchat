using EasyDesk.CleanArchitecture.Application.Messaging;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.ExternalEventHandlers.UserLifecycle;

// TODO

/// <summary>
/// An external event published by the customer context whenever a new user is registered to the system.
/// </summary>
/// <param name="AccountId">The Id of the user.</param>
public record AccountCreated(Guid AccountId) : IMessage;

/// <summary>
/// Class that handles Account Registration.
/// </summary>
public class UserCreationHandler : IMessageHandler<AccountCreated>
{
    /*private readonly IUserRepository _userRepository;

    public UserCreationHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override Task<Response<Nothing>> Handle(UserCreated ev)
    {
        var user = User.Create(ev.Id);
        _userRepository.Save(user);
        return OkAsync;
    }*/
    Task IHandleMessages<AccountCreated>.Handle(AccountCreated message) => throw new NotImplementedException();
}
