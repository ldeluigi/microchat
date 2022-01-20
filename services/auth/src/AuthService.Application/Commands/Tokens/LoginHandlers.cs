using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication;
using AuthService.Domain.Authentication.Accounts;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Domain.Model;

namespace AuthService.Application.Commands.Tokens;

public static class LoginHandlers
{
    public record EmailLoginCommand(string Email, string Password) : CommandBase<AuthenticationResult>;

    public record UsernameLoginCommand(string Username, string Password) : CommandBase<AuthenticationResult>;

    public class EmailLoginHandler : LoginHandlerBase<AccountEmailCredentials, EmailLoginCommand>
    {
        public EmailLoginHandler(
            IAccountRepository accountRepository,
            LoginService loginService,
            AccountAuthenticationMethod accountLogin,
            IUnitOfWork unitOfWork)
            : base(loginService, accountRepository, accountLogin, unitOfWork)
        {
        }

        protected override AccountEmailCredentials GetCredentials(EmailLoginCommand request) =>
            new(Email.From(request.Email), PlainTextPassword.From(request.Password));
    }

    public class UsernameLoginHandler : LoginHandlerBase<AccountUsernameCredentials, UsernameLoginCommand>
    {
        public UsernameLoginHandler(
            IAccountRepository accountRepository,
            LoginService loginService,
            AccountAuthenticationMethod accountLogin,
            IUnitOfWork unitOfWork)
            : base(loginService, accountRepository, accountLogin, unitOfWork)
        {
        }

        protected override AccountUsernameCredentials GetCredentials(UsernameLoginCommand request) =>
            new(Username.From(request.Username), PlainTextPassword.From(request.Password));
    }
}
