using EasyDesk.CleanArchitecture.Domain.Metamodel;

namespace AuthService.Domain.Aggregates.AccountAggregate.Events;

public record UsernameChangedEvent(Account Account) : DomainEvent;
