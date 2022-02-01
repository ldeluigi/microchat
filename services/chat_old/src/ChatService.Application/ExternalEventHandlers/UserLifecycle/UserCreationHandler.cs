using EasyDesk.CleanArchitecture.Application.Events.ExternalEvents;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.Tools;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.ExternalEventHandlers.UserLifecycle;

// TODO

/// <summary>
/// An external event published by the customer context whenever a new user is registered to the system.
/// </summary>
/// <param name="Id">The Id of the user.</param>
public record UserCreated(Guid Id) : ExternalEvent;

/// <summary>
/// Class that handles Account Registration.
/// </summary>
public class UserCreationHandler : ExternalEventHandlerBase<UserCreated>
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
    protected override Task<Response<Nothing>> Handle(UserCreated ev) => throw new NotImplementedException();
}
