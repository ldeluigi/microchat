using AuthService.Domain.Aggregates.AccountAggregate.Events;
using EasyDesk.CleanArchitecture.Application.Events.ExternalEvents;
using EasyDesk.CleanArchitecture.Application.Mediator;
using System;

namespace AuthService.Application.Events.Domain.PropagatedEvents;

public record PasswordChanged(Guid UserId) : ExternalEvent;

public record PasswordRecoveryRequested(Guid UserId, string Token) : ExternalEvent;

public class PropagatePasswordChangedEvent : DomainEventPropagator<PasswordChangedEvent>
{
    public PropagatePasswordChangedEvent(IExternalEventPublisher publisher) : base(publisher)
    {
    }

    protected override ExternalEvent ConvertToExternalEvent(PasswordChangedEvent ev) =>
        new PasswordChanged(ev.Account.Id);
}

public class PropagatePasswordRecoveryRequestedEvent : DomainEventPropagator<PasswordRecoveryRequestedEvent>
{
    public PropagatePasswordRecoveryRequestedEvent(IExternalEventPublisher publisher) : base(publisher)
    {
    }

    protected override ExternalEvent ConvertToExternalEvent(PasswordRecoveryRequestedEvent ev) =>
        new PasswordRecoveryRequested(ev.Account.Id, ev.Token.Value);
}
