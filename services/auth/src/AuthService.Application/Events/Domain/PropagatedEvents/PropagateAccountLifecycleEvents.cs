using AuthService.Domain.Authentication.Accounts;
using EasyDesk.CleanArchitecture.Application.ExternalEvents;
using EasyDesk.CleanArchitecture.Application.Mediator;
using System;

namespace AuthService.Application.Events.Domain.PropagatedEvents;

public record UserCreated(Guid UserId) : ExternalEvent;

public record UserDeleted(Guid UserId) : ExternalEvent;

public class PropagateAccountRegisteredEvent : DomainEventPropagator<AccountRegisteredEvent>
{
    public PropagateAccountRegisteredEvent(IExternalEventPublisher publisher) : base(publisher)
    {
    }

    protected override ExternalEvent ConvertToExternalEvent(AccountRegisteredEvent ev) =>
        new UserCreated(ev.Account.Id);
}

public class PropagateAccountUnregisteredEvent : DomainEventPropagator<AccountUnregisteredEvent>
{
    public PropagateAccountUnregisteredEvent(IExternalEventPublisher publisher) : base(publisher)
    {
    }

    protected override ExternalEvent ConvertToExternalEvent(AccountUnregisteredEvent ev) =>
        new UserDeleted(ev.Account.Id);
}
