using System;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;

namespace ChatService.Application.Queries.PrivateChats.Outputs;

public record PrivateChatOfUserOutput(
    Guid Id,
    Option<Guid> CreatorId,
    Option<Guid> PartecipantId,
    Timestamp CreationTimestamp,
    Option<int> NumberOfUnreadMessages,
    Option<Timestamp> LastMessageTimestamp);
