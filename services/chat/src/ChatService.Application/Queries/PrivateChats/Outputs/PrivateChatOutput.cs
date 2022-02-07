using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatService.Application.Queries.PrivateChats.Outputs;

public record PrivateChatOutput(
    Guid Id,
    Option<Guid> CreatorId,
    Option<Guid> PartecipantId,
    Timestamp CreationTimestamp)
{
    public IEnumerable<Guid> Members => Enumerable.Concat(CreatorId, PartecipantId);

    public static PrivateChatOutput From(PrivateChat privateChat) =>
        new(
            Id: privateChat.Id,
            CreatorId: privateChat.CreatorId,
            PartecipantId: privateChat.PartecipantId,
            CreationTimestamp: privateChat.CreationTime);
}
