using EasyDesk.CleanArchitecture.Application.Messaging;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.Tools;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.ExternalEventHandlers.UserLifecycle;

/// <summary>
/// An external event published by the customer context whenever a new user is deleted to the system.
/// </summary>
/// <param name="AccountId">The Id of the user.</param>
public record AccountDeleted(Guid AccountId) : IMessage;

/// <summary>
/// Class that handles User Deletion.
/// </summary>
public class UserDeletionHandler : IMessageHandler<AccountDeleted>
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
    Task IHandleMessages<AccountDeleted>.Handle(AccountDeleted message) => throw new NotImplementedException();
}
