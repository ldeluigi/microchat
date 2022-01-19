using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication.Passwords;
using AuthService.Domain.Emails;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.CleanArchitecture.Domain.Time;
using EasyDesk.Tools;
using System;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;
using static EasyDesk.Tools.Options.OptionImports;

namespace AuthService.Domain.Authentication.Accounts;

public record EmailAlreadyInUse : DomainError;

public record UsernameAlreadyInUse : DomainError;

public record AccountRegisteredEvent(Account Account) : DomainEvent;

public record AccountUnregisteredEvent(Account Account) : DomainEvent;

public class AccountLifecycleService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITimestampProvider _timestampProvider;
    private readonly IDomainEventNotifier _eventNotifier;
    private readonly EmailManagementService _emailManagementService;
    private readonly IRegistrationMethod<AccountRegistrationData> _registrationMethod;

    public AccountLifecycleService(
        IAccountRepository accountRepository,
        ITimestampProvider timestampProvider,
        IDomainEventNotifier eventNotifier,
        EmailManagementService emailManagementService,
        AccountRegistrationMethod registrationMethod)
    {
        _accountRepository = accountRepository;
        _timestampProvider = timestampProvider;
        _eventNotifier = eventNotifier;
        _emailManagementService = emailManagementService;
        _registrationMethod = registrationMethod;
    }

    public async Task<Result<Account>> Register(Email email, Username username, PlainTextPassword password)
    {
        return await VerifyEmailAndUsernameAreNotTaken(email, username)
            .ThenFlatMapAsync(_ => _registrationMethod.CreateAccount(new AccountRegistrationData(username, email, password, _timestampProvider.Now))
            .ThenIfSuccess(_emailManagementService.GenerateEmailConfirmationTokenForRegisteredUser)
            .ThenIfSuccess(account => _accountRepository.Save(account))
            .ThenIfSuccess(account => _eventNotifier.Notify(new AccountRegisteredEvent(account))));
    }

    private async Task<Result<Nothing>> VerifyEmailAndUsernameAreNotTaken(Email email, Username username)
    {
        var (emailExists, usernameExists) = await _accountRepository.EmailOrUsernameExists(email, username);
        if (emailExists)
        {
            return new EmailAlreadyInUse();
        }
        if (usernameExists)
        {
            return new UsernameAlreadyInUse();
        }
        return Ok;
    }

    public async Task<Result<Account>> Unregister(Guid userId)
    {
        return await _accountRepository
            .GetById(userId)
            .ThenIfSuccess(user => _accountRepository.Remove(user))
            .ThenIfSuccess(user => _eventNotifier.Notify(new AccountUnregisteredEvent(user)));
    }
}
