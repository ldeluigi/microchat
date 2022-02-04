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
    private User(Guid id)
    {
        Id = id;
    }

    public static User Create(Guid id) => new(id);

    public Guid Id { get; }

    public Option<Timestamp> LastLogin { get; private set; } = None;

    public Result<Nothing> UpdateLastLoginTo(Timestamp timestamp)
    {
        if (LastLogin.All(t => t <= timestamp))
        {
            LastLogin = Some(timestamp);
            return Ok;
        }
        else
        {
            return new NewLoginPrecedesLastLogin(LastLogin.Value, timestamp);
        }
    }
}
