﻿using System.Threading.Tasks;
using AuthService.Domain.Aggregates.AccountAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace AuthService.Domain.Authentication.Accounts;

public record EmailNotConfirmed : DomainError;

public class LoginService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthenticationService _authenticationService;

    public LoginService(IAccountRepository accountRepository, IAuthenticationService authenticationService)
    {
        _accountRepository = accountRepository;
        _authenticationService = authenticationService;
    }

    public async Task<Result<(AuthenticationResult Result, Account Account)>> Login<T>(T credentials, ILoginMethod<T> loginMethod)
    {
        return await loginMethod.VerifyCredentials(credentials)
            .ThenFlatMapAsync(userId => _accountRepository.GetById(userId))
            .ThenMap(account => (_authenticationService.GenerateAuthenticationResult(account), account));
    }
}
