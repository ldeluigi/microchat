using System;
using EasyDesk.CleanArchitecture.Domain.Metamodel;

namespace ChatService.Domain.Aggregates.MessageAggregate;

public class User : AggregateRoot
{
    public User(Guid id)
    {
        Id = id;
    }

    public static User Create(Guid id) => new(id);

    public Guid Id { get; }
}
