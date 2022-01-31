using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Infrastructure.DataAccess.Model.MessageAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.ModelConverter;

public class MessageConverter
{
    public Message ToDomain(MessageModel model) => new(
        id: model.Id,
        chatId: model.ChatId,
        text: model.Text,
        sender: model.Sender,
        sendTime: model.SendTime,
        lastEditTime: model.LastEditTime);

    public void ApplyChanges(Message origin, MessageModel destination)
    {
        destination.Id = origin.Id;
        destination.ChatId = origin.ChatId;
        destination.Text = origin.Text;
        destination.Sender = origin.Sender;
        destination.SendTime = origin.SendTime;
        destination.LastEditTime = origin.LastEditTime;
    }
}
