using System.Threading;
using System.Threading.Tasks;
using AuthService.Application.Commands.Tokens;
using AuthService.Application.Queries.Accounts.Outputs;
using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication.Accounts;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Mediator.Handlers;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using FluentValidation;

namespace AuthService.Application.Commands.Registration;

public static class RegisterAccount
{
    [AllowUnknownUser]
    public record Command(
        string Email,
        string Password,
        string Username) : CommandBase<AccountOutput>;

    public class Validator : PasswordValidatorBase<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().Matches(Email.Pattern);
            ValidatePassword(RuleFor(x => x.Password));
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(Username.MinimumLenght)
                .Matches(Username.Pattern);
        }
    }

    public class Handler : ICommandHandler<Command, AccountOutput>
    {
        private readonly AccountLifecycleService _accountLifecycleService;

        public Handler(
            AccountLifecycleService accountLifecycleService)
        {
            _accountLifecycleService = accountLifecycleService;
        }

        public async Task<Response<AccountOutput>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _accountLifecycleService
                .Register(
                    email: Email.From(request.Email),
                    username: Username.From(request.Username),
                    password: PlainTextPassword.From(request.Password))
                .ThenMap(AccountOutput.From)
                .ThenToResponse();
        }
    }
}
