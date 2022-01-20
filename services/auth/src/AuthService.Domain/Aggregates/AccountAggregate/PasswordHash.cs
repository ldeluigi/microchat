namespace AuthService.Domain.Aggregates.AccountAggregate;

public record PasswordHash(string Password, string Salt);
