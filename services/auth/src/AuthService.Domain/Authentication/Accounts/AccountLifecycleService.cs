using System;
using System.Threading.Tasks;
using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Aggregates.AccountAggregate.Events;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.CleanArchitecture.Domain.Time;
using EasyDesk.Tools;
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
    private readonly PasswordService _passwordService;
    private readonly IRegistrationMethod<AccountRegistrationData> _registrationMethod;

    public AccountLifecycleService(
        IAccountRepository accountRepository,
        ITimestampProvider timestampProvider,
        IDomainEventNotifier eventNotifier,
        PasswordService passwordService,
        AccountRegistrationMethod registrationMethod)
    {
        _accountRepository = accountRepository;
        _timestampProvider = timestampProvider;
        _eventNotifier = eventNotifier;
        _passwordService = passwordService;
        _registrationMethod = registrationMethod;
    }

    public async Task<Result<Account>> Register(Email email, Username username, PlainTextPassword password)
    {
        return await VerifyEmailAndUsernameAreNotTaken(email, username)
            .ThenFlatMapAsync(_ => _registrationMethod.CreateAccount(
                new AccountRegistrationData(username, email, password, _timestampProvider.Now))
            .ThenIfSuccess(account => _accountRepository.Save(account))
            .ThenIfSuccess(account => _eventNotifier.Notify(new AccountRegisteredEvent(account))));
    }

    public async Task<Result<Account>> UpdateEmail(Guid guid, Email newEmail)
    {
        return await VerifyEmailIsNotTaken(newEmail)
            .ThenFlatMapAsync(_ => _accountRepository.GetById(guid))
            .ThenIfSuccess(account =>
            {
                var oldEmail = account.Email;
                account.UpdateEmail(newEmail);
                _accountRepository.Save(account);
                _eventNotifier.Notify(new EmailChangedEvent(account, oldEmail));
            });
    }

    public async Task<Result<Account>> UpdatePassword(Guid guid, PlainTextPassword oldPassword, PlainTextPassword newPassword)
    {
        return await _accountRepository
            .GetById(guid)
                .ThenRequire(account => _passwordService
                    .ChangePassword(account, PlainTextPassword.From(oldPassword), PlainTextPassword.From(newPassword)))
                .ThenIfSuccess(account => _accountRepository.Save(account));
    }

    private async Task<Result<Nothing>> VerifyEmailIsNotTaken(Email newEmail)
    {
        if (await _accountRepository.EmailExists(newEmail))
        {
            return new EmailAlreadyInUse();
        }
        return Ok;
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

    public async Task<Result<Account>> Unregister(Guid accountId)
    {
        return await _accountRepository
            .GetById(accountId)
            .ThenIfSuccess(account => _accountRepository.Remove(account))
            .ThenIfSuccess(account => _eventNotifier.Notify(new AccountUnregisteredEvent(account)));
    }
}
