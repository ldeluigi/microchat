using System.Threading.Tasks;
using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication;
using AuthService.Domain.Authentication.Accounts;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using FluentValidation;

namespace AuthService.Application.Commands.Tokens;

public static class Refresh
{
    [AllowUnknownUser]
    public record Command(string RefreshToken, string AccessToken) : CommandBase<AuthenticationResult>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
            RuleFor(x => x.AccessToken).NotEmpty();
        }
    }

    public class Handler : RequestHandlerBase<Command, AuthenticationResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly RefreshService _refreshService;

        public Handler(
            IAccountRepository accountRepository,
            RefreshService refreshService)
        {
            _accountRepository = accountRepository;
            _refreshService = refreshService;
        }

        protected override async Task<Response<AuthenticationResult>> Handle(Command request)
        {
            var refreshToken = Token.From(request.RefreshToken);
            return await _accountRepository.GetByRefreshToken(refreshToken)
                .ThenFlatMap(account => _refreshService
                    .Refresh(account, Token.From(request.AccessToken), refreshToken)
                    .IfSuccess(_ => _accountRepository.Save(account)))
                .ThenToResponse();
        }
    }
}
