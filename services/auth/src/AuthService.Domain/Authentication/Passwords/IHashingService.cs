using AuthService.Domain.Aggregates.AccountAggregate;

namespace AuthService.Domain.Authentication.Passwords;

public interface IHashingService
{
    bool IsCorrectPassword(PlainTextPassword password, PasswordHash passwordHash);

    PasswordHash GenerateHash(PlainTextPassword password);
}
