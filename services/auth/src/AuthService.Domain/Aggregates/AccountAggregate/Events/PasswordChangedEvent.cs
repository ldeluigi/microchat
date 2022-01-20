using EasyDesk.CleanArchitecture.Domain.Metamodel;

namespace AuthService.Domain.Aggregates.AccountAggregate.Events;

public record PasswordChangedEvent(Account Account) : DomainEvent;
