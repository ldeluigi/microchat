using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Domain.Aggregates.ChatAggregate;

/// <summary>
/// The place where is possible to send message.
/// </summary>
public class PrivateChat : AggregateRoot, IChat
{
    /// <summary>
    /// Chat constructor.
    /// </summary>
    /// <param name="id">Identificator of the Chat.</param>
    /// <param name="owner">The entities that create this chat.</param>
    /// <param name="partecipant">The entities that can send messages in this chat.</param>
    /// <param name="creationTime">The time when this chat is created.</param>
    public PrivateChat(Guid id, Guid owner, Guid partecipant, Timestamp creationTime)
    {
        Id = id;
        Owner = owner;
        Partecipant = partecipant;
        CreationTime = creationTime;
    }

    /// <summary>
    /// Initialize a new Chat.
    /// </summary>
    /// <param name="id">The id of this chat.</param>
    /// <param name="owner">The owner of this chat.</param>
    /// <param name="partecipant">The partecipant of this chat.</param>
    /// <param name="creationTime">The time when chat is created.</param>
    /// <returns>The chat.</returns>
    public static PrivateChat Create(Guid id, Guid owner, Guid partecipant, Timestamp creationTime) =>
        new(id, owner, partecipant, creationTime);

    /// <summary>
    /// The unique identifier of this chat.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// The unique identifier of the owner of this chat.
    /// </summary>
    public Guid Owner { get; }

    /// <summary>
    /// The unique identifier of the partecipant of this chat.
    /// </summary>
    public Guid Partecipant { get; }

    /// <summary>
    /// The dateTime when this chat is created.
    /// </summary>
    public Timestamp CreationTime { get; }
}
