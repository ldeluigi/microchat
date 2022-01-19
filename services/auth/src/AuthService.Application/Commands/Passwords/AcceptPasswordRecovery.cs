using System.Threading.Tasks;
using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools;

namespace AuthService.Application.Commands.Passwords;

public static class AcceptPasswordRecovery
{
    public record Command(string Token, string NewPassword) : CommandBase<Nothing>;

    public class Handler : UnitOfWorkHandler<Command, Nothing>
    {
        private readonly PasswordService _passwordService;
        private readonly IAccountRepository _accountRepository;

        public Handler(
            PasswordService passwordService,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _passwordService = passwordService;
            _accountRepository = accountRepository;
        }

        protected override async Task<Response<Nothing>> HandleRequest(Command request)
        {
            var token = Token.From(request.Token);
            return await _accountRepository.GetByPasswordRecoveryToken(token)
                .ThenRequire(account => _passwordService.AcceptPasswordRecovery(account, PlainTextPassword.From(request.NewPassword), token))
                .ThenIfSuccess(account => _accountRepository.Save(account))
                .ThenToResponse();
        }
    }
}
