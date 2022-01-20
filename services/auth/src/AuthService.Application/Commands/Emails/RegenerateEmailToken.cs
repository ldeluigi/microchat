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

public static class RegenerateEmailToken
{
    public record Command(string Email) : CommandBase<Nothing>;

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
            return await _accountRepository.GetByEmail(Email.From(request.Email))
                .ThenRequire(user => _emailManagementService.RegenerateEmailConfirmationToken(user))
                .ThenIfSuccess(user => _accountRepository.Save(user))
                .ThenToResponse();
        }
    }
}
