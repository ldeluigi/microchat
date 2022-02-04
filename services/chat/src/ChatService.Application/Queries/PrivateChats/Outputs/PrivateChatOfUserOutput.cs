using System;
using System.Collections.Generic;
using System.Linq;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Application.Queries.PrivateChats.Outputs;

public record PrivateChatOfUserOutput(
    Guid Id,
    IEnumerable<Guid> OtherParticipantIds,
    Timestamp CreationTimestamp,
    Option<int> NumberOfUnreadMessages)
{
    public static PrivateChatOfUserOutput From(PrivateChat privateChat, Guid asSeenBy) => new(
        Id: privateChat.Id,
        OtherParticipantIds: privateChat.PartecipantId.Union(privateChat.CreatorId).Where(g => g != asSeenBy),
        CreationTimestamp: privateChat.CreationTime,
        NumberOfUnreadMessages: None);
}
