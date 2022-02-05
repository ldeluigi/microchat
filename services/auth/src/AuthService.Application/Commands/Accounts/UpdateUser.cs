using System;
using System.Threading.Tasks;
using AuthService.Application.Queries.Accounts;
using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication.Accounts;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools;
using EasyDesk.Tools.Options;
using FluentValidation;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace AuthService.Application.Commands.Accounts;

public static class UpdateUser
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
        private readonly AccountLifecycleService _accountLifecycle;

        public Handler(
            AccountLifecycleService accountLifecycle)
        {
            _accountLifecycle = accountLifecycle;
        }

        protected override async Task<Response<AccountOutput>> Handle(Command request)
        {
            // TODO: add check for authorization
            await request.Username.IfPresentAsync(async username =>
                    await _accountLifecycle
                        .UpdateUsername(request.AccountId, Username.From(username)));
            return await Task.FromResult(request.Email)
                .ThenFlatMapAsync<string, Result<Account>>(async email =>
                    await _accountLifecycle
                        .UpdateEmail(request.AccountId, Email.From(email)))
                .Map(x => x.Value)
                .ThenMap(AccountOutput.From)
                .ThenToResponse();
        }
    }
}
