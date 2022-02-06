using System;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Application.Queries.PrivateChats.Outputs;

public record PrivateChatOfUserOutput(
    Guid Id,
    Option<Guid> CreatorId,
    Option<Guid> PartecipantId,
    Timestamp CreationTimestamp,
    Option<int> NumberOfUnreadMessages)
{
    public static PrivateChatOfUserOutput From(PrivateChat privateChat, Guid asSeenBy) => new(
        Id: privateChat.Id,
        CreatorId: privateChat.CreatorId,
        PartecipantId: privateChat.PartecipantId,
        CreationTimestamp: privateChat.CreationTime,
        NumberOfUnreadMessages: None);
}
