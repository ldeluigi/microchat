using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;
using System.Linq;

namespace ChatService.Application.Queries.PrivateMessages.Outputs;

public record PrivateChatMessageOutput(
    Guid Id,
    Guid ChatId,
    Timestamp SendTime,
    Option<Timestamp> LastEditTime,
    Option<Guid> SenderId,
    bool Viewed,
    MessageText Text)
{
    public static PrivateChatMessageOutput From(PrivateMessage privateMessage, Guid asSeenBy) => new(
        Id: privateMessage.Id,
        ChatId: privateMessage.ChatId,
        SendTime: privateMessage.SendTime,
        LastEditTime: privateMessage.LastEditTime,
        SenderId: privateMessage.SenderId,
        Viewed: privateMessage.SenderId.Contains(asSeenBy) ? true : privateMessage.Viewed,
        Text: privateMessage.Text);
}
