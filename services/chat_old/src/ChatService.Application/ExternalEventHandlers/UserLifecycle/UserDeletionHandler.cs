using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Events.ExternalEvents;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.ExternalEventHandlers.UserLifecycle;

/// <summary>
/// An external event published by the customer context whenever a new user is deleted to the system.
/// </summary>
/// <param name="Id">The Id of the user.</param>
public record UserDeleted(Guid Id) : ExternalEvent;

/// <summary>
/// Class that handles User Deletion.
/// </summary>
public class UserDeletionHandler : ExternalEventHandlerBase<UserDeleted>
{
    /*private readonly IUserRepository _userRepository;

    public UserDeletionHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task<Response<Nothing>> Handle(UserDeleted ev)
    {
        return await _userRepository.GetById(ev.Id)
            .ThenIfSuccess(_userRepository.Remove)
            .ThenToResponse();
    }*/
    protected override Task<Response<Nothing>> Handle(UserDeleted ev) => throw new NotImplementedException();
}
