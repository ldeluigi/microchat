using System;
using System.Threading.Tasks;
using AuthService.Application.Queries.Accounts.Outputs;
using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication.Accounts;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.Options;
using FluentValidation;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace AuthService.Application.Commands.Accounts;

public static class UpdateAccount
{
    public record Command(Guid AccountId, Option<string> Username, Option<string> Email) : CommandBase<AccountOutput>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Username.OrElseNull())
                .MinimumLength(Username.MinimumLenght)
                .Matches(Username.Pattern)
                .When(x => x.Username.IsPresent);

            RuleFor(x => x.Email.OrElseNull())
                .Matches(Email.Pattern)
                .When(x => x.Email.IsPresent);

            RuleFor(x => x)
                .Must(x => x.Username.IsPresent || x.Email.IsPresent);
        }
    }

    public class Handler : RequestHandlerBase<Command, AccountOutput>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly AccountLifecycleService _accountLifecycle;
        private readonly IUserInfoProvider _userInfoProvider;

        public Handler(
            IAccountRepository accountRepository,
            AccountLifecycleService accountLifecycle,
            IUserInfoProvider userInfoProvider)
        {
            _accountRepository = accountRepository;
            _accountLifecycle = accountLifecycle;
            _userInfoProvider = userInfoProvider;
        }

        protected override async Task<Response<AccountOutput>> Handle(Command request)
        {
            if (_userInfoProvider.RequireUserId() != request.AccountId)
            {
                return ResponseImports.Failure<AccountOutput>(new NotFoundError());
            }
            return await _accountRepository.GetById(request.AccountId)
                .ThenMapAsync(async account =>
                {
                    await request.Username.IfPresentAsync(async username =>
                        await _accountLifecycle.UpdateUsername(account, Username.From(username)));
                    return account;
                })
                .ThenMapAsync(async account =>
                {
                    await request.Email.IfPresentAsync(async email =>
                        await _accountLifecycle.UpdateEmail(account, Email.From(email)));
                    return account;
                })
                .ThenMap(AccountOutput.From)
                .ThenToResponse();
        }
    }
}
