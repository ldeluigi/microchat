using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Passwords;

public static class ChangePassword
{
    public record Command(string OldPassword, string NewPassword, Guid AccountId) : CommandBase<Nothing>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.NewPassword).NotEqual(x => x.OldPassword);
        }
    }

    public class Handler : UnitOfWorkHandler<Command, Nothing>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly PasswordService _passwordService;

        public Handler(
            IAccountRepository accountRepository,
            PasswordService passwordService,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _accountRepository = accountRepository;
            _passwordService = passwordService;
        }

        protected override async Task<Response<Nothing>> HandleRequest(Command request)
        {
            return await _accountRepository
                .GetById(request.AccountId)
                .ThenRequire(account => _passwordService.ChangePassword(account, PlainTextPassword.From(request.OldPassword), PlainTextPassword.From(request.NewPassword)))
                .ThenIfSuccess(account => _accountRepository.Save(account))
                .ThenToResponse();
        }
    }
}
