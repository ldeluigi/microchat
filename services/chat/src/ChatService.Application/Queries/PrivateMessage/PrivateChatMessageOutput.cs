using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Application.Queries.PrivateMessage;

public record PrivateChatMessageOutput(
    Guid Id,
    Guid ChatId,
    Timestamp SendTime,
    Option<Timestamp> LastEditTime,
    Guid SenderId,
    bool Viewed,
    MessageText Text)
{
    public static PrivateChatMessageOutput From(Domain.Aggregates.MessageAggregate.PrivateMessage privateMessage) => new(
        Id: privateMessage.Id,
        ChatId: privateMessage.ChatId,
        SendTime: privateMessage.SendTime,
        LastEditTime: privateMessage.LastEditTime,
        SenderId: privateMessage.SenderId,
        Viewed: privateMessage.Viewed,
        Text: privateMessage.Text);
}
