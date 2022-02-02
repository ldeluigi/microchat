using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication;
using AuthService.Domain.Authentication.Accounts;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Domain.Model;
using FluentValidation;

namespace AuthService.Application.Commands.Tokens;

public static class LoginHandlers
{
    [AllowUnknownUser]
    public record EmailLoginCommand(string Email, string Password) : CommandBase<AuthenticationResult>;

    public class EmailLoginValidator : PasswordValidatorBase<EmailLoginCommand>
    {
        public EmailLoginValidator()
        {
            RuleFor(x => x.Email).Matches(Email.Pattern);
            ValidatePassword(RuleFor(x => x.Password));
        }
    }

    [AllowUnknownUser]
    public record UsernameLoginCommand(string Username, string Password) : CommandBase<AuthenticationResult>;

    public class UsernameLoginValidator : PasswordValidatorBase<UsernameLoginCommand>
    {
        public UsernameLoginValidator()
        {
            RuleFor(x => x.Username).Matches(Username.Pattern);
            ValidatePassword(RuleFor(x => x.Password));
        }
    }

    public class EmailLoginHandler : LoginHandlerBase<AccountEmailCredentials, EmailLoginCommand>
    {
        public EmailLoginHandler(
            IAccountRepository accountRepository,
            LoginService loginService,
            AccountAuthenticationMethod accountLogin)
            : base(loginService, accountRepository, accountLogin)
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
            AccountAuthenticationMethod accountLogin)
            : base(loginService, accountRepository, accountLogin)
        {
        }

        protected override AccountUsernameCredentials GetCredentials(UsernameLoginCommand request) =>
            new(Username.From(request.Username), PlainTextPassword.From(request.Password));
    }
}
