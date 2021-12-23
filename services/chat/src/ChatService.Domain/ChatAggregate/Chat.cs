using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;
using System.Collections.Generic;

namespace Microchat.ChatService.Domain.ChatAggregate
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
        /// <param name="creationTime">The time when this chat is created.</param>
        public Chat(Guid id, IEnumerable<Guid> partecipants, Timestamp creationTime)
        {
            Id = id;
            Partecipants = partecipants;
            CreationTime = creationTime;
        }

        /// <summary>
        /// Initialize a new Chat.
        /// </summary>
        /// <param name="partecipants">The partecipants of this chat.</param>
        /// <param name="creationTime">The time when chat is created.</param>
        /// <returns>The chat.</returns>
        public static Chat Create(IEnumerable<Guid> partecipants, Timestamp creationTime)
        {
            if (partecipants is null)
            {
                throw new ArgumentNullException(nameof(partecipants));
            }

            if (creationTime is null)
            {
                throw new ArgumentNullException(nameof(creationTime));
            }

            return new(Guid.NewGuid(), partecipants, creationTime);
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
        public Timestamp CreationTime { get; }
    }
}
