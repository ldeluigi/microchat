using AuthService.Domain.Authentication.Accounts;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools;
using System;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Emails;

public static class ChangeEmail
{
    public record Command(Guid UserId, string Email) : CommandBase<Nothing>;

    public class Handler : UnitOfWorkHandler<Command, Nothing>
    {
        private readonly AccountLifecycleService _accountLifecycle;

        public Handler(
            AccountLifecycleService accountLifecycle,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _accountLifecycle = accountLifecycle;
        }

        protected override async Task<Response<Nothing>> HandleRequest(Command request)
        {
            return await _accountLifecycle
                .UpdateEmail(request.UserId, Email.From(request.Email))
                .ThenToResponse();
        }
    }
}
