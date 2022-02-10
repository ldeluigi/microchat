using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Application.Queries.PrivateMessages.Outputs;

public record PrivateChatMessageOutput(
    Guid Id,
    Guid ChatId,
    Timestamp SendTime,
    Option<Timestamp> LastEditTime,
    Option<Guid> SenderId,
    bool Viewed,
    string Text,
    PrivateChatOutput Chat)
{
    public static PrivateChatMessageOutput From(PrivateMessage privateMessage, PrivateChat chat) => new(
        Id: privateMessage.Id,
        ChatId: privateMessage.ChatId,
        SendTime: privateMessage.SendTime,
        LastEditTime: privateMessage.LastEditTime,
        SenderId: privateMessage.SenderId,
        Viewed: privateMessage.Viewed,
        Text: privateMessage.Text,
        Chat: PrivateChatOutput.From(chat));
}
