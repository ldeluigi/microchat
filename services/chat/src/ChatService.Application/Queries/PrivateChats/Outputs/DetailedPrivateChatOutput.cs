using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Application.Queries.PrivateChats.Outputs;

public record DetailedPrivateChatOutput(
    Guid Id,
    Option<Guid> CreatorId,
    Option<Guid> PartecipantId,
    Timestamp CreationTimestamp,
    int NumberOfMessages) : PrivateChatOutput(
        Id,
        CreatorId,
        PartecipantId,
        CreationTimestamp);
