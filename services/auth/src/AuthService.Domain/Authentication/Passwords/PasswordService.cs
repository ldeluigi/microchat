using AuthService.Domain.Aggregates.AccountAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace AuthService.Domain.Authentication.Passwords;

public record IncorrectPassword : DomainError;

public record PasswordRecoveryNotStarted : DomainError;

public class PasswordService
{
    private readonly IHashingService _hashingService;

    public PasswordService(IHashingService hashingService)
    {
        _hashingService = hashingService;
    }

    public Result<Nothing> ChangePassword(Account account, PlainTextPassword oldPassword, PlainTextPassword newPassword)
    {
        return VerifyOldPassword(account, oldPassword)
            .IfSuccess(_ => UpdatePassword(account, newPassword));
    }

    private Result<Nothing> VerifyOldPassword(Account account, PlainTextPassword password)
    {
        return RequireTrue(
            _hashingService.IsCorrectPassword(password, account.PasswordHash),
            () => new IncorrectPassword());
    }

    private void UpdatePassword(Account account, PlainTextPassword newPassword) =>
        account.UpdatePassword(_hashingService.GenerateHash(newPassword));
}
