using System;
using ChatService.Domain.Aggregates.ChatAggregate;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;

namespace ChatService.Application.Queries;

public record PrivateChatOutput(
    Guid Id,
    Guid Owner,
    Guid Partecipant,
    Timestamp CreationTime)
{
    public static PrivateChatOutput From(PrivateChat chat) => new(
        Id: chat.Id,
        Owner: chat.Owner,
        Partecipant: chat.Partecipant,
        CreationTime: chat.CreationTime);
}
