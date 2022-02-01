using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Application.Queries
{
    public record MessageOutput(
        Guid Id,
        Guid ChatId,
        string Text,
        Guid Sender,
        Timestamp SendTime)
    {
        public static MessageOutput From(Message message) => new(
            Id: message.Id,
            ChatId: message.ChatId,
            Text: message.Text,
            Sender: message.Sender,
            SendTime: message.SendTime);
    }
}
