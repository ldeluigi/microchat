using System;
using AuthService.Domain.Authentication.Accounts;
using EasyDesk.CleanArchitecture.Application.Mediator.Handlers;
using EasyDesk.CleanArchitecture.Application.Messaging;

namespace AuthService.Application.Events.Domain.PropagatedEvents;

public record AccountCreated(Guid AccountId, string Username) : IMessage;

public record AccountDeleted(Guid AccountId) : IMessage;

public class PropagateAccountRegisteredEvent : IDomainEventPropagator<AccountRegisteredEvent>
{
    public IMessage ConvertToMessage(AccountRegisteredEvent ev) =>
        new AccountCreated(ev.Account.Id, ev.Account.Username);
}

public class PropagateAccountUnregisteredEvent : IDomainEventPropagator<AccountUnregisteredEvent>
{
    public IMessage ConvertToMessage(AccountUnregisteredEvent ev) =>
        new AccountDeleted(ev.Account.Id);
}
