using System;
using System.Threading.Tasks;
using AuthService.Application.Queries.Accounts;
using AuthService.Domain.Authentication.Accounts;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using FluentValidation;

namespace AuthService.Application.Commands.Emails;

public static class ChangeEmail
{
    public record Command(Guid AccountId, string Email) : CommandBase<AccountOutput>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).Matches(Email.Pattern);
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
            return await _accountLifecycle
                .UpdateEmail(request.AccountId, Email.From(request.Email))
                .ThenMap(AccountOutput.From)
                .ThenToResponse();
        }
    }
}
