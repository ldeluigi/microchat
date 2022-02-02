using System;
using AuthService.Domain.Authentication.Accounts;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Messaging;

namespace AuthService.Application.Events.Domain.PropagatedEvents;

public record AccountCreated(Guid AccountId, string Username) : IMessage;

public record AccountDeleted(Guid AccountId) : IMessage;

public class PropagateAccountRegisteredEvent : DomainEventPropagator<AccountRegisteredEvent>
{
    public PropagateAccountRegisteredEvent(MessageBroker messageBroker) : base(messageBroker)
    {
    }

    protected override IMessage ConvertToMessage(AccountRegisteredEvent ev) =>
        new AccountCreated(ev.Account.Id, ev.Account.Username);
}

public class PropagateAccountUnregisteredEvent : DomainEventPropagator<AccountUnregisteredEvent>
{
    public PropagateAccountUnregisteredEvent(MessageBroker publisher) : base(publisher)
    {
    }

    protected override IMessage ConvertToMessage(AccountUnregisteredEvent ev) =>
        new AccountDeleted(ev.Account.Id);
}
