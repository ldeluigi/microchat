using AuthService.Domain.Aggregates.AccountAggregate.Events;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Messaging;
using System;

namespace AuthService.Application.Events.Domain.PropagatedEvents;

public record UsernameChanged(Guid AccountId, string Username) : IMessage;

public class PropagateUsernameUpdates : DomainEventPropagator<UsernameChangedEvent>
{
    public PropagateUsernameUpdates(MessageBroker broker) : base(broker)
    {
    }

    protected override IMessage ConvertToMessage(UsernameChangedEvent ev) =>
        new UsernameChanged(ev.Account.Id, ev.Account.Username);
}
