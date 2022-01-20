using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace AuthService.Domain.Authentication.Accounts;

public record AccountRegistrationData(Username Username, Email Email, PlainTextPassword Password, Timestamp Creation);

public class AccountRegistrationMethod : IRegistrationMethod<AccountRegistrationData>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IHashingService _hashingService;

    public AccountRegistrationMethod(
        IAccountRepository accountRepository,
        IHashingService hashingService)
    {
        _accountRepository = accountRepository;
        _hashingService = hashingService;
    }

    public Task<Result<Account>> CreateAccount(AccountRegistrationData accountData)
    {
        var passwordHash = _hashingService.GenerateHash(accountData.Password);
        var account = Account.Create(
            email: accountData.Email,
            passwordHash: passwordHash,
            username: accountData.Username,
            creation: accountData.Creation);

        _accountRepository.Save(account);

        return Task.FromResult(Success(account));
    }
}
