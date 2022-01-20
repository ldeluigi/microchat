using System;
using System.Threading.Tasks;
using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools;
using EasyDesk.Tools.Options;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;
using static EasyDesk.Tools.Options.OptionImports;

namespace AuthService.Domain.Authentication.Accounts;

public record AccountEmailCredentials(Email Email, PlainTextPassword Password);

public record AccountUsernameCredentials(Username Username, PlainTextPassword Password);

public record LoginFailed : DomainError;

public class AccountAuthenticationMethod : ILoginMethod<AccountEmailCredentials>, ILoginMethod<AccountUsernameCredentials>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IHashingService _hashingService;

    public AccountAuthenticationMethod(
        IAccountRepository accountRepository,
        IHashingService hashingService)
    {
        _accountRepository = accountRepository;
        _hashingService = hashingService;
    }

    public async Task<Result<Guid>> VerifyCredentials(AccountEmailCredentials credentials)
    {
        var account = await _accountRepository.GetByEmail(credentials.Email);
        return account.Match(
            success: a => VerifyPassword(a, credentials.Password).Map(_ => a.Id),
            failure: _ => new LoginFailed());
    }

    public async Task<Result<Guid>> VerifyCredentials(AccountUsernameCredentials credentials)
    {
        var account = await _accountRepository.GetByUsername(credentials.Username);
        return account.Match(
            success: a => VerifyPassword(a, credentials.Password).Map(_ => a.Id),
            failure: _ => new LoginFailed());
    }

    private Result<Nothing> VerifyPassword(Account account, PlainTextPassword password) =>
        RequireTrue(_hashingService.IsCorrectPassword(password, account.PasswordHash), () => new LoginFailed());
}
