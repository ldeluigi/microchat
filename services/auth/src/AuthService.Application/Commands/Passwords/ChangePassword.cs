using System;
using System.Threading.Tasks;
using AuthService.Application.Queries.Accounts.Outputs;
using AuthService.Domain.Authentication.Accounts;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Application.Authorization;
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
            RuleFor(c => c.NewPassword).NotEmpty().MinimumLength(PlainTextPassword.MinimumLength);
            RuleFor(c => c.NewPassword).NotEqual(x => x.OldPassword)
                .WithMessage("New password must be different from old password.");
            RuleFor(c => c.OldPassword).NotEmpty();
        }
    }

    public class Handler : RequestHandlerBase<Command, AccountOutput>
    {
        private readonly IUserInfoProvider _userInfoProvider;
        private readonly AccountLifecycleService _accountLifecycleService;

        public Handler(
            IUserInfoProvider userInfoProvider,
            AccountLifecycleService accountLifecycleService)
        {
            _userInfoProvider = userInfoProvider;
            _accountLifecycleService = accountLifecycleService;
        }

        protected override async Task<Response<AccountOutput>> Handle(Command request)
        {
            if (_userInfoProvider.RequireUserId() != request.AccountId)
            {
                return ResponseImports.Failure<AccountOutput>(new NotFoundError());
            }
            return await _accountLifecycleService
                .UpdatePassword(request.AccountId, PlainTextPassword.From(request.OldPassword), PlainTextPassword.From(request.NewPassword))
                .ThenMap(AccountOutput.From)
                .ThenToResponse();
        }
    }
}
