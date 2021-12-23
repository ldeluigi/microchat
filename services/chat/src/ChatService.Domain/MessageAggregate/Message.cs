using System;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.Tools.Options;
using static EasyDesk.Tools.Options.OptionImports;

namespace Microchat.ChatService.Domain.Aggregates.MessageAggregate
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
        /// <param name="text">The text pf this message.</param>
        /// <param name="sender">The sender of this message.</param>
        public Message(Guid id, string text, Guid sender)
        {
            Id = id;
            Text = text;
            SendTime = DateTime.Now;
            LastEditTime = None;
            Sender = sender;
        }

        /// <summary>
        /// The unique identifier of this message.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The text of this message.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// The time when the message have been sent for the first time.
        /// </summary>
        public DateTime SendTime { get; }

        /// <summary>
        /// Last time when the message have been modified.
        /// </summary>
        public Option<DateTime> LastEditTime { get; private set; }

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
            LastEditTime = Some(DateTime.Now);
            Text = newText;
        }
    }
}
