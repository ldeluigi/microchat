using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Application.Queries.PrivateChats.Outputs;

public record PrivateChatOutput(
    Guid Id,
    Option<Guid> CreatorId,
    Option<Guid> PartecipantId,
    Timestamp CreationTimestamp)
{
    public static PrivateChatOutput From(PrivateChat privateChat) =>
        new(
            Id: privateChat.Id,
            CreatorId: privateChat.CreatorId,
            PartecipantId: privateChat.PartecipantId,
            CreationTimestamp: privateChat.CreationTime);
}
