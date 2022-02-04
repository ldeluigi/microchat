using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Application.Queries.PrivateChat;

public record DetailedPrivateChatOutput(
    Guid Id,
    Guid CreatorId,
    Guid PartecipantId,
    Timestamp CreationTimestamp,
    int NumberOfMessages,
    Option<int> NumberOfUnreadMessages);
