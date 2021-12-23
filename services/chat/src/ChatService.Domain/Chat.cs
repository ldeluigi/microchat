using System;
using System.Collections.Generic;
using EasyDesk.CleanArchitecture.Domain.Metamodel;

namespace Microchat.Domain.Aggregates.Chat
{
    /// <summary>
    /// The place where is possible to send message.
    /// </summary>
    public class Chat : AggregateRoot
    {
        /// <summary>
        /// Chat constructor.
        /// </summary>
        /// <param name="id">Identificator of the Chat.</param>
        /// <param name="partecipants">The entities that can send messages in this chat.</param>
        public Chat(Guid id, IEnumerable<Guid> partecipants)
        {
            Id = id;
            Partecipants = partecipants;
            CreationTime = DateTime.Now;
        }

        /// <summary>
        /// The unique identifier of this chat.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The list of user that partecipate to this chat.
        /// </summary>
        public IEnumerable<Guid> Partecipants { get; }

        /// <summary>
        /// The dateTime when this chat is created.
        /// </summary>
        public DateTime CreationTime { get; }
    }
}
