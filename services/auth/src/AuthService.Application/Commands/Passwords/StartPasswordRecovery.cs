using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Application.Responses.ResponseImports;

namespace AuthService.Application.Commands.Passwords;

public static class StartPasswordRecovery
{
    public record Command(string Email) : CommandBase<Nothing>;

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
            var account = await _accountRepository.GetByEmail(Email.From(request.Email));
            ////Audit:
            ////if (account.IsFailure)
            ////{
            ////    await _publisher.Publish(new PasswordRecoveryRequested(request.Email, Name: null, Token: null));
            ////    return Ok;
            ////}
            account.IfSuccess(a =>
            {
                _passwordService.StartPasswordRecovery(a);
                _accountRepository.Save(a);
            });
            return Ok;
        }
    }
}
