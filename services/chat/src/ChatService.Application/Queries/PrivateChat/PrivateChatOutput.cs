using System;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Application.Queries.PrivateChat;

public record PrivateChatOutput(
    Guid Id,
    Guid CreatorId,
    Guid PartecipantId,
    Timestamp CreationTimestamp,
    Option<int> NumberOfUnreadMessages)
{
    public static PrivateChatOutput From(Domain.Aggregates.PrivateChatAggregate.PrivateChat privateChat) => new(
        Id: privateChat.Id,
        CreatorId: privateChat.Creator,
        PartecipantId: privateChat.Partecipant,
        CreationTimestamp: privateChat.CreationTime,
        NumberOfUnreadMessages: None);
}
