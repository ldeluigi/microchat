using ChatService.Domain.Chat;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Domain.Aggregates.PrivateChatAggregate;

public class PrivateChat : AggregateRoot, IChat
{
    private PrivateChat(Guid id, Guid creator, Guid partecipant, Timestamp creationTime)
    {
        Id = id;
        Creator = creator;
        Partecipant = partecipant;
        CreationTime = creationTime;
    }

    public static PrivateChat Create(
        Guid id,
        Guid creator,
        Guid partecipant,
        Timestamp creationTime) =>
        new(id, creator, partecipant, creationTime);

    public Guid Id { get; }

    public Guid Creator { get; }

    public Guid Partecipant { get; }

    public Timestamp CreationTime { get; }
}
