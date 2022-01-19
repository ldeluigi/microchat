using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication;
using AuthService.Domain.Authentication.Accounts;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Tokens;

public abstract class LoginHandlerBase<TCredentials, TRequest> : UnitOfWorkHandler<TRequest, AuthenticationResult>
    where TRequest : CommandBase<AuthenticationResult>
{
    private readonly LoginService _loginService;
    private readonly IAccountRepository _accountRepository;
    private readonly ILoginMethod<TCredentials> _loginMethod;

    public LoginHandlerBase(
        LoginService loginService,
        IAccountRepository userRepository,
        ILoginMethod<TCredentials> loginMethod,
        IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _loginService = loginService;
        _accountRepository = userRepository;
        _loginMethod = loginMethod;
    }

    protected override async Task<Response<AuthenticationResult>> HandleRequest(TRequest request)
    {
        var credentials = GetCredentials(request);
        return await _loginService.Login(credentials, _loginMethod)
            .ThenIfSuccess(loginResult => _accountRepository.Save(loginResult.Account))
            .ThenMap(loginResult => loginResult.Result)
            .ThenToResponse();
    }

    protected abstract TCredentials GetCredentials(TRequest request);
}
