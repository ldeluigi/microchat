using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using Microchat.UserService.Application.Queries;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserService;

namespace UserService.Application.Commands;

public class UnregisterUser
{
    public record Command(Guid UserId) : CommandBase<UserOutput>;

    public class Handler : RequestHandlerBase<Command, UserOutput>
    {
        private readonly UserLifecycleService _userLifecycleService;

        public Handler(UserLifecycleService userLifecycleService)
        {
            _userLifecycleService = userLifecycleService;
        }

        protected override async Task<Response<UserOutput>> Handle(Command request)
        {
            return await _userLifecycleService.Unregister(request.UserId)
                .ThenMap(UserOutput.From)
                .ThenToResponse();
        }
    }
}
