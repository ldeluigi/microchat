using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Tokens;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.CleanArchitecture.Domain.Time;
using EasyDesk.CleanArchitecture.Domain.Utils;
using EasyDesk.Tools;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace AuthService.Domain.Authentication.Passwords;

public record IncorrectPassword : DomainError;

public record PasswordRecoveryNotStarted : DomainError;

public class PasswordService
{
    private readonly IHashingService _hashingService;
    private readonly ITimestampProvider _timestampProvider;
    private readonly Duration _tokenDuration;

    public PasswordService(IHashingService hashingService, ITimestampProvider timestampProvider, Duration tokenDuration)
    {
        _hashingService = hashingService;
        _timestampProvider = timestampProvider;
        _tokenDuration = tokenDuration;
    }

    public void StartPasswordRecovery(Account account)
    {
        var token = TokenInfo.Random(_timestampProvider.Now + _tokenDuration.AsTimeOffset);
        account.StartPasswordRecovery(token);
    }

    public Result<Nothing> AcceptPasswordRecovery(Account account, PlainTextPassword newPassword, Token token)
    {
        return ValidateToken(account, token)
            .IfSuccess(_ => UpdatePassword(account, newPassword));
    }

    private Result<Nothing> ValidateToken(Account account, Token token)
    {
        return account.PasswordRecoveryToken
            .OrElseError(() => new PasswordRecoveryNotStarted())
            .Require(t => t.Validate(token, _timestampProvider.Now));
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
