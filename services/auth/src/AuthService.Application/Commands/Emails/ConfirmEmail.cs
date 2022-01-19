using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Emails;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Emails;

public static class ConfirmEmail
{
    public record Command(string Token) : CommandBase<Nothing>;

    public class Handler : UnitOfWorkHandler<Command, Nothing>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly EmailManagementService _emailManagementService;

        public Handler(
            IAccountRepository accountRepository,
            EmailManagementService emailManagementService,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _accountRepository = accountRepository;
            _emailManagementService = emailManagementService;
        }

        protected override async Task<Response<Nothing>> HandleRequest(Command request)
        {
            var token = Token.From(request.Token);
            return await _accountRepository.GetByConfirmationToken(token)
                .ThenRequire(user => _emailManagementService.ConfirmEmail(user, token))
                .ThenIfSuccess(user => _accountRepository.Save(user))
                .ThenToResponse();
        }
    }
}
