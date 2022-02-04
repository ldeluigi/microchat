using System;
using System.Linq;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Domain.Aggregates.MessageAggregate;

public record NewLoginPrecedesLastLogin(Timestamp LastLogin, Timestamp NewLogin) : DomainError;

public class User : AggregateRoot
{
    public User(Guid id, Option<Timestamp> lastSeenTimestamp)
    {
        Id = id;
        LastSeenTimestamp = lastSeenTimestamp;
    }

    public static User Create(Guid id) => new(id, None);

    public Guid Id { get; }

    public Option<Timestamp> LastSeenTimestamp { get; private set; } = None;

    public Result<Nothing> UpdateLastSeenTo(Timestamp timestamp)
    {
        if (LastSeenTimestamp.All(t => t <= timestamp))
        {
            LastSeenTimestamp = Some(timestamp);
            return Ok;
        }
        else
        {
            return new NewLoginPrecedesLastLogin(LastSeenTimestamp.Value, timestamp);
        }
    }
}
