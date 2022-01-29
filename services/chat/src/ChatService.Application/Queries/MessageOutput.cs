using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Application.Queries
{
    // TODO Option<Timestamp> LastEditTime va gestito diversamente
    public record MessageOutput(
        Guid Id,
        Guid ChatId,
        string Text,
        Guid Sender,
        Timestamp SendTime)
    {
        public MessageOutput From(MessageOutput messageOutput) => new(
            Id: messageOutput.Id,
            ChatId: messageOutput.ChatId,
            Text: messageOutput.Text,
            Sender: messageOutput.Sender,
            SendTime: messageOutput.SendTime);
    }
}
