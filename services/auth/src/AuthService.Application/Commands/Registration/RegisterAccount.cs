using AuthService.Application.Queries.Accounts;
using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication.Accounts;
using AuthService.Domain.Authentication.Passwords;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Registration;

public static class RegisterAccount
{
    public record Command(
        string Email,
        string Password,
        string Username) : CommandBase<AccountOutput>;

    public class Handler : UnitOfWorkHandler<Command, AccountOutput>
    {
        private readonly AccountLifecycleService _accountLifecycleService;

        public Handler(
            AccountLifecycleService accountLifecycleService,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _accountLifecycleService = accountLifecycleService;
        }

        protected override async Task<Response<AccountOutput>> HandleRequest(Command request)
        {
            return await _accountLifecycleService
                .Register(
                email: Email.From(request.Email),
                username: Username.From(request.Username),
                password: PlainTextPassword.From(request.Password))
                .ThenMap(AccountOutput.From)
                .ThenToResponse();
        }
    }
}
