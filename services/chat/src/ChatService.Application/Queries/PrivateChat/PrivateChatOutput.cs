﻿using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Application.Queries.PrivateChat;

public record PrivateChatOutput(
    Guid Id,
    Guid CreatorId,
    Guid PartecipantId,
    Timestamp CreationTimestamp,
    Option<int> NumberOfUnreadMessages);
