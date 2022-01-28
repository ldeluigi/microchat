﻿using AuthService.Domain.Aggregates.AccountAggregate.Events;
using EasyDesk.CleanArchitecture.Application.Events.ExternalEvents;
using EasyDesk.CleanArchitecture.Application.Mediator;
using System;

namespace AuthService.Application.Events.Domain.PropagatedEvents;

public record EmailChanged(Guid UserId, string OldEmail, string NewEmail) : ExternalEvent;

public record AccountVerificationRequested(Guid UserId, string Token) : ExternalEvent;

public class PropagateEmailChangedEvent : DomainEventPropagator<EmailChangedEvent>
{
    public PropagateEmailChangedEvent(IExternalEventPublisher publisher) : base(publisher)
    {
    }

    protected override ExternalEvent ConvertToExternalEvent(EmailChangedEvent ev) =>
        new EmailChanged(ev.Account.Id, ev.OldEmail, ev.Account.Email);
}

public class PropagateEmailConfirmationRequiredEvent : DomainEventPropagator<EmailConfirmationRequiredEvent>
{
    public PropagateEmailConfirmationRequiredEvent(IExternalEventPublisher publisher) : base(publisher)
    {
    }

    protected override ExternalEvent ConvertToExternalEvent(EmailConfirmationRequiredEvent ev) =>
        new AccountVerificationRequested(ev.Account.Id, ev.Token.Value);
}