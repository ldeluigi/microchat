using System;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Domain.Aggregates.MessageAggregate;

public class PrivateMessage : AggregateRoot
{
    private PrivateMessage(
        Guid id,
        Guid chatId,
        MessageText text,
        Guid senderId,
        Timestamp sendTime,
        Option<Timestamp> lastEditTime,
        bool viewed)
    {
        Id = id;
        ChatId = chatId;
        Text = text;
        SenderId = senderId;
        SendTime = sendTime;
        LastEditTime = lastEditTime;
        Viewed = viewed;
    }

    public static PrivateMessage Create(
        Guid id,
        Guid chatId,
        MessageText text,
        Guid senderId,
        Timestamp sendTime)
    {
        return new(id, chatId, text, senderId, sendTime, None, false);
    }

    public Guid Id { get; }

    public Guid ChatId { get; }

    public MessageText Text { get; private set; }

    public Timestamp SendTime { get; }

    public Option<Timestamp> LastEditTime { get; private set; }

    public bool Viewed { get; private set; }

    public Guid SenderId { get; }

    public void EditText(MessageText newText)
    {
        LastEditTime = Some(Timestamp.Now);
        Text = newText;
    }

    public void SetViewed()
    {
        Viewed = true;
    }
}
