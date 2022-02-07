using ChatService.Domain.Chat;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Domain.Aggregates.PrivateChatAggregate;

public class PrivateChat : AggregateRoot, IChat
{
    public PrivateChat(Guid id, Option<Guid> creator, Option<Guid> partecipant, Timestamp creationTime)
    {
        Id = id;
        CreatorId = creator;
        PartecipantId = partecipant;
        CreationTime = creationTime;
    }

    public static PrivateChat Create(
        Guid id,
        Guid creator,
        Guid partecipant,
        Timestamp creationTime) =>
        new(id, Some(creator), Some(partecipant), creationTime);

    public Guid Id { get; }

    public Option<Guid> CreatorId { get; }

    public Option<Guid> PartecipantId { get; }

    public Timestamp CreationTime { get; }
}
