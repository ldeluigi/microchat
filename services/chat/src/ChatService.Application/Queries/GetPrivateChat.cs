using EasyDesk.CleanArchitecture.Application.Mediator;
using System;

namespace ChatService.Application.Queries;

public static class GetPrivateChat
{
    /// <summary>
    /// Find a Chat by his identifier.
    /// </summary>
    /// <param name="ChatId">The chat's identifier.</param>
    public record Query(Guid ChatId) : QueryBase<PrivateChatOutput>;
}
