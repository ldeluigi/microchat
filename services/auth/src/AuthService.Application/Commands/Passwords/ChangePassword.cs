using System;
using System.Threading.Tasks;
using AuthService.Application.Queries.Accounts;
using AuthService.Domain.Authentication.Accounts;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using FluentValidation;

namespace AuthService.Application.Commands.Passwords;

public static class ChangePassword
{
    public record Command(string OldPassword, string NewPassword, Guid AccountId) : CommandBase<AccountOutput>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.NewPassword).NotEqual(x => x.OldPassword);
            RuleFor(c => c.NewPassword).MinimumLength(PlainTextPassword.MinimumLength);
        }
    }

    public class Handler : RequestHandlerBase<Command, AccountOutput>
    {
        private readonly AccountLifecycleService _accountLifecycleService;

        public Handler(
            AccountLifecycleService accountLifecycleService)
        {
            _accountLifecycleService = accountLifecycleService;
        }

        protected override async Task<Response<AccountOutput>> Handle(Command request)
        {
            // TODO: add check for authorization
            return await _accountLifecycleService
                .UpdatePassword(request.AccountId, PlainTextPassword.From(request.OldPassword), PlainTextPassword.From(request.NewPassword))
                .ThenMap(AccountOutput.From)
                .ThenToResponse();
        }
    }
}
