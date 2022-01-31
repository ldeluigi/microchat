using System;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Domain.Aggregates.MessageAggregate
{
    /// <summary>
    /// The message sent in the chat.
    /// </summary>
    public class Message : AggregateRoot
    {
        /// <summary>
        /// Creates a new <see cref="Message"/>.
        /// </summary>
        /// <param name="id">The unique identifier of this.</param>
        /// <param name="chatId">The unique identifier of the message's chat.</param>
        /// <param name="text">The text of this message.</param>
        /// <param name="sender">The sender of this message.</param>
        /// <param name="sendTime">The time when this message is sent.</param>
        /// <param name="lastEditTime">The last time when this message is modified.</param>
        public Message(Guid id, Guid chatId, string text, Guid sender, Timestamp sendTime, Option<Timestamp> lastEditTime)
        {
            Id = id;
            ChatId = chatId;
            Text = text;
            Sender = sender;
            SendTime = sendTime;
            LastEditTime = lastEditTime;
        }

        /// <summary>
        /// Create a new message.
        /// </summary>
        /// <param name="id">The unique identifier of the message.</param>
        /// <param name="chatId">The unique identifier of the message's chat.</param>
        /// <param name="text">The text of this message.</param>
        /// <param name="sender">The sender of this message.</param>
        /// <param name="sendTime">The time when this message is sent.</param>
        /// <returns>A new message.</returns>
        public static Message Create(Guid id, Guid chatId, string text, Guid sender, Timestamp sendTime)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (sendTime is null)
            {
                throw new ArgumentNullException(nameof(sendTime));
            }
            return new(id, chatId, text, sender, sendTime, None);
        }

        /// <summary>
        /// The unique identifier of this message.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The unique identifier of this message's chat.
        /// </summary>
        public Guid ChatId { get; }

        /// <summary>
        /// The text of this message.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// The time when the message have been sent for the first time.
        /// </summary>
        public Timestamp SendTime { get; }

        /// <summary>
        /// Last time when the message have been modified.
        /// </summary>
        public Option<Timestamp> LastEditTime { get; private set; }

        /// <summary>
        /// The sender of the message.
        /// </summary>
        public Guid Sender { get; }

        /// <summary>
        /// Change the text of this message.
        /// </summary>
        /// <param name="newText">The new content of this message.</param>
        public void EditText(string newText)
        {
            LastEditTime = Some(Timestamp.Now);
            Text = newText;
        }
    }

    /// <summary>
    /// The Event created when message is created.
    /// </summary>
    /// <param name="Message">The new message.</param>
    public record MessageCreatedEvent(Message Message) : DomainEvent;

    /// <summary>
    /// The Event created when message is modified.
    /// </summary>
    /// <param name="Message">The modified message.</param>
    public record MessageModifiedEvent(Message Message) : DomainEvent;
}
