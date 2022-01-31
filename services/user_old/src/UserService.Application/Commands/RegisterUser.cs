using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using Microchat.UserService.Application.Queries;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Aggregates.UserService;

namespace UserService.Application.Commands;

public class RegisterUser
{
    public record Command(
        Guid UserId,
        string Name,
        string Surname,
        string Email) : CommandBase<UserOutput>;

    public class Handler : UnitOfWorkHandler<Command, UserOutput>
    {
        private readonly UserLifecycleService _userLifecycleService;

        public Handler(
            UserLifecycleService accountLifecycleService,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userLifecycleService = accountLifecycleService;
        }

        protected override async Task<Response<UserOutput>> HandleRequest(Command request)
        {
            return await _userLifecycleService
                .Register(
                id: request.UserId,
                name: Name.From(request.Name),
                surname: Name.From(request.Surname),
                email: Email.From(request.Email))
                .ThenMap(UserOutput.From)
                .ThenToResponse();
        }
    }
}
