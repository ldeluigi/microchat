using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Application.Queries.Message;

public record PrivateChatMessageOutput(
    Guid Id,
    Timestamp SendTime,
    Option<Timestamp> LastEditTime,
    Guid SenderId,
    bool Viewed,
    MessageText Text);
