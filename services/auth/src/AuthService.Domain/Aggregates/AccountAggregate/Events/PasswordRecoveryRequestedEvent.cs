using AuthService.Domain.Tokens;
using EasyDesk.CleanArchitecture.Domain.Metamodel;

namespace AuthService.Domain.Aggregates.AccountAggregate.Events;

public record PasswordRecoveryRequestedEvent(Account Account, TokenInfo Token) : DomainEvent;
