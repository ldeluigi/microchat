using System;
using System.Threading.Tasks;
using AuthService.Application.Queries.Accounts.Outputs;
using AuthService.Domain.Authentication.Accounts;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;

namespace AuthService.Application.Commands.Registration;

public static class UnregisterAccount
{
    public record Command(Guid AccountId) : CommandBase<AccountOutput>;

    public class Handler : RequestHandlerBase<Command, AccountOutput>
    {
        private readonly IUserInfoProvider _userInfoProvider;
        private readonly AccountLifecycleService _accountLifecycleService;

        public Handler(
            IUserInfoProvider userInfoProvider,
            AccountLifecycleService accountLifecycleService)
        {
            _userInfoProvider = userInfoProvider;
            _accountLifecycleService = accountLifecycleService;
        }

        protected override async Task<Response<AccountOutput>> Handle(Command request)
        {
            if (_userInfoProvider.RequireUserId() != request.AccountId)
            {
                return ResponseImports.Failure<AccountOutput>(new NotFoundError());
            }
            return await _accountLifecycleService.Unregister(request.AccountId)
                .ThenMap(AccountOutput.From)
                .ThenToResponse();
        }
    }
}
