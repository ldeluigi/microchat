using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Model;

namespace AuthService.Domain.Aggregates.AccountAggregate.Events;

public record EmailChangedEvent(Account Account, Email OldEmail) : DomainEvent;
