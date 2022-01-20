using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication;
using AuthService.Domain.Authentication.Accounts;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Tokens;

public static class Refresh
{
    public record Command(string RefreshToken, string AccessToken) : CommandBase<AuthenticationResult>;

    public class Handler : UnitOfWorkHandler<Command, AuthenticationResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly RefreshService _refreshService;

        public Handler(
            IAccountRepository accountRepository,
            RefreshService refreshService,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _accountRepository = accountRepository;
            _refreshService = refreshService;
        }

        protected override async Task<Response<AuthenticationResult>> HandleRequest(Command request)
        {
            var refreshToken = Token.From(request.RefreshToken);
            return await _accountRepository.GetByRefreshToken(refreshToken)
                .ThenFlatMap(user => _refreshService
                    .Refresh(user, Token.From(request.AccessToken), refreshToken)
                    .IfSuccess(_ => _accountRepository.Save(user)))
                .ThenToResponse();
        }
    }
}
